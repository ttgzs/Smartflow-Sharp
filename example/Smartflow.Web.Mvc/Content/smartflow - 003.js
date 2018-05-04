/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://github.com/chengderen/Smartflow

 Author:chengderen-237552006@qq.com
 */
(function ($) {
    var
        //存储所有节点的实例
        NC = {},
        //所有线的实例
        LC = {},
        //维护关系
        RC = [],
        //全局变量
        draw,
        //连线ID
        fromConnect,
        //规则检查
        rule = {
        duplicateCheck: function (from, to) {
            //检查是否已经存在相同路线
            var result = false;
            for (var i = 0, len = RC.length; i < len; i++) {
                var r = RC[i];
                if (r.from === from && r.to === to) {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }


    //数组添加移除方法
    $.extend(Array.prototype, {
        remove: function (dx, to) {
            this.splice(dx, (to || 1));
        },
        each: function (cb) {
            if (typeof cb === 'function') {
                for (var i = 0, len = this.length; i < this.length; i++) {
                    cb.call(this[i], i, this[i]);
                }
            }
        }
    });

    //函数添加继承
    $.extend(Function.prototype, {
        extend: function (Parent, Override) {
            function F() { }
            F.prototype = Parent.prototype;
            this.prototype = new F();
            this.prototype.constructor = this;
            this.base = {};
            this.base.Parent = Parent;
            this.base.Constructor = Parent;
            if (Override) {
                $.extend(this.prototype, Override);
            }
        }
    });

    function bind(elementId) {
        draw = SVG(elementId);
        draw.mouseup(function (e) {
            draw.off('mousemove');
        });
        return draw;
    }

    function Element(name, category) {
        //节点ID
        this.id = undefined;
        //文本画笔
        this.brush = undefined;
        //节点中文名称
        this.name = name;
        //节点类别（LINE、NODE、START、END）
        this.category = category;
        //唯一标识
        this.uniqueId = undefined;
        //禁用事件
        this.disable = false;

        //背景颜色
        this.bgColor = '#f06';
        //悬停节点背影颜色
        this.bgOverColor = '#f8e233';
        //当前节点颜色
        this.bgCurrentColor= '#D6C80E';
    }

    Element.prototype = {
        constructor: Element,
        draw: function () {
            if (!this.disable) {
                this.bindEvent.apply(SVG.get(this.id), [this]);
            }
            return this.id;
        },
        bindEvent: function (o) {
            this.mouseout(function () {
                if (this.type === 'rect') {
                    this.attr({ fill: o.bgColor });
                } else {
                    this.stroke({ color: o.bgColor })
                }
            });
            this.mouseover(function (e) {
                if (this.type === 'rect') {
                    this.attr({ fill: o.bgOverColor });
                } else {
                    this.stroke({ color: o.bgOverColor })
                }
            });
        }
    };

    //定义线
    function Line() {
        this.x1 = 0;
        this.y1 = 0;
        this.x2 = 0;
        this.y2 = 0;
        this.border = 3;
        this.from = undefined;
        this.to = undefined;
        Line.base.Constructor.call(this, "line", "line");
    }

    Line.extend(Element, {
        constructor: Line,
        draw: function () {
            var self = this,
                l = draw.line(self.x1, self.y1, self.x2, self.y2);
            l.stroke({ width: self.border, color: self.bgColor });
            self.brush = draw.text(function (add) {
                add.tspan(self.name);
            });
            
            self.brush.attr({ x: (self.x2 - self.x1) / 2 + self.x1, y: (self.y2 - self.y1) / 2 + self.y1 });
            self.id = l.id();
            LC[self.id] = this;
            return Line.base.Parent.prototype.draw.call(self);
        },
        bindEvent: function (l) {
            this.dblclick(function (evt) {
                //删除
                var instance = LC[this.id()];
                if (evt.ctrlKey && evt.altKey) {
                    eachElements(instance.id);
                    this.off('dblclick');
                    this.remove();
                    instance.brush.remove();
                    delete LC[instance.id];
                } else {
                    var nodeName = prompt("请输入路线名称", instance.name);
                    if (nodeName) {
                        instance.name = nodeName;
                        instance.brush.clear();
                        instance.brush.text(function (add) {
                            add.tspan(instance.name);
                        });
                    }
                }
            });
            return Line.base.Parent.prototype.bindEvent.call(this,l);
        }
    });

    //定义节点
    function Node() {
        this.w = 180;
        this.h = 50;
        this.x = 10;
        this.y = 10;
        this.cx = 120;
        this.cy = 0;
        this.disX = 0;
        this.disY = 0;
        this.ox1 = 0;
        this.oy1 = 0;
        this.ox2 = 0;
        this.oy2 = 0;
        Node.base.Constructor.call(this, "node", "node");
    }

    Node.extend(Element, {
        draw: function (b) {
            var n = this,
                color = b ? n.bgCurrentColor : n.bgColor,
                rect = draw.rect(n.w, n.h).attr({ fill: color, x: n.x, y: n.y });

            n.brush = draw.text(function (add) {
                add.tspan(n.name);
            });
            n.brush.attr({ x: n.x + rect.width() / 2, y: n.y + rect.height() / 2 + 5 });

            n.id = rect.id();
            NC[n.id] = n;
            return Node.base.Parent.prototype.draw.call(this);
        },
        checkRule: function (nf) {
            var rule = ((nf.category === 'end' || this.category === 'start') ||
                        (nf.category === 'start' && this.category === 'end'));
            return rule;
        },
        bindEvent: function (n) {
            this.mousedown(OnDrag);
            this.dblclick(function (evt) {
                var node = NC[this.id()];
                node.edit.call(this, evt);
            });
            Node.base.Parent.prototype.bindEvent.call(this,n);
        },
        edit: function (evt) {

            if (evt.ctrlKey && evt.altKey) {
                var id = this.id(),
                    node = NC[id],
                    rect = SVG.get(id),
                    elements = findByElementId(id);

                delElement(elements);

                rect.remove();
                node.brush.remove();
                delete NC[id];

            } else {
                var nx = NC[this.id()],
                    nodeName = prompt("请输入节点名称", nx.name);

                if (nodeName) {
                    nx.name = nodeName;
                    nx.brush.clear();
                    nx.brush.text(function (add) {
                        add.tspan(nx.name);
                    });
                }
            }
        },
        move: function (element, d) {
            var self = this;
            self.x = d.clientX - self.disX - self.cx;
            self.y = d.clientY - self.disY - self.cy;
            element.attr({ x: self.x, y: self.y });

            if (self.brush) {
                self.brush.attr({ x: (element.x() + (element.width() / 2)), y: element.y() + (element.height() / 2) + 5 });
            }

            var toElements = findByElementId(self.id, "to"),
                fromElements = findByElementId(self.id, "from");

            $.each(toElements, function () {
                var lineElement = SVG.get(this.id),
                    instance = LC[this.id];

                if (lineElement && instance) {
                    instance.x2 = self.x + self.ox2;
                    instance.y2 = self.y + self.oy2;
                    lineElement.attr({ x2: instance.x2, y2: instance.y2 });
                    instance.brush.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
                }
            });

            $.each(fromElements, function () {
                var lineElement = SVG.get(this.id),
                    instance = LC[this.id];
                if (lineElement && instance) {
                    instance.x1 = self.x + self.ox1;
                    instance.y1 = self.y + self.oy1;
                    lineElement.attr({ x1: instance.x1, y1: instance.y1 });
                    instance.brush.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
                }
            });
        }
    });


    //决策节点
    function Decision() {
        Decision.base.Constructor.call(this);
        this.name = 'decision';
        this.category = 'decision';
        this.circles = [];
    }

    Decision.extend(Node, {
        draw: function () {
            Decision.base.Parent.prototype.draw.call(this);
            var y = this.y + this.h,
                w = this.w;
            //09dd1a
            for (var i = 0, len = w / 10; i <= len; i++) {
                var circle = draw.circle(10);
                circle.attr({ fill: '#fff', cx: this.x + i * 10, cy: y });
                this.circles.push(circle);
            }
        },
        edit: function (evt) {
            //删除
            if (evt.ctrlKey && evt.altKey) {
                var decision = NC[this.id()];
                $.each(decision.circles, function () {
                    this.remove();
                });
            }
            Decision.base.Parent.prototype.edit.call(this, evt);
        },
        move: function (element, evt) {
            Decision.base.Parent.prototype.move.call(this, element, evt);
            var self = this;
            //09dd1a
            $.each(self.circles, function (i) {
                this.attr({ fill: '#fff', cx: self.x + i * 10, cy: self.y + self.h });
            });
        },
        bindEvent: function (decision) {
            Decision.base.Parent.prototype.bindEvent.call(this, decision);
        }
    });

    //开始节点
    function Start() {
        Start.base.Constructor.call(this);
        this.category = "start";
        this.name = "开始";
    }

    Start.extend(Node, {
        draw: function () {
            Start.base.Parent.prototype.draw.call(this);
            var nid = this.id,
                rect = SVG.get(nid);

            rect.radius(10);
        },
        bindEvent: function (n) {
            Start.base.Parent.prototype.bindEvent.call(this, n);
            this.off('dblclick');
        }
    });

    //结束节点
    function End() {
        End.base.Constructor.call(this);
        this.category = "end";
        this.name = "结束";
    }

    End.extend(Node, {
        constructor: Start,
        draw: function () {
            End.base.Parent.prototype.draw.call(this);
            var nid = this.id,
                rect = SVG.get(nid);

            rect.radius(10);
        },
        bindEvent: function (n) {
            End.base.Parent.prototype.bindEvent.call(this,n);
            this.off('dblclick');
        }
    });

    //选择
    function select() {
        initEvent();
        draw.each(function (i, child) {
            if (this.type === 'rect') {
                this.mousedown(OnDrag);
            }
        });
    }

    //连接节点
    function connect() {
        initEvent();
        $(document).bind('mousedown', OnConnect);
        $(document).bind('mouseup', OnConnected);
    }

    //开始连线
    function OnConnect(evt) {
        var node = $(evt.target).get(0),
            nodeName = node.nodeName,
            nodeId = node.id;
        if (nodeName === 'rect') {
            fromConnect = {
                id: nodeId,
                x: evt.clientX
            }
        }
        evt.preventDefault();
        return false;
    }

    //结束连线
    function OnConnected(evt) {
        var node = $(evt.target).get(0),
            nodeName = node.nodeName,
            nodeId = node.id;

        if (nodeName === 'rect' && fromConnect) {

            var toRect = SVG.get(nodeId),
                fromRect = SVG.get(fromConnect.id),
                nt = NC[nodeId],
                nf = NC[fromConnect.id];

            if (nodeId !== fromConnect.id
                && !nt.checkRule(nf)
                && !rule.duplicateCheck(fromConnect.id, nodeId)) {

                var instance = new Line(),
                    orientation = checkOrientation(fromRect, toRect);

                if (orientation === 'down') {
                    instance.x1 = fromConnect.x - nf.cx;//fromRect.width() / 2 + fromRect.x();
                    instance.y1 = fromRect.height() + fromRect.y();
                    instance.x2 = evt.clientX - nt.cx;
                    instance.y2 = toRect.y();
                } else {

                    instance.x1 = fromConnect.x - nf.cx;
                    instance.y1 = fromRect.y();

                    instance.x2 = evt.clientX - nt.cx;
                    instance.y2 = toRect.height() + toRect.y();
                }
                instance.from = fromConnect.id;
                instance.to = toRect.id();
                instance.draw();

                var l = SVG.get(instance.id),
                    r = SVG.get(instance.from);

                RC.push({ id: instance.id, from: fromConnect.id, to: nodeId });
                nt.ox2 = l.attr("x2") - toRect.x();
                nt.oy2 = l.attr("y2") - toRect.y();
                nf.ox1 = l.attr("x1") - fromRect.x();
                nf.oy1 = l.attr("y1") - fromRect.y();
            }
        }
        fromConnect = undefined;
        evt.preventDefault();
        return true;
    }

    //初始化事件
    function initEvent() {
        $(document).unbind('mousedown');
        $(document).unbind('mouseup');
        draw.each(function (i, child) {
            if (this.type === 'rect') {
                this.off('mousedown');
            }
        });
    }

    function OnDrag(evt) {
        var self = this,
            nx = NC[self.id()];

        nx.disX = evt.clientX - self.x() - nx.cx;
        nx.disY = evt.clientY - self.y() - nx.cy;
        draw.on('mousemove', function (d) {
            nx.move(self, d);
        });
    }

    //删除与当前节点的连接线
    function delElement(elements) {
        for (var i = 0; i < elements.length; i++) {
            var element = elements[i];
            if (element) {
                var l = SVG.get(element.id),
                    instance = LC[element.id];

                l.remove();
                instance.brush.remove();
                eachElements(element.id);
                delete LC[element.id];
            }
        }
    }

    //清空与线交接另一头的引用
    function eachElements(id) {
        for (var i = 0, len = RC.length; i < len; i++) {
            if (RC[i].id == id) {
                RC.remove(i);
                break;
            }
        }
    }

    function findByElementId(elementId, propertyName) {
        var elements = [];
        $.each(RC, function () {
            var self = this;
            if (propertyName) {
                if (this[propertyName] === elementId) {
                    elements.push(this);
                }
            } else {
                $.each(["to", "from"], function () {
                    if (self[this] === elementId) {
                        elements.push(self);
                    }
                });
            }
        });
        return elements;
    }

    //导出数据
    function exportToJSON() {
        var uniqueId = 0,
            nodeCollection = [],
            pathCollection = [];

        if (RC.length === 0) {
            alert("该流程图不符合流程定义规则");
            return;
        }

        function generatorId() {
            uniqueId++;
            return uniqueId;
        }

        for (var propertyName in NC) {
            NC[propertyName].uniqueId = generatorId();
        }

        var builder = new StringBuilder();
        builder.append("<workflow>");
        $.each(NC, function () {
            builder
                .append("<" + this.category)
                .append(" id=\"" + this.uniqueId + "\"")
                .append(" name=\"" + this.name + "\"")
                .append(">");

            builder.append(exportChildNode(builder, this.id));
            builder.append("</" + this.category + ">");
        });

        builder.append("</workflow>");

        $.each(NC, function () {
            var instance = new Node();
            $.extend(instance, this);
            delete instance['brush'];
            if (instance['circles']) {
                delete instance['circles'];
            }

            nodeCollection.push(instance);
        });

        $.each(LC, function () {
            var instance = new Line();
            $.extend(instance, this);
            delete instance['brush'];
            pathCollection.push(instance);
        });

        function exportChildNode(sb, rectId) {
            var elements = findByElementId(rectId, "from");
            $.each(elements, function () {
                if (this.from === rectId) {
                    var L = LC[this.id],
                        N = NC[L.to];
                    sb.append("<transition");
                    sb.append(" name=\"" + L.name + "\"");
                    sb.append(" to=\"" + N.uniqueId + "\"");
                    sb.append(">")
                    sb.append("</transition>");
                }
            });
        }

        var imageData=escape(JSON.stringify({
            RC: RC,
            NC: nodeCollection,
            PC: pathCollection
        }))
  
        return {
            XML: escape(builder.toString()),
            IMAGE: imageData
        };
    }

    //恢复流程图
    function revertFlow(data, disable, currentNodeId) {

        var origin = JSON.parse(unescape(data.ORIGIN)),
            nodeCollection = origin.NC,
            pathCollection = origin.PC,
            relationCollection = origin.RC;

        $.each(nodeCollection, function () {
            var self = this;

            var instance = convertToRealType(this.category);
            $.extend(instance, this);

            instance.disable = (disable || false);
            instance.draw(currentNodeId);

            $.each(["to", "from"], function () {
                eachNode(instance.id, self.id, this);
            });
        });

        $.each(pathCollection, function () {
            var instance = new Line();
            $.extend(instance, this);
            instance.disable = (disable || false);
            instance.draw();
            eachNode(instance.id, this.id, "id");
        });

        $.each(relationCollection, function () {
            RC.push(this);
        });

        function eachNode(id, originId, nid) {
            $.each(relationCollection, function () {
                var self = this;
                if (self[nid] === originId) {
                    self[nid] = id;
                }
            });
        }
    }

    //转换成真实的节点类型
    function convertToRealType(category) {
        var convertType;
        switch (category) {
            case "node":
                convertType = new Node();
                break;
            case "start":
                convertType = new Start();
                break;
            case "end":
                convertType = new End();
                break;
            case "decision":
                convertType = new Decision();
                break;
            default:
                break;
        }
        return convertType;
    }

    //获取方向
    function checkOrientation(from,to) {
        var orientation = 'down';
        if (from.y() < to.y()) {
            orientation = 'down';
        } else {
            orientation='up'
        }
        return orientation;
    }


    //帮助类
    function StringBuilder() {
        this.elements = [];
    }

    StringBuilder.prototype = {
        constructor: StringBuilder,
        append: function (text) {
            this.elements.push(text);
            return this;
        },
        toString: function () {
            return this.elements.join('');
        }
    }



 
    //对外提供访问接口
    window.SMF = {
        //绑定元素，并进行初始化
        bind: bind,
        //选择
        select: select,
        //连接
        connect: connect,
        //导出到JSON对象，以序列化保存到数据库
        exportToJSON: exportToJSON,
        //恢复图形
        revert: revertFlow,
        //创建流程节点
        create: function (category) {
            var reallType = convertToRealType(category);
            reallType.draw();
        }
    };

})(jQuery);





