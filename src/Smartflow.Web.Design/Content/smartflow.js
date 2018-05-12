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
        //参数
        drawOption,
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
        },
        //标签配置
        config = {
            //流程定义根节点
            rootStart : '<workflow>',
            //流程定义根节点闭合
            rootEnd : '</workflow>',
            //开始
            start: '<',
            //结束
            end :'>',
            //左引号
            lQuotation: '"',
            //右引号
            rQuotation :'"',
            //闭合
            beforeClose :'</',
            //闭合
            afterClose : '/>',
            //等于
            equal:'=',
            //本身引用
            //空隙
            space: ' ',
            //参与组
            group: 'group',
            from :'from',
            //跳转线
            transition:'transition'
        };

    //数组添加移除方法
    $.extend(Array.prototype, {
        remove: function (dx, to) {
            this.splice(dx, (to || 1));
        }
    });

    //禁用右键菜单
    document.oncontextmenu = function () { return false; }


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

    function init(elementId, option) {
        draw = SVG(elementId);
        draw.mouseup(function (e) {
            draw.off('mousemove');
        });

        drawOption = $.extend(drawOption, option);
        unselect();
        return draw;
    }

    //取消文字选中
    function unselect() {
        $(document).bind('mousedown', function () { return false; });
        $(document).bind('mouseup', function () { return false; });
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
        //当前节点颜色
        this.bgCurrentColor = '#6DEA47';
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
            var className = this.type;
            this.addClass(className);
        }
    };

    //定义线
    function Line() {
        this.x1 = 0;
        this.y1 = 0;
        this.x2 = 0;
        this.y2 = 0;
        this.border = 3;
        this.orientation = 'down';

        /*表达式*/
        this.expression='';
        Line.base.Constructor.call(this, "line", "line");
    }

    Line.extend(Element, {
        constructor: Line,
        draw: function () {
            var self = this,
                l = draw.line(self.x1, self.y1, self.x2, self.y2);
            l.stroke({ width: self.border, color: self.bgColor });
            self.brush = draw.text(self.name);
            l.marker('end', 10, 10, function (add) {
                add.path('M0,0 L0,6 L6,3 z').fill("#f00");
                this.attr({refX:6, refY:3, orient: 'auto'});
            });

            self.brush.attr({ x: (self.x2 - self.x1) / 2 + self.x1, y: (self.y2 - self.y1) / 2 + self.y1 });
            self.id = l.id();
            LC[self.id] = this;
            return Line.base.Parent.prototype.draw.call(self);
        },
        bindEvent: function (l) {
            this.dblclick(function (evt) {
                evt.preventDefault();
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
                        instance.brush.text(instance.name);
                    }
                }
                return false;
            });
            Line.base.Parent.prototype.bindEvent.call(this, l);
        }
    });

    //定义节点
    function Node() {
        this.w = 180;
        this.h = 40;
        this.x = 10;
        this.y = 10;
        this.cx = 120;
        this.cy = 0;
        this.disX = 0;
        this.disY = 0;
        this.group = [];//参与组
        this.actors = [];//参与者
        Node.base.Constructor.call(this, "node", "node");
    }

    Node.extend(Element, {
        getTransitions: function () {
            var elements = findByElementId(this.id, config.from),
                lineCollection= [];
            $.each(elements, function () {
                lineCollection.push(LC[this.id]);
            });
            return lineCollection;
        },
        setExpression: function (expressions) {
            $.each(expressions, function () {
                LC[this.id].expression = this.expression;
            });
        },
        draw: function (b) {
            var n = this,
                color = (b == n.uniqueId && b && n.uniqueId) ? n.bgCurrentColor : n.bgColor,
                rect = draw.rect(n.w, n.h).attr({ fill: color, x: n.x, y: n.y });

            n.brush = draw.text(n.name);
            n.brush.attr({ x: n.x + rect.width() / 2, y: n.y + rect.height() / 2 });

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
                evt.preventDefault();
                var node = NC[this.id()];
                node.edit.call(this, evt);
                return false;
            });
            Node.base.Parent.prototype.bindEvent.call(this, n);
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
                var nx = NC[this.id()];
                drawOption['dblClick']
                            && drawOption['dblClick'].call(this, nx);
            }
        },
        move: function (element, d) {
            var self = this;
            self.x = d.clientX - self.disX - self.cx;
            self.y = d.clientY - self.disY - self.cy;
            element.attr({ x: self.x, y: self.y });

            if (self.brush) {
                self.brush.attr({ x: (element.x() + (element.width() / 2)), y: element.y() + (element.height() / 2) });
            }

            var toElements = findByElementId(self.id, "to"),
                fromElements = findByElementId(self.id, "from");

            $.each(toElements, function () {
                var lineElement = SVG.get(this.id),
                    instance = LC[this.id];

                if (lineElement && instance) {
                    instance.x2 = self.x + this.ox2;
                    instance.y2 = self.y + this.oy2;
                    lineElement.attr({ x2: instance.x2, y2: instance.y2 });
                    instance.brush.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
                }
            });

            $.each(fromElements, function () {
                var lineElement = SVG.get(this.id),
                    instance = LC[this.id];
                if (lineElement && instance) {
                    instance.x1 = self.x + this.ox1;
                    instance.y1 = self.y + this.oy1;
                    lineElement.attr({ x1: instance.x1, y1: instance.y1 });
                    instance.brush.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
                }
            });
        },
        exportElement: function () {
            var
                self = this,
                build = new StringBuilder();

            build.append(config.start)
                .append(self.category);

            eachAttributs(build, self);
            build.append(config.end);

            //参与组
            $.each(self.group, function () {

                build.append(config.start)
                     .append(config.group);
                eachAttributs(build, this, config.group);
                build.append(config.afterClose);
            });

            //导出Decision 其他元素
            if (self.exportDecision) {
                self.exportDecision(build);
            }
            
            var elements = findByElementId(self.id, config.from);
            $.each(elements, function () {
                if (this.from === self.id) {
                    var
                        L = LC[this.id],
                        N = NC[this.to];

                    build.append(config.start)
                         .append(config.transition)
                         .append(config.space)
                         .append('name')
                         .append(config.equal)
                         .append(config.lQuotation)
                         .append(L.name)
                         .append(config.rQuotation)
                         .append(config.space)
                         .append('to')
                         .append(config.equal)
                         .append(config.lQuotation)
                         .append(N.uniqueId)
                         .append(config.rQuotation)
                         .append(config.space)
                         .append('expression')
                         .append(config.equal)
                         .append(config.lQuotation)
                         .append(L.expression)
                         .append(config.rQuotation)
                         .append(config.afterClose);
                }
            });

            //结束
            build.append(config.beforeClose)
                 .append(self.category)
                 .append(config.end);

            //属性
            function eachAttributs(build, reference, attribute) {
                var propertyName = 'uniqueId'
                $.each(['id', 'name'], function (i, p) {
                    build.append(config.space)
                         .append(p)
                         .append(config.equal)
                         .append(config.lQuotation)
                         .append(p === 'id' && attribute !== 'group' ? reference[propertyName] : reference[p])
                         .append(config.rQuotation);
                });
            }

            //导出
            return build.toString();
        }
    });


    //决策节点
    function Decision() {
        Decision.base.Constructor.call(this);
        this.name = 'decision';
        this.category = 'decision';
        this.circles = [];
        //命令
        this.command = undefined;
    }

    Decision.extend(Node, {
        draw: function () {
            Decision.base.Parent.prototype.draw.call(this);
            var y = this.y + this.h,
                w = this.w;

            //09dd1a
            for (var i = 0, len = w / 20; i < len; i++) {
                var circle = draw.circle(20);
                circle.attr({ fill: '#F485B2', cx: this.x + i * 20 + 10, cy: y });
                circle.addClass('circle');

                var rect = draw.rect(20, 20).attr({
                    x: this.x + i * 20,
                    y: y - 20
                });
                var clip = draw.clip().add(rect);
                circle.clipWith(clip);

                circle.attr({ decisionId: this.id });
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
            var self = this, y = self.y + self.h;
            //09dd1a
            $.each(self.circles, function (i) {
                var clipRect = this.reference('clip-path');
                var rect = SVG.get(clipRect.node.firstChild.id);
                rect.attr({ x: self.x + i * 20, y: y - 20 });
                this.attr({ fill: '#F485B2', cx: self.x + i * 20 + 10, cy: y });
            });
        },
        bindEvent: function (decision) {
            Decision.base.Parent.prototype.bindEvent.call(this, decision);
        },
        exportDecision: function (build) {
            var self = this;
            if (self.commad) {

                build.append(config.start)
                     .append('command')
                     .append(config.end);

                $.each(self.command, function (propertyName, value) {
                   
                    build.append(config.start)
                         .append(propertyName)
                         .append(config.end)
                         .append(value)
                         .append(config.beforeClose)
                         .append(propertyName)
                         .append(config.end);
                });

                build.append(config.beforeClose)
                     .append('command')
                     .append(config.end);
            }
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
        constructor: End,
        draw: function () {
            End.base.Parent.prototype.draw.call(this);
            var nid = this.id,
                rect = SVG.get(nid);

            rect.radius(10);
        },
        bindEvent: function (n) {
            End.base.Parent.prototype.bindEvent.call(this, n);
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

        if (nodeName === 'rect' || nodeName === 'circle') {
            var instance, y, x;
            if (!NC[nodeId]) {
                var decisionId = node.getAttribute("decisionId");
                instance = NC[decisionId];
                y = node.instance.cy();
                x = node.instance.cx();
            } else {
                instance = NC[nodeId];
                y = evt.clientY - instance.cy;
                x = evt.clientX - instance.cx;
            }

            fromConnect = {
                id: instance.id,
                x: x,
                y: y
            }
        }

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

                if (orientation === 'down' && nf.category === 'decision') {
                    instance.x1 = fromConnect.x;
                    instance.y1 = fromConnect.y;
                    instance.x2 = toRect.width() / 2 + toRect.x();
                    instance.y2 = toRect.y();
                }
                else if (orientation === 'down') {
                    instance.x1 = fromRect.width() / 2 + fromRect.x();
                    instance.y1 = fromRect.height() + fromRect.y();
                    instance.x2 = toRect.width() / 2 + toRect.x();
                    instance.y2 = toRect.y();

                } else {
                    instance.x1 = fromConnect.x;
                    instance.y1 = fromRect.y();
                    instance.x2 = evt.clientX - nt.cx;
                    instance.y2 = toRect.height() + toRect.y();
                }

                instance.orientation = orientation;
                instance.draw();

                var l = SVG.get(instance.id),
                    r = SVG.get(fromConnect.id);

                RC.push({
                    id: instance.id,
                    from: fromConnect.id,
                    to: nodeId,
                    ox2: l.attr("x2") - toRect.x(),
                    oy2: l.attr("y2") - toRect.y(),
                    ox1: l.attr("x1") - fromRect.x(),
                    oy1: l.attr("y1") - fromRect.y()
                });
            }
        }
        fromConnect = undefined;
        //evt.preventDefault();
        return false;
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
        evt.preventDefault();
        nx.disX = evt.clientX - self.x() - nx.cx;
        nx.disY = evt.clientY - self.y() - nx.cy;
        draw.on('mousemove', function (d) {
            d.preventDefault();
            nx.move(self, d);
            return false;
        });
        return false;
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
        var uniqueId = 29,
            nodeCollection = [],
            pathCollection = [],
            build = new StringBuilder();

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

        build.append(config.rootStart);
        $.each(NC, function () {
            var self = this;
            build.append(self.exportElement());
        });

        build.append(config.rootEnd);

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


        var imageData = escape(JSON.stringify({
            RC: RC,
            NC: nodeCollection,
            PC: pathCollection
        }))

        return {
            XML: escape(build.toString()),
            IMAGE: imageData
        };
    }

    //恢复流程图
    function revertFlow(data, disable, currentNodeId) {

        var imageData = JSON.parse(unescape(data)),
            nodeCollection = imageData.NC,
            pathCollection = imageData.PC,
            relationCollection = imageData.RC;

        $.each(nodeCollection, function () {
            var self = this,
                originId = self.id;

            var instance = convertToRealType(this.category);
            $.extend(instance, this);

            instance.disable = (disable || false);
            instance.draw(currentNodeId);

            $.each(["to", "from"], function (i, propertyName) {
                eachNode(instance.id, originId, propertyName);
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
    function checkOrientation(from, to) {
        var orientation = 'down';
        if (from.y() < to.y()) {
            orientation = 'down';
        } else {
            orientation = 'up'
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
        init: init,
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

            reallType.x = Math.floor(Math.random() * 200 + 1);
            reallType.y = Math.floor(Math.random() * 200 + 1);

            reallType.draw();
        }
    };

})(jQuery);





