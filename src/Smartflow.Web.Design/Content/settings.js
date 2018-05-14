(function () {
    var
        assignToUserSelector = '#userAssign',
        searchButtonSelector = '#btnSearch',
        roleGridSelector = '#roleGrid',
        assignToRoleSelector = '#roleAssign',
        unitSelector = '#txtUnit',
        treeSelector = '#tree',
        orgCodeSelector = '#hidOrgCode',
        cmdTextSelector = '#txtCommand',
        optionSelector = '#ddlRuleConfig option:selected',
        ruleSelector = '#ddlRuleConfig',
        searchTextSelector = '#txtSearchKey',
        itemTemplate = "<li id=%{0}%>%{1}%</li>",
        lineTemplate = "<tr><td class='smartflow-header'>%{0}%</td><td><input type='text' value=%{1}% id=%{2}% class='layui-input smartflow-input' /></td></tr>";
    tabConfig = {
        node: ['#tab li[category=rule]'],
        decision: ['#tab li[category=role]', '#tab li[category=actor]']
    },
    config = {
        //开始
        start: '<',
        //结束
        end: '>',
        //左引号
        lQuotation: '"',
        //右引号
        rQuotation: '"',
        //闭合
        beforeClose: '</',
        //闭合
        afterClose: '/>',
        //等于
        equal: '=',
        //本身引用
        //空隙
        space: ' ',
    };

    $.extend(String.prototype, {
        format: function () {
            var regexp = /\{(\d+)\}/g;
            var args = arguments,
                escapeChar = '';
            var result = this.replace(regexp, function (m, i, o, n) {
                return args[i];
            });
            return result.replaceAll('%', escapeChar);
        },
        replaceAll: function (searchValue, replaceValue) {
            var regExp = new RegExp(searchValue, "g");
            return this.replace(regExp, replaceValue);
        }
    });

    function initOption(option) {
        config = $.extend(config, option);
    }

    function initEvent() {

        $(assignToUserSelector).on("dblclick", "li", function () {
            $(this).remove();
            doSearch();
        });

        $(searchButtonSelector).click(function () {
            doSearch();
        });

        $(unitSelector).click(function () {
            $(treeSelector).show();
        });

        $(treeSelector).hover(function () { }, function () {
            $(treeSelector).hide();
        });
    }

    function loadRoleGrid(group) {

        var ajaxSettings = { url: config.roleUrl };
        if (group.length > 0) {

            var roleIds = [];
            $.each(group, function () {
                roleIds.push(this.id);
            });

            ajaxSettings.data = {
                roleIds: roleIds.join(',')
            };
        }

        ajaxSettings.success = function (serverData) {
            var build = new StringBuilder(),
                Abuild = new StringBuilder();
            $.each(serverData, function () {
                build.append(config.start)
                    .append('li')
                    .append(config.space)
                    .append('id')
                    .append(config.equal)
                    .append(config.lQuotation)
                    .append(this.ID)
                    .append(config.rQuotation)
                    .append(config.space)
                    .append('name')
                    .append(config.equal)
                    .append(config.lQuotation)
                    .append(this.Name)
                    .append(config.rQuotation)
                    .append(config.end)
                    .append(this.Name)
                    .append(config.beforeClose)
                    .append('li')
                    .append(config.afterClose);
            });

            $(roleGridSelector).html(build.toString());

            $(roleGridSelector).on("dblclick", "li", function () {
                $(assignToRoleSelector).append(this);
            });

            $(assignToRoleSelector).on("dblclick", "li", function () {
                $(roleGridSelector).append(this);
            });

            $.each(group, function () {
                Abuild.append(config.start)
                    .append('li')
                    .append(config.space)
                    .append('id')
                    .append(config.equal)
                    .append(config.lQuotation)
                    .append(this.id)
                    .append(config.rQuotation)
                    .append(config.space)
                    .append('name')
                    .append(config.equal)
                    .append(config.lQuotation)
                    .append(this.name)
                    .append(config.rQuotation)
                    .append(config.end)
                    .append(this.name)
                    .append(config.beforeClose)
                    .append('li')
                    .append(config.afterClose);
            });

            $(assignToRoleSelector).html(Abuild.toString());
        };
        ajaxService(ajaxSettings);
    }
    function loadUserGrid(actors) {
        layui.use('table', function () {
            var table = layui.table,
                selector = '.layui-table-main table.layui-table tbody tr';
            var tableSettings = {
                elem: '#userList',
                url: config.userUrl,
                id: 'userGrid',
                method: 'post',
                size: 'sm',
                height: 'full',
                limit: 7,
                page: {
                    layout: ['count', 'prev', 'page', 'next', 'skip'],
                    groups: 1,
                    first: false,
                    last: false
                },
                cols: [[
                   { type: 'numbers', width: 60, align: 'center', title: '序号' },
                   { field: 'Name', width: 120, align: 'center', title: '姓名' },
                   { field: 'OrgName', width: 180, title: '部门', sort: false }
                ]],
                done: function (res, curr, count) {
                    $("#userList").next().find(selector).bind('dblclick', function () {
                        var index = $(this).attr("data-index"),
                            row = res.data[index];
                        $(assignToUserSelector).append(itemTemplate.format(row.ID, row.Name));
                        doSearch();
                    });
                }
            };

            if (actors.length > 0) {
                var aes = [],
                    build = new StringBuilder();
                $.each(actors, function () {
                    aes.push(this.id);
                    build.append(itemTemplate.format(this.id, this.name));
                });
                $(assignToUserSelector).append(build.toString());
                tableSettings.where = { userIdStr: aes.join(",") };
            }
            table.render(tableSettings);
        });
    }

    function setSettingsToNode(nx) {
        var roles = [],
            actors = [],
            expressions = [],
            name = $("#txtNodeName").val();

        if (nx.category === 'decision') {
            $("#transitions li").each(function () {
                var input = $(this).find("input");
                expressions.push({ id: input.attr("id"), expression: input.val() });
            });

            var cmdText = $(cmdTextSelector).val(),
                option = $(optionSelector);

            if (cmdText != '' && cmdText && option.length > 0) {
                var data = JSON.parse(unescape(option.attr("data")));
                nx.command = {
                    id: data.ID,
                    text: cmdText,
                    connection: data.Connection,
                    dbcategory: data.DbCategory,
                    commandtype: 'text'
                };
            }
            nx.setExpression(expressions);
        } else {

            $("#roleAssign li").each(function () {
                var self = $(this);
                roles.push({ id: self.attr("id"), name: self.attr("name") });
            });

            $("#userAssign li").each(function () {
                var self = $(this);
                actors.push({ id: self.attr("id"), name: self.text() });
            });

            if (name) {
                nx.name = name;
                nx.brush.text(nx.name);
            }
            nx.actors = actors;
            nx.group = roles;
        }
    }
    function setNodeToSettings(nx) {
        $("#txtNodeName").val(nx.name);
        if (nx.category === 'decision') {
            var lineCollection = nx.getTransitions();
            if (lineCollection.length > 0) {
                var unqiueId = 'lineTo',
                    build = new StringBuilder();
                $.each(lineCollection, function (i) {
                    build.append(lineTemplate.format(this.name, this.expression, this.id));
                });
                $("#transitions>tbody").html(build.toString());
            }
            if (nx.command) {
                var cmd = nx.command;
                $(cmdTextSelector).val(cmd.text);
                $(ruleSelector).val(cmd.id);
            }
            loadSelect();
        } else {
            loadRoleGrid(nx.group);
            loadUserGrid(nx.actors);
            loadTree();
            initEvent();
        }

        var items = tabConfig[nx.category];
        $.each(items, function (i, selector) {
            $(selector).hide();
        });
    }

    function loadTree() {

        var ajaxSettings = {
            url: config.treeUrl
        }

        var ts = {
            check: {
                enable: true
            },            view: {
                showLine: true
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "Code",
                    pIdKey: "ParentCode"
                },
                key: {
                    name: "Name",
                    children: "Children"
                }
            },
            callback: {
                onCheck: function () {
                    var treeObj = $.fn.zTree.getZTreeObj("tree"),
                        nodes = treeObj.getCheckedNodes(true),
                        text = [], value = [];
                    $.each(nodes, function () {
                        if (!this.isParent) {
                            text.push(this.Name);
                            value.push(this.Code);
                        }
                    });
                    $(unitSelector).val(text.join(","));
                    $(orgCodeSelector).val(value.join(","));
                }
            }
        };

        ajaxSettings.success = function (serverData) {
            var tree = $.fn.zTree.init($(treeSelector), ts, serverData);
            var node = tree.getNodesByParam("Code", "000", null);
            if (node) {
                tree.expandAll(true);
            }
        }
        ajaxService(ajaxSettings);
    }
    function doSearch(data) {
        layui.use('table', function () {
            var table = layui.table,
                assignSelector = '#userAssign li';

            var searchParam = {},
                search = $(searchTextSelector).val(),
                code = $(orgCodeSelector).val(),
                assign = [];

            $(assignSelector).each(function () {
                var self = $(this),
                    id = self.attr('id');
                assign.push(id);
            });

            if (assign.length > 0) {
                searchParam.userIdStr = assign.join(",");
            }

            searchParam.searchKey = search;
            searchParam.code = code;
            table.reload('userGrid', { where: searchParam });
        });
    }
    function loadSelect() {
        var settings = {
            url: config.configUrl
        };
        settings.success = function (serverData) {
            var build = new StringBuilder();
            $.each(serverData, function () {
                var data = JSON.stringify(this);

                build.append(config.start)
                     .append('option')
                     .append(config.space)
                     .append('data')
                     .append(config.equal)
                     .append(config.lQuotation)
                     .append(escape(data))
                     .append(config.rQuotation)
                     .append(config.space)
                     .append('value')
                     .append(config.equal)
                     .append(config.lQuotation)
                     .append(this.ID)
                     .append(config.rQuotation)
                     .append(config.end)
                     .append(this.Name)
                     .append(config.beforeClose)
                     .append('option')
                     .append(config.afterClose);
            });
            $(ruleSelector).html(build.toString());
        }
        ajaxService(settings);
    }


    function ajaxService(settings) {
        var defaultSettings = $.extend({
            dataType: 'json',
            type: 'post',
            cache: false
        }, settings);
        $.ajax(defaultSettings);
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

    window.SMF = {
        init: initOption,
        setNodeToSettings: setNodeToSettings,
        setSettingsToNode: setSettingsToNode
    };
})();