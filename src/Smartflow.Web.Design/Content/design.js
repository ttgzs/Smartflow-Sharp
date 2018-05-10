(function () {

    function registerTabClick() {
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

            $(".card-tabs-stack >div").each(function () {
                var tab = $(this),
                    title = tab.attr("data-tab");
                if (text == title) {
                    tab.show();
                } else {
                    tab.hide();
                }
            });
        });
    }

    function init() {

        registerTabClick();
    }

    function loadRoleGrid(group, url) {

        var settings = {
            type: 'post',
            dataType: 'json'
        };

        settings.url = url;

        if (group.length > 0) {

            var roleIds = [];
            $.each(group, function () {
                roleIds.push(this.id);
            });

            settings.data = {
                roleIds: roleIds.join(',')
            };
        }

        settings.success = function (serverData) {
            var elements = [], assignElements = [];
            $.each(serverData, function () {
                elements.push("<li  id=" + this.ID + " name=" + this.Name + ">" + this.Name + "</li>");
            });

            $("#roleGrid").html(elements.join(""));

            $("#roleGrid").on("dblclick", "li", function () {
                $("#roleAssign").append(this);
            });

            $("#roleAssign").on("dblclick", "li", function () {
                $("#roleGrid").append(this);
            });

            $.each(group, function () {
                assignElements.push("<li  id=" + this.id + " name=" + this.name + ">" + this.name + "</li>");
            });

            $("#roleAssign").html(assignElements.join(""));
        };
        $.ajax(settings);
    }

    function loadUserGrid(actors) {

        var elementActors = [],
            settings = {
                url: 'GetUserList',
                datatype: "json",
                colNames: ['ID', '姓名', '部门'],
                colModel: [
                             { name: 'ID', index: 'ID', hidden: true },
                             { name: 'EmployeeName', index: '姓名', align: 'center', width: 120, sortable: false },
                             { name: 'OrgName', index: '部门', sortable: false }
                ],
                rowNum: 5,
                rowList: [5, 10, 15],
                pager: '#pager',//表格页脚的占位符(一般是div)的id
                /*sortname : 'id',//初始化的时候排序的字段
                sortorder : "desc",//排序方式,可选desc,asc*/
                mtype: "post",//向后台请求数据的ajax的类型。可选post,get
                viewrecords: true,
                pagerpos: 'left',
                /* postData:'',*/
                recordpos: 'right',
                width: 342,
                shrinkToFit: true,
                rownumbers: true,
                toolbar: [true, 'top'],
                ondblClickRow: function (rowid, irow) {
                    var row = $("#userlist").jqGrid('getRowData', rowid);
                    $("#userlist").jqGrid('delRowData', rowid);
                    $("#userAssign").append("<li id=" + row.ID + ">" + row.EmployeeName + "</li>");
                }
            };






        if (actors) {
            $.each(actors, function () {
                elementActors.push(this.id);

                $("#userAssign").append("<li id=" + this.id + ">" + this.name + "</li>");

            });
            if (elementActors.length > 0) {
                settings.postData = { userIdStr: elementActors.join(",") };
            }
        }

        $("#userlist").jqGrid(settings);
        $("#userlist").jqGrid('navGrid', '#pager', { edit: false, search: false, refresh: false, add: false, del: false });
        $("#t_userlist").html(document.getElementById("tbTemplate").innerHTML);


        $("#userAssign").on("dblclick", "li", function () {
            $(this).remove();
            doSearch();
        });

        $("#btnSearch").click(function () {
            doSearch();
        });
    }

    function getSettings() {
        var settings = {},
            roleArray = [],
            actorArray = [];

        settings.name = $("#txtName").val();

        $("#roleAssign li").each(function () {
            roleArray.push({ id: $(this).attr("id"), name: $(this).attr("name") });
        });

        $("#userAssign li").each(function () {
            actorArray.push({ id: $(this).attr("id"), name: $(this).text() });
        });

        settings.actors = actorArray;
        settings.group = roleArray;
        return settings;
    }

    function setSettings(name, group, actors, url) {
        $("#txtName").val(name);
        loadRoleGrid(group, url);

        loadUserGrid(actors);
    }

    window.SMF = {
        init: init,
        setSettings: setSettings,
        getSettings: getSettings
    };
})();