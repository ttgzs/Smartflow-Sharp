Array.prototype.remove = function (dx, to) {
         if (isNaN(dx) || dx > this.length) { return false; }
         this.splice(dx, (to || 1));
     };
Function.prototype.extend = function (Parent, Override) {
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

var draw, nodeArray = {}, LineArray = {}, relationShip = [],
line = {
    x1: 0,
    y1: 0,
    x2: 0,
    y2: 0,
    from: undefined,
    to: undefined
};

SVG.ready(function () {


});

line.checkRule = function () {
    var rule = (line.from !== line.to && line.from);
    if (!rule) {
        reset();
    }
    return rule;
}

window.onload = function () {
    if (SVG.supported) {
        draw = SVG('drawing');
        draw.mouseup(function (e) {
            draw.off('mousemove');
        });

        var start = new Start(),
            end = new End();
        end.x = start.x = 30;
        end.y = start.w + 50;
        start.draw();
        end.draw();
    } else {
        alert('SVG not supported')
    }
}

//选择
function select() {
    initEvent();
    draw.each(function (i, child) {
        if (this.type === 'rect') {
            this.mousedown(rectMousedown);
        }
    });
}

//连接节点
function connect() {
    initEvent();
    $(document).bind('mousedown', function (e) {
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
    });
    $(document).bind('mouseup', function (e) {
        var nodeName = $(e.target).get(0).nodeName,
               nid = $(e.target).get(0).id;

        if (nodeName === 'rect' && line.from) {
            var rect = SVG.get(nid),
                nt = nodeArray[nid],
                nf = nodeArray[line.from];
            line.x2 = rect.width() / 2 + rect.x();
            line.y2 = rect.y();
            line.to = rect.id();
            if (line.checkRule() && !nt.checkRule(line.to, nf)) {
                var instance = new Line();
                $.extend(instance, line);

                instance.draw();

                var l = SVG.get(instance.id),
                    r = SVG.get(instance.from);

                //nt.to.push({ id: id, rectId: nid });
                //nf.from.push({ id: id, rectId: line.from });

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
    });
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

//创建节点
function createNode() {
    new Node().draw();
}

function createDecision() {
    new Decision().draw();
}

function Element(name, category) {
    this.id = undefined;
    this.text = undefined;
    this.name = name;
    this.category = category;
    this.uniqueId = undefined;
    this.attributes = [];
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

//线条
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

        self.text = draw.text(function (add) {
            add.tspan(self.name);
        });

        self.text.attr({ x: (self.x2 - self.x1) / 2 + self.x1, y: (self.y2 - self.y1) / 2 + self.y1 });
        self.id = l.id();
        LineArray[self.id] = this;
        return Line.base.Parent.prototype.draw.call(self);
    },
    bindEvent: function () {
        this.dblclick(function (evt) {
            //删除
            var instance = LineArray[this.id()];
            if (evt.ctrlKey && evt.altKey) {
                eachElements(instance.id);
                this.off('dblclick');
                this.remove();
                instance.text.remove();
                delete LineArray[instance.id];
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

//节点
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
    constructor: Node,
    draw: function () {
        var n = this;
        var rect = draw.rect(n.w, n.h)
             .attr({ fill: '#f06', x: n.x, y: n.y });

        n.id = rect.id();
        nodeArray[n.id] = n;
        n.text = draw.text(function (add) {
            add.tspan(n.name);
        });

        n.text.attr({ x: n.x + rect.width() / 2, y: n.y + rect.height() / 2 + 5 });
        return Node.base.Parent.prototype.draw.call(this);
    },
    checkRule: function (to, nf) {
        var rule = ((nf.category === 'end' || this.category === 'start') ||
                    (nf.category === 'start' && this.category === 'end'));
        return rule;
    },
    bindEvent: function (n) {
        this.mousedown(rectMousedown);
        this.dblclick(function (evt) {
            var node = nodeArray[this.id()];
            node.edit.call(this, evt);
        });
        Node.base.Parent.prototype.bindEvent.call(this);
    },
    edit: function (evt) {

        if (evt.ctrlKey && evt.altKey) {
            var id = this.id(),
                node = nodeArray[id],
                rect = SVG.get(id),
                elements = findByElementId(id);

            delElement(elements);

            rect.remove();
            node.text.remove();
            delete nodeArray[id];

        } else {
            var nx = nodeArray[this.id()], nodeName = prompt("请输入节点名称", nx.name);
            if (nodeName) {
                nx.name = nodeName;
                nx.text.clear();
                nx.text.text(function (add) {
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

        if (self.text) {
            self.text.attr({ x: (element.x() + (element.width() / 2)), y: element.y() + (element.height() / 2) + 5 });
        }

        var toElements = findByElementId(self.id, "to"),
            fromElements = findByElementId(self.id, "from");

        $.each(toElements, function () {
            var lineElement = SVG.get(this.id),
                instance = LineArray[this.id];

            if (lineElement && instance) {
                instance.x2 = self.x + self.ox2;
                instance.y2 = self.y + self.oy2;
                lineElement.attr({ x2: instance.x2, y2: instance.y2 });
                instance.text.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
            }
        });

        $.each(fromElements, function () {
            var lineElement = SVG.get(this.id),
                instance = LineArray[this.id];
            if (lineElement && instance) {
                instance.x1 = self.x + self.ox1;
                instance.y1 = self.y + self.oy1;
                lineElement.attr({ x1: instance.x1, y1: instance.y1 });
                instance.text.attr({ x: (instance.x2 - instance.x1) / 2 + instance.x1, y: (instance.y2 - instance.y1) / 2 + instance.y1 });
            }
        });
    }
});

function Start() {
    Start.base.Constructor.call(this);
    this.category = "start";
    this.name = "开始";
}

Start.extend(Node, {
    constructor: Start,
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

//决策节点
function Decision() {
    Decision.base.Constructor.call(this);
    this.name = 'decision';
    this.category = 'decision';
    this.circles = [];
}

Decision.extend(Node, {
    constructor: Decision,
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
            var decision = nodeArray[this.id()];
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


function rectMousedown(evt) {
    var nx = nodeArray[this.id()];
    nx.disX = evt.clientX - this.x() - nx.cx;
    nx.disY = evt.clientY - this.y() - nx.cy;
    dragElement.apply(this);
}

//删除与当前节点的连接线
function delElement(elements) {
    for (var i = 0; i < elements.length; i++) {
        var element = elements[i];
        if (element) {
            var l = SVG.get(element.id),
                instance = LineArray[element.id];
            l.remove();

            instance.text.remove();
            eachElements(element.id);
            delete LineArray[element.id];
        }
    }
}

//清空与线交接另一头的引用
function eachElements(id) {
    for (var i = 0, len = relationShip.length; i < len; i++) {
        var r = relationShip[i];
        if (r.id == id) {
            relationShip.remove(i);
            break;
        }
    }
}

//节点移动
function dragElement() {
    var element = this;
    var nx = nodeArray[element.id()];
    draw.on('mousemove', function (d) {
        nx.move(element, d);
    });
}

function findByElementId(elementId, propertyName) {
    var elements = [];
    $.each(relationShip, function () {
        if (propertyName) {
            if (this[propertyName] === elementId) {
                elements.push(this);
            }
        } else {
            if (this["to"] === elementId || this["from"] === elementId) {
                elements.push(this);
            }
        }
    });
    return elements;
}


//导出数据
function exportJson() {

    var UUID = 0;

    function createUID() {
        UUID++;
        return UUID;
    }

    for (var p in nodeArray) {
        nodeArray[p].uniqueId = createUID();
    }


    var nodeCollection = [], pathCollection = [];
    var builder = new StringBuilder();

    builder.Append("<workflow>");
    $.each(nodeArray, function () {
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
    return;





    $.each(nodeArray, function () {
        var instance = new Node();
        $.extend(instance, this);
        var ele = SVG.get(instance.id);
        if (ele) {
            ele.remove();
        }

        if (instance.text) {
            instance.text.remove();
        }
        delete instance['text'];
        nodeCollection.push(instance);
        delete this;
    });

    $.each(LineArray, function () {
        var instance = new Line();
        $.extend(instance, this);
        pathCollection.push(instance);
        var ele = SVG.get(instance.id);
        if (ele) {
            ele.remove();
        }
        if (instance.text) {
            instance.text.remove();
        }
        delete this;
    });

    $.each(nodeCollection, function () {
        var instance = new Node();
        $.extend(instance, this);
        var rectId = instance.draw();
        var from = instance.from || [],
             to = instance.to || [];

        eachNode(from, rectId, this.id, 'rectId');
        eachNode(to, rectId, this.id, 'rectId');

    });

    $.each(pathCollection, function () {
        var instance = new Line();
        $.extend(instance, this);
        var lid = instance.draw();

        LineArray[lid] = instance;
        changeRelationShip(lid, this.id, 'id');
    });

    function changeRelationShip(id, originId, nid) {
        $.each(nodeArray, function () {
            var from = this.from || [],
                  to = this.to || [];

            eachNode(from, id, originId, nid);
            eachNode(to, id, originId, nid);
        });
    }

    function eachNode(elements, id, originId, nid) {
        $.each(elements, function () {
            if (this[nid] === originId) {
                this[nid] = id;
            }
        });
    }
}

function exportChildNode(sb, rectId) {

    var elements = findByElementId(rectId, "from");

    $.each(elements, function () {
        if (this.from === rectId) {

            var line = LineArray[this.id],
                n = nodeArray[line.to];
            sb.append("<transition");
            sb.append(" name=\"" + line.name + "\"");
            sb.append(" to=\"" + n.uniqueId + "\"");
            sb.append(">")
            sb.append("</transition>");
        }
    });
}


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

