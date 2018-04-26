(function ($) {

    var SMF = {},
        //存储所有节点的实例
        NC = {},
        //所有线的实例
        LC = {},
        //维护关系
        RC = []

    var draw,
        line = {
        x1: 0,
        y1: 0,
        x2: 0,
        y2: 0,
        from: undefined,
        to: undefined
    };

    //数组添加移除方法
    $.extend(Array.prototype, {
        remove: function (dx, to) {
            this.splice(dx, (to || 1));
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


    $.fn.SmartFlow = function () {
        var self = this,
            elementId = self.attr("id");

        draw = SVG(elementId);
        draw.mouseup(function (e) {
            draw.off('mousemove');
        });

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
    }

    Element.prototype = {
        constructor: Element,
        draw: function () {
            this.bindEvent.apply(SVG.get(this.id), [this]);
            return this.id;
        },
        bindEvent: function () {
            this.mouseout(function () {
                if (this.type === 'rect') {
                    this.attr({ fill: '#f06' });
                } else {
                    this.stroke({ color: '#f06' })
                }
            });

            this.mouseover(function (e) {
                if (this.type === 'rect') {
                    this.attr({ fill: '#f8e233' });
                } else {
                    this.stroke({ color: '#f8e233' })
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
        this.color = '#f06';
        this.border = 3;
        this.from = undefined;
        this.to = undefined;
        Line.base.Constructor.call(this, "line", "line");
    }

    Line.extend(Element, {
        constructor: Line,
        draw: function () {
            var self = this;
            var l = draw.line(self.x1, self.y1, self.x2, self.y2)
                   .stroke({ width: self.border, color: self.color });

            self.brush = draw.text(function (add) {
                add.tspan(self.name);
            });

            self.brush.attr({ x: (self.x2 - self.x1) / 2 + self.x1, y: (self.y2 - self.y1) / 2 + self.y1 });
            self.id = l.id();
            LC[self.id] = this;
            return Line.base.Parent.prototype.draw.call(self);
        },
        bindEvent: function () {
            this.dblclick(function (evt) {
                //删除
                var instance = LC[this.id()];
                if (evt.ctrlKey && evt.altKey) {
                    eachElements(instance.id);
                    this.off('dblclick');
                    this.remove();
                    instance.text.remove();
                    delete LC[instance.id];
                } else {
                    var nodeName = prompt("请输入节点名称", instance.name);
                    if (nodeName) {
                        instance.name = nodeName;
                        instance.text.clear();
                        instance.text.text(function (add) {
                            add.tspan(lA.name);
                        });
                    }
                }
            });
            return Line.base.Parent.prototype.bindEvent.call(this);
        }
    });

    //定义节点
    function Node() {
        this.w = 180;
        this.h = 50;
        this.x = 10;
        this.y = 10;
        this.cx = 80;
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
        draw: function () {
            var n = this;
            var rect = draw.rect(n.w, n.h)
                 .attr({ fill: '#f06', x: n.x, y: n.y });
            n.id = rect.id();
            n.brush = draw.text(function (add) {
                add.tspan(n.name);
            });
            n.brush.attr({ x: n.x + rect.width() / 2, y: n.y + rect.height() / 2 + 5 });
            NC[n.id] = n;
            return Node.base.Parent.prototype.draw.call(this);
        },
        checkRule: function (to, nf) {
            var rule = ((nf.category === 'end' || this.category === 'start') ||
                        (nf.category === 'start' && this.category === 'end'));
            return rule;
        },
        bindEvent: function (n) {
            this.mousedown(OnStartDrag);
            this.dblclick(function (evt) {
                var node = NC[this.id()];
                node.edit.call(this, evt);
            });
            Node.base.Parent.prototype.bindEvent.call(this);
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
                    instance.text.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
                }
            });

            $.each(fromElements, function () {
                var lineElement = SVG.get(this.id),
                    instance = LC[this.id];
                if (lineElement && instance) {
                    instance.x1 = self.x + self.ox1;
                    instance.y1 = self.y + self.oy1;
                    lineElement.attr({ x1: instance.x1, y1: instance.y1 });
                    instance.text.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
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
            Decision.base.Parent.prototype.bindEvent.call(this);
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
        bindEvent: function () {
            Start.base.Parent.prototype.bindEvent.call(this);
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
        bindEvent: function () {
            End.base.Parent.prototype.bindEvent.call(this);
            this.off('dblclick');
        }
    });

    //选择
    function select() {
        initEvent();
        draw.each(function (i, child) {
            if (this.type === 'rect') {
                this.mousedown(OnStartDrag);
            }
        });
    }

    //连接节点
    function connect() {
        initEvent();
        $(document).bind('mousedown', OnStartLine);
        $(document).bind('mouseup', OnEndLine);
    }

    //开始连线
    function OnStartLine(evt) {
        var nodeName = $(e.target).get(0).nodeName,
            nid = $(e.target).get(0).id;
        if (nodeName === 'rect') {
            var rect = SVG.get(nid);
            line.x1 = rect.width() / 2 + rect.x();
            line.y1 = rect.height() + rect.y();
            line.from = rect.id();
        }
        e.preventDefault();//阻止默认事件，取消文字选中
        return false;
    }

    //结束连线
    function OnEndLine(evt) {
        var nodeName = $(e.target).get(0).nodeName,
               nid = $(e.target).get(0).id;

        if (nodeName === 'rect' && line.from) {
            var rect = SVG.get(nid),
                nt = NC[nid],
                nf = NC[line.from];
            line.x2 = rect.width() / 2 + rect.x();
            line.y2 = rect.y();
            line.to = rect.id();
            if (line.checkRule() && !nt.checkRule(line.to, nf)) {
                var instance = new Line();
                $.extend(instance, line);

                instance.draw();

                var l = SVG.get(instance.id),
                    r = SVG.get(instance.from);

                relationShip.push({ id: instance.id, from: line.from, to: nid });

                nt.ox2 = l.attr("x2") - rect.x();
                nt.oy2 = l.attr("y2") - rect.y();
                nf.ox1 = l.attr("x1") - r.x();
                nf.oy1 = l.attr("y1") - r.y();
            } else {
                reset();
            }
        }
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

    function OnStartDrag(evt) {
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

    //清空连线的值
    function reset() {
        $.extend(line, {
            x1: 0,
            y1: 0,
            x2: 0,
            y2: 0,
            from: undefined,
            to: undefined
        });
    }

    line.checkRule = function () {
        var rule = (line.from !== line.to && line.from);
        if (!rule) {
            reset();
        }
        return rule;
    }

    //导出数据
    function exportToJSON() {
        var UUID = 0,
            nodeCollection = [],
            pathCollection = [];

        function createUID() {
            UUID++;
            return UUID;
        }

        for (var p in NC) {
            NC[p].uniqueId = createUID();
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
        alert(builder.toString());

        $.each(NC, function () {
            var instance = new Node();
            $.extend(instance, this);
            delete instance['text'];
            nodeCollection.push(instance);
        });


        $.each(LC, function () {
            var instance = new Line();
            $.extend(instance, this);
            delete instance['text'];
            pathCollection.push(instance);
        });

        
        return {
            RC: relationShip,
            NC: nodeCollection,
            PC: pathCollection
        }

      
    }

    function exportChildNode(sb, rectId) {
        var elements = findByElementId(rectId, "from");
        $.each(elements, function () {
            if (this.from === rectId) {
                var line = LC[this.id],
                    n = NC[line.to];
                sb.append("<transition");
                sb.append(" name=\"" + line.name + "\"");
                sb.append(" to=\"" + n.uniqueId + "\"");
                sb.append(">")
                sb.append("</transition>");
            }
        });
    }

    //恢复流程图
    function revertFlow(data) {

        var origin = JSON.parse(unescape(data.ORIGIN)),
            nodeCollection = origin.NC,
            pathCollection = origin.PC,
            relationCollection = origin.RC;

        $.each(nodeCollection, function () {
            var self = this;
            var instance = new Node();
            $.extend(instance, this);
            instance.draw();

            $.each(["to", "from"], function () {
                eachNode(instance.id, self.id, this);
            });
        });

        $.each(pathCollection, function () {
            var instance = new Line();
            $.extend(instance, this);
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
    window.SMF = SMF = {
        //选择
        select: select,
        //连接
        connect: connect,
        //导出到JSON对象，以序列化保存到数据库
        exportToJSON: exportToJSON,
        //恢复图形
        revert: revertFlow,

        create: function (category) {
            switch (category) {
                case "node":
                    new Node().draw();
                    break;
                case "line":
                    new Line().draw();
                    break;
                default:
                    break;
            }
        }
    };


})(jQuery);





