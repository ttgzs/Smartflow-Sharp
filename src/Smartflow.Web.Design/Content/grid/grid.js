/*! nice Grid 0.2.0
 * (c) 2015 Jony Zhang <zj86@live.cn>, MIT Licensed
 * https://github.com/niceue/nice-grid
 */
(function($, undefined) {
    "use strict";

    var NS = "grid",
        PREFIX = 'n-',
        CLS_NS = '.'+ NS,
        CLS_PREFIX = '.'+ PREFIX,
        DISABLED = 'disabled',
        SELECTED = 'selected',

        rTemplate = /{([^\{\}]*)}/,
        rSize = /^\d+(?:px)?$/,

        isIE = document.documentMode || +(navigator.userAgent.match(/MSIE (\d+)/) && RegExp.$1),
        isString = function(s) {
            return typeof s === 'string';
        },
        isObject = function(o) {
            return o && Object.prototype.toString.call(o) === '[object Object]';
        },
        isFunction = $.isFunction,
        isArray = $.isArray,
        proxy = $.proxy,
        trim = $.trim,

        defaults = {
            theme: "default",       //主题（暂未实现）
            local: 'zh-CN',         //语言环境（暂未实现）
            form: null,             //绑定表单后，自动提取表单参数
            context: null,          
            autoLoad: true,         //如果开启，并且设置了url参数，则自动请求数据并渲染

            dataSource: {
                url: '',
                type: 'post',
                dataType: 'json',
                cache: false
            },               // String | Object

            keyMap: {
                rows: "rows",
                pageSize: "pageSize",
                pageIndex: "pageIndex",
                sortName: "sortName",
                sortOrder: "sortOrder",
                records: "records"
            },
            caption: null,          //标题（暂未实现）
            columns: [],            //列配置：field、title、align、style、className、width、minScreenWidth、locked、
                                    //formatter、editor、editable、sortable、resizable、hidden、values
            summary: null,          //汇总（暂未实现）
            formatters: null,       //自定义格式化器
            editors: null,          //自定义编辑器（暂未实现）

            width: "100%",          //"auto"、number、"fit"
            height: "auto",         //"auto"、number、"fit"
            keepHeight: true,       //保持最后一页高度
     
            pageable: {
                pageIndex: 1,
                pageSize: 10,
                pageSizeOptions: [10,20,30,50],
                info: true,
                messages: {
                    first: '首页',
                    last: '尾页',
                    prev: '上一页',
                    next: '下一页'
                }
            },
            selectable: false,      // multiple,row,cell  // .select(row) 、.select(cell) 、.clearSelection()
            scrollable: true,       //（暂未实现）
            resizable: true,        //（暂未实现）是否可拖拽列宽
            editable: false,        //（暂未实现）inline popup
            sortable: false,        //（暂未实现）是否可排序
            filterable: false,      //（暂未实现）
            groupable: false,       //（暂未实现）
            navigatable: false,     //（暂未实现）

            cellRender: function(value, row, rowIndex) {
                return value;
            },

            onDblclick: null,       //双击单元格
            onGetData: null,        //每次请求回数据
            onComplete: null        //每次渲染完成
        }


    /**
     * 
     * @class Grid
     * @constructor
     */
    function Grid(container, options) {
        var me = this, opts;

        if (!(me instanceof Grid)) {
            return new Grid(container, options);
        }

        me.$el = $(container);
        opts = me.config(options);

        if (opts.columns) {
            me._init();
        }
        if (opts.autoLoad) {
            me.query();
        }
    }

    Grid.prototype = {
        config: function(options) {
            var me = this,
                MARK = 'bind-grid';

            if (options.form) {
                me.$form = $(options.form);
                if ( me.$form.length ) {
                    if ( !me.$form.data(MARK) ) {
                        me.$form.on('submit'+CLS_NS, $.proxy(me, '_submit')).data( MARK, true );
                    }
                } else {
                    me.$form = null;
                }
            }
            if ( isString(options.dataSource) ) {
                options.dataSource = {url: options.dataSource};
            }

            return me.options = $.extend(true, {}, defaults, options);
        },

        _init: function() {
            var me = this,
                opts = me.options,
                pageable = opts.pageable;

            opts.context = opts.context || opts;
            if (opts.width === '100%') {
                me.fitWidth = true;
            }
            me.formatters = new Formatters(opts.formatters, true);
            me.editors = new Editors(opts.editors, true);
            me.pageIndex = pageable ? pageable.pageIndex : 1;
            me._initColumns(opts.columns);
            me.$thead = $( me._createThead() );
            me.$theadWrap = me.$thead.children();
            me.$tbody = $( me._createTbody() );
            me.$tbodyWrap = me.$tbody.children();
            me.$tfoot = $( me._createTfoot() );

            me.tbody = me.$tbody.find('tbody')[0];
            me._className = me.$el[0].className;

            me.$el.data(NS, me)
                .addClass( _className(NS, me.fitWidth && 'fitwidth') )
                .append(me.$thead, me.$tbody, me.$tfoot)
                // bind events
                .on('click'+CLS_NS, CLS_PREFIX+'pager-nav', proxy(me, '_clickPager'));

            me.$tbody
                .on('scroll'+CLS_NS, proxy(me, '_scrollData'))
                .on('dblclick'+CLS_NS, 'td', proxy(me, '_dblclickTD'))
                .on('click'+CLS_NS, 'td', proxy(me, '_clickTD'))
                .on('click'+CLS_NS, 'a[data-fn]', proxy(me, '_clickFn'));

            if (opts.selectable) {
                me.$tbody.find('table').addClass(PREFIX+'selectable');
            }

            me._initSize();
        },

        _initColumns: function(columns) {
            var me = this,
                opts = me.options,
                headRows = [],
                dataCols = [],
                callSelf = function (cols, key, next) {
                    var i = 0, len = cols.length, col;
                    key = key || 0;
                    next && key++;
                    headRows[key] = headRows[key] || [];
                    for (; i < len; ) {
                        col = cols[i++];
                        if (isString(col.formatter)) {
                            col.render = me.formatters[col.formatter];
                        }
                        headRows[key].push(col);
                        //要计算colspan但不用计算rowspan
                        if (col.columns) {
                            col.rowspan = 1;
                            col.colspan = getColspan(col.columns);
                            callSelf(col.columns, key, true);
                        }
                    }
                };
            callSelf(columns);
            
            me.headRowCount = headRows.length;
            me.headRows = headRows;
            me.dataCols = me.headRowCount > 1 ? _mapTree(columns, dataCols, 'columns') : columns;
        },

        _initSize: function() {
            var me = this,
                opts = me.options,
                isHScroll = me.isHScroll = _isHScroll(me.$theadWrap[0]);

            if (rSize.test(opts.width)) {
                me.width = parseInt(opts.width);
                me.$el.width(opts.width);
                me.$tbody.width(me.$thead.width());
            }
            if (rSize.test(opts.height)) {
                me.height = parseInt(opts.height);
                me.$el.height(opts.height);
                me.$tbody.height( opts.height - me.$thead.outerHeight() - me.$tfoot.outerHeight() );
            }
            me.$el[ isHScroll ? 'addClass' : 'removeClass' ](PREFIX + 'hscroll');
        },

        _createThead: function() {
            var me = this, html = '',
                headRows = me.headRows,
                i = 0, rowCount = headRows.length, j, colCount,
                row, col, rowspan, colspan;

            html += '<div class="'+ PREFIX +'thead"><div class="'+ PREFIX + 'thead-wrap">';
            html += '<table role="grid" cellpadding="0" cellspacing="0">';
            html += me._createColgroup();
            html += '<thead role="rowgroup">';

            for (; i < rowCount; i++) {
                row = headRows[i];
                html += '<tr role="row">';
                for (j = 0, colCount = row.length; j < colCount; j++) {
                    col = row[j];
                    colspan = col.colspan;
                    colspan = (colspan && colspan > 1) ? ' colspan=' + colspan : '';
                    rowspan = col.rowspan || rowCount;
                    rowspan = (rowspan > 1) ? ' rowspan=' + rowspan : '';
                    html += '<th role="columnheader" data-index=' + j + (col.field ? ' data-field="'+ col.field : '') +'"';
                    html += ' class="'+ _className('header', col.align && 'align-'+col.align , colspan && 'colspan', col.sortable && 'sortable', colCount-1===j && 'last') + '"';
                    if (col.hidden) html += ' style="display:none"';
                    html += colspan + rowspan + '>' + (col.title || '') + '</th>';
                }
                html += '</tr>';
            }

            html += '</thead></table>';
            html += '</div></div>';
            return html;
        },

        _createTbody: function() {
            var me = this, html = '';

            html += '<div class="'+ _className('tbody', 'scrollable') +'"><div class="'+ PREFIX + 'tbody-wrap">';
            html += '<table role="grid" cellpadding="0" cellspacing="0">';
            html += me._createColgroup();
            html += '<tbody role="rowgroup"></tbody></table>';
            html += '</div></div>';
            return html;
        },

        _createTfoot: function() {
            var me = this,
                opts = me.options,
                pageable = opts.pageable, messages,
                PAGER_NAV = 'pager-nav',
                html = '';

            if (pageable) {
                messages = pageable.messages;
                html += '<div class="'+ _className('tfoot', 'pager') +'">';
                html += '<a href="javascript:" class="'+ _className(PAGER_NAV, 'pager-first', DISABLED) +'" rel="first" tabindex="-1">'+ messages.first +'</a>';
                html += '<a href="javascript:" class="'+ _className(PAGER_NAV, 'pager-prev', DISABLED) +'" rel="prev" tabindex="-1">'+ messages.prev +'</a>';
                html += '<span class="'+ _className('pager-numbers') +'">';
                html += '<a href="javascript:" class="'+ _className(DISABLED) +'" rel="index" tabindex="-1">1</a>';
                html += '</span>';
                html += '<a href="javascript:" class="'+ _className(PAGER_NAV, 'pager-next', DISABLED) +'" rel="next" tabindex="-1">'+ messages.next +'</a>';
                html += '<a href="javascript:" class="'+ _className(PAGER_NAV, 'pager-last', DISABLED) +'" rel="last" tabindex="-1">'+ messages.last +'</a>';
                html += '<span class="'+ _className('pager-sizes') +'"></span>';
                html += '<span class="'+ _className('pager-info') +'"></span>';
                html += '</div>';
            }
            return html;
        },

        _createColgroup: function() {
            var me = this, html = '',
                dataCols = me.dataCols,
                i = 0, len = dataCols.length;

            html += '<colgroup>';
            for (; i < len; i++) {
                html += '<col'+ ( dataCols[i].width ? ' style="width:'+ dataCols[i].width +'px"' : '' ) +'>';
            }
            html += '</colgroup>';
            return html;
        },

        _createRows: function(rows, start) {
            var me = this, opts = me.options,
                cols = me.dataCols, style, className, value,
                i, j, row, col, rowCount = rows.length, colCount = cols.length,
                html = '', STR = '';

            if (!rowCount) {
                html += '<tr class="'+ _className('last', 'no-data') +'"><td class="'+ _className('last') +'" align="center" colspan="'+ colCount +'">暂无数据</td></tr>'
            }
            for (i = 0; i < rowCount; i++) {
                row = rows[i];
                html += '<tr role="row" data-index="'+ (start + i) +'" class="'+ _className((i%2 ? 'even' : 'odd'), rowCount-1===i && 'last') +'">';

                for (j = 0; j < colCount; j++ ) {
                    col = cols[j];
                    style = col.hidden ? 'display:none;' : STR;
                    if (col.style) style += col.style;
                    if (style) style = ' style="' + style + '"';
                    // className
                    className = _className(col.align && 'align-'+col.align, colCount-1===j && 'last');
                    if (col.className) {
                        className += ' ' + col.className;
                    }
                    if (className) className = ' class="' + trim(className) + '"';
                    // value
                    if (col.field) {
                        value = row[col.field];
                        if (value !== undefined) {
                            if (col.values) value = col.values[value] || value;
                        } else {
                            value = STR;
                        }
                    } else {
                        value = STR;
                    }
                    html += '<td role="gridcell"' + className + style + '>';
                    html += ((col.render || opts.cellRender).call(col, value, row, i ) || '&nbsp;') + '</td>';
                }

                html += '</tr>';
            }

            return html;
        },

        _updatePager: function() {
            var me = this,
                opts = me.options,
                pageable = opts.pageable,
                pageSatrt, pageEnd, pageTotle, pageIndex,
                pagerNumbers = '', pagerInfo = '',
                i, len, $pagerNav;

            if (!pageable) return;

            pageTotle = me.pageTotle = Math.ceil(me.records / pageable.pageSize);
            pageIndex = me.pageIndex;
            pageSatrt = Math.ceil(pageIndex/5) * 5 - 4;
            pageEnd = Math.min(pageSatrt+4, pageTotle || pageSatrt);

            if (pageSatrt > 1) {
                pagerNumbers += '<a href="javascript:" class="'+ _className('pager-nav', 'pager-more') +'" data-page="'+ (pageSatrt - 1) +'" rel="index" tabindex="-1">...</a>';
            }
            for (i=pageSatrt; i<=pageEnd; i++) {
                pagerNumbers += '<a href="javascript:" class="'+ _className(i===pageIndex ? 'pager-current' : 'pager-nav', !pageTotle&&DISABLED) +'" data-page="'+ i +'" rel="index" tabindex="-1">'+ i +'</a>';
            }
            if (pageEnd < pageTotle) {
                pagerNumbers += '<a href="javascript:" class="'+ _className('pager-nav', 'pager-more') +'" data-page="'+ (pageEnd + 1) +'" rel="index" tabindex="-1">...</a>';
            }
            // update page numbers
            me.$tfoot.find('span'+CLS_PREFIX+'pager-numbers').html(pagerNumbers);

            // update page infomation
            if (pageable.info) {
                if (pageTotle) {
                    pagerInfo = ((pageIndex-1) * pageable.pageSize + 1) + ' - '+ Math.min(pageIndex * pageable.pageSize, me.records) +' 条，总 '+ me.records +' 条';
                }
                me.$tfoot.find('span'+CLS_PREFIX+'pager-info').html(pagerInfo);
            }
            
            // update page navigation state
            $pagerNav = me.$tfoot.children(CLS_PREFIX+'pager-nav').removeClass(PREFIX+DISABLED);
            if (pageIndex === 1) {
                $pagerNav.slice(0, 2).addClass(PREFIX+DISABLED);
            }
            if (!pageTotle || pageIndex === pageTotle) {
                $pagerNav.slice(-2).addClass(PREFIX+DISABLED);
            }
        },

        _clickPager: function(e) {
            var me = this, $a = $(e.currentTarget), index;

            e.preventDefault();

            if ($a.is(CLS_PREFIX+DISABLED)) {
                return;
            }
            switch ($a.attr('rel')) {
                case 'prev':
                    index = me.pageIndex - 1;
                    break;
                case 'next':
                    index = me.pageIndex + 1;
                    break;
                case 'first':
                    index = 1;
                    break;
                case 'last':
                    index = me.pageTotle;
                    break;
                case 'index':
                    index = +$a.attr('data-page');
                    break;
            }
            me.pageIndex = index;
            me.query();
        },

        /**
         * Query data
         * @param  {Object|String} params
         */
        query: function(params) {
            var me = this,
                opts = me.options,
                pageable = opts.pageable,
                arr = [],
                data = {},
                settings = {};

            if (me.$xhr || !opts.dataSource) return;

            // from cache
            if (!params && pageable.local && me.rows) {
                me.render();
            }
            // Local data
            else if (isArray(opts.dataSource)) {
                if (!me.rows) {
                    me.rows = opts.dataSource;
                    me.records = me.rows.length;
                    pageable.local = true;
                }
                me.render();
            }
            // Request data
            else {
                if (pageable) {
                    data[me._k('pageIndex')] = me.pageIndex;
                    data[me._k('pageSize')] = pageable.pageSize;
                }
                if (isString(opts.dataSource)) {
                    settings.url = opts.dataSource;
                } else {
                    $.extend(settings, opts.dataSource);
                    if (isObject(settings.data)) {
                        $.extend(data, settings.data);
                    }
                }
                if (isObject(params)) {
                    $.extend(data, params);
                } else if (isString(params)) {
                    arr.push(params)
                }
                arr.push( isString(settings.data) ? settings.data : $.param(data));
                if (me.$form) arr.push(me.$form.serialize());

                settings.data = arr.join('&');

                me.$xhr = $.ajax(settings)
                    .done(function(d){
                        var data = isFunction(opts.onGetData) && opts.onGetData.call(me, d) || (!d[me._k('rows')] && d.data ? d.data : d);
                        me.rows = data[me._k('rows')] || [];
                        me.records = data[me._k('records')] || me.rows.length;
                        me.render();
                    })
                    .always(function(){
                        delete me.$xhr;
                    });
            }
        },

        render: function() {
            var me = this,
                opts = me.options,
                pageable = opts.pageable,
                start = 0,
                rows = me.rows;

            // local paging
            if (pageable) {
                start = (me.pageIndex - 1) * pageable.pageSize;
                if (pageable.local) {
                    rows = me.rows.slice(start, start + pageable.pageSize);
                }
                me._updatePager();
            }
            me.$tbody.find('tbody').html( me._createRows(rows, start) );

            if (opts.height === 'auto') {
                if (opts.keepHeight) {
                    me.rowHeight = me.rowHeight || me.$tbody.find('tr').eq(0).outerHeight();
                    me.$tbodyWrap.css('padding-bottom', me.rowHeight * (pageable.pageSize - (rows.length||1))); //至少会有一行的高度
                }
            } else {
                //可能出现滚动条，要修正一下宽度
                setTimeout(function(){
                    me.$el[ _isVScroll(me.$tbody[0]) ? 'addClass' : 'removeClass' ](PREFIX + 'vscroll');
                }, 100);
            }

            if (isFunction(opts.onComplete)) {
                opts.onComplete.call(me);
            }
        },

        _submit: function(e) {
            e.preventDefault();
            this.pageIndex = 1;
            this.query();
        },

        _scrollData: function(e) {
            this.$theadWrap[0].scrollLeft = e.target.scrollLeft;
        },

        _clickTD: function(e) {
            var me = this,
                opts = me.options,
                selectable = opts.selectable,
                isRow, index, lastIndex, CLS = PREFIX + SELECTED,
                $tbody = me.$tbody, tr, dblclick,
                lastClick = $tbody.lastClick,
                el = e.currentTarget, $el;

            if (!selectable) return;

            $el = $(el);
            if (~selectable.indexOf('row')) {
                isRow = true;
                $el = $el.parent();
            }

            if (lastClick) {
                // dblclcik
                if (e.timeStamp - lastClick < 400) {
                    dblclick = true;
                    
                }
            }
            $tbody.lastClick = e.timeStamp;
            

            if (~selectable.indexOf('multiple')) {
                if (e.ctrlKey) {
                    $el.toggleClass(CLS);
                }
                else if (e.shiftKey) {

                }
                else {
                    $el.addClass(CLS).siblings().removeClass(CLS);
                }
            } else {
                if (dblclick && $el.hasClass(CLS)) return;
                $el.toggleClass(CLS).siblings().removeClass(CLS);
            }
            
            
        },

        _dblclickTD: function(e) {
            var me = this, opts = me.options;
            if (isFunction(opts.onDblclick)) {
                e.data = me.rows[e.currentTarget.parentNode.getAttribute('data-index')];
                opts.onDblclick.call(me, e);
            }
        },

        _clickFn: function(e) {
            e.preventDefault();
            var me = this,
                opts = me.options,
                $a = $(e.currentTarget),
                fn = $a.attr('data-fn');

            if (!opts.context[fn]) return;
            if (opts.selectable) e.stopPropagation();
            opts.context[fn].call( opts.context, e, me.rows[ $a.closest('tr').attr('data-index') ] );
        },

        _k: function(key) {
            return this.options.keyMap[key];
        },

        destroy: function() {
            var me = this;

            me.$tbody.off().remove();
            me.$el.off().removeData('grid').html('').removeClass().addClass(me._className);
        },

        refresh: function() {
            this.query();
            return this;
        },

        select: function() {

        },

        /**
         * 获取选中行的数据
         * @return {Array} rows
         */
        getSelectedRows: function() {
            var me = this,
                rows = [];
            $(me.tbody.rows).filter(CLS_PREFIX+SELECTED).each(function(i, tr){
                rows.push(me.rows[tr.getAttribute('data-index')]);
            });
            return rows;
        }
    };

    function Formatters(obj, context) {
        if (!isObject(obj)) return;

        var k, that = context ? context === true ? this : context : Formatters.prototype;

        for (k in obj) {
            that[k] = obj[k];
        }
    }

    function Editors(obj, context) {
        if (!isObject(obj)) return;

        var k, that = context ? context === true ? this : context : Editors.prototype;

        for (k in obj) {
            that[k] = obj[k];
        }
    }


    /**
     * Expand object arrays
     * @param srcArray  Source Array
     * @param mapArray  Map Array
     * @param key       
     */
    function _mapTree(srcArray, mapArray, key) {
        var i = 0, len = srcArray.length, col;
        for (; i < len; ) {
            col = srcArray[i++];
            col[key] ? _mapTree(col[key], mapArray, key) : mapArray.push(col);
        }
        return mapArray;
    }

    /**
     * Calculation colspan
     * @param cols       
     */
    function _getColspan(cols) {
        var arr = [];
        _mapTree(cols, arr, 'columns');
        return arr.length;
    }

    function _className() {
        var args = arguments, i = args.length, arr = [];
        while (i--) {
            if (args[i]) arr.push(PREFIX + args[i]);
        }
        return arr.reverse().join(' ');
    }

    function _isHScroll(el) {
        return el.scrollWidth - el.clientWidth !== 0;
    }

    function _isVScroll(el) {
        return el.scrollHeight - el.clientHeight !== 0;
    }

    new Formatters({
        /** currency
         *  @usage:
            formatter: "currency"
         */
        currency: function(value) {
            var parts = Number(value).toFixed(2).toString().split('.');
            return parts[0].replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1,') + '.' + parts[1];
        }
    });
    
    /** @interface: config
     *  @usage:
        $.grid.config( obj )
     */
    Grid.config = function(obj) {
        $.each(obj, function(k, o) {
            if (k === 'formatters') {
                new Formatters(o);
            }
            else if (k === 'editors') {
                new Editors(o);
            }
            else {
                defaults[k] = o;
            }
        });
    };


    /* Plugin Method
     * 
     * @param {optional String|Object} options 调用对应的方法|初始化或重载grid
     * @return {jqObject}
     */
    $.fn.grid = function(options) {
        var args = arguments;

        return this.each(function () {
            var me = $.data(this, 'grid');

            if (me) {
                switch (typeof options) {
                    case 'string':
                        me[options].apply(me, Array.prototype.slice.call(args, 1));
                        return;
                    case 'object':
                        me.config(options);
                        //传了columns就要重建结构
                        if (options.columns) {
                            me.headRowCount && me.destroy(); //生成过需要先移除
                            me._init();
                            return;
                        }
                        break;
                }
                me.query();
            }
            else if (options) {
                new Grid(this, options);
            }
        });
    };

    return $.grid = Grid;

})(jQuery);