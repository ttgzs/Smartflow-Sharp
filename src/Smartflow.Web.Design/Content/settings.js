(function () {

    var config = {
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

    function init(option) {
        config = $.extend(config, option);
        loadTree();
        loadSelect();
        bindEvent();
    }

    function bindEvent() {
        $(".card-tabs-bar>a").click(function () {
            var
                self = $(this),
                text = self.text();
            if (!self.hasClass("active")) {
                self.addClass("active");
            }
            $(".card-tabs-bar a.active").not(self).each(function () {
                $(this).removeClass("active");
            });
            $(".card-tabs-stack >div[data-tab]").each(function () {
                var tab = $(this),
                    title = tab.attr("data-tab");
                tab[(text == title) ? 'show' : 'hide']();
            });
        });

        $("#userAssign").on("dblclick", "li", function () {
            $(this).remove();
            doSearch();
        });
        $("#btnSearch").click(function () {
            doSearch();
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

            $("#roleGrid").html(build.toString());

            $("#roleGrid").on("dblclick", "li", function () {
                $("#roleAssign").append(this);
            });

            $("#roleAssign").on("dblclick", "li", function () {
                $("#roleGrid").append(this);
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

            $("#roleAssign").html(Abuild.toString());
        };
        ajaxService(ajaxSettings);
    }



    function getSettings() {
        var settings = {},
            roles = [],
            actors = [],
            expressions = [];

        settings.name = $("#txtNodeName").val();

        $("#roleAssign li").each(function () {
            roles.push({ id: $(this).attr("id"), name: $(this).attr("name") });
        });

        $("#userAssign li").each(function () {
            actors.push({ id: $(this).attr("id"), name: $(this).text() });
        });

        $("#transitions li").each(function () {
            var input = $(this).find("input");
            expressions.push({ id: input.attr("id"), expression: input.val() });
        });

        var cmdText = $("#txtCommand").val(),
            ddlSelect = $("#ddlRuleConfig option:selected");

        if (cmdText != '' && cmdText) {
            var data = JSON.parse(unescape(ddlSelect.attr("data")));
            settings.command = {
                text: cmdText,
                connection: data.Connection,
                dbcategory: data.DbCategory,
                commandtype: 'text'
            };
        }
        settings.expressions = expressions;
        settings.actors = actors;
        settings.group = roles;
        return settings;

    }

    function setSettings(nx) {
        $("#txtNodeName").val(nx.name);

        var lineCollection = nx.getTransitions();

        loadRoleGrid(nx.group);
        //loadUserGrid(nx.actors);

        if (lineCollection.length > 0) {
            var unqiueId = 'lineTo';
            var ruleArray = [];
            var build = new StringBuilder();

            $.each(lineCollection, function (i) {
                /*
                build.append(config.start)
                     .append('li')
                     .append(config.end)
                     .append(config.start)
                     .append('label')
                     .append(config.space)
                     .append('for')
                     .append(config.equal)
                     .append(config.lQuotation)
                     .append(this.id)
                     .append(config.rQuotation)
                     .append(config.end)
                     .append(this.name)
                     .append(config.beforeClose)
                     .append('label')
                     .append(config.afterClose)
                     .append(config.start)
                     .append('input')
                     .append(config.space)
                     .append('type')
                     .append(config.equal)
                     .append(config.lQuotation)
                     .append('text')
                     .append(config.rQuotation)*/

                ruleArray.push("<li><label for='" + this.id + "'>" + this.name + "</label><input type='text' value='" + this.expression + "' id='" + this.id + "' /></li>");
            });
            $("#transitions").html(ruleArray.join(""));
        }
        if (nx.command) {
            var cmd = nx.command;
            $("#txtCommand").val(cmd.text);
            $("#ddlRuleConfig").val(cmd.id);
        }
    }

    function loadTree() {

        var ajaxSettings = {
            url: config.treeUrl
        }

        var ts = {
            view: {
                showLine: true
            },
            data: {
                simpleData: {
                    enable: true,
                    idKey: "Code",
                    pIdKey: "ParentCode",
                    rootPId: ""
                },
                key: {
                    name: "Name",
                    children: "Children"
                }
            },
            callback: {
                onClick: function (treeId, treeNode, data) {
                    doSearch(data);
                }
            }
        };

        ajaxSettings.success = function (serverData) {
            var tree = $.fn.zTree.init($("#tree"), ts, serverData);
            var node = tree.getNodesByParam("Code", "000", null);
            if (node) {
                tree.expandAll(true);
            }
        }
        ajaxService(ajaxSettings);
    }

    function doSearch(data) {
        var search = $("#txtSearchKey").val(),
            postSettings = {},
            searchParam = {};

        if (search) {
            searchParam.searchKey = search;
        }

        var zTreeObj = $.fn.zTree.getZTreeObj("tree"),
            selectedNodes = zTreeObj.getSelectedNodes();

        if (selectedNodes.length > 0) {
            var node = selectedNodes[0];
            data = (data || node);
            searchParam.code = data.Code;
        }

        var userAssing = [];

        $("#userAssign li").each(function () {
            var id = $(this).attr('id'),
                text = $(this).text();

            userAssing.push(id);
        });

        if (userAssing.length > 0) {
            searchParam.userIdStr = userAssing.join(",");
        }

        postSettings.postData = searchParam;
        $("#userlist").jqGrid('setGridParam', postSettings).trigger("reloadGrid");
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
            $("#ddlRuleConfig").html(build.toString());
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

    window.SMF = {
        init: init,
        setSettings: setSettings,
        getSettings: getSettings
    };
})();