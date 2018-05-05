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
    
    function loadRoleGrid(group,url) {

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

    function getSettings() {
        var settings = {},
            roleArray = [];

        settings.name = $("#txtName").val();

        $("#roleAssign li").each(function () {
            roleArray.push({ id: $(this).attr("id"), name: $(this).attr("name") });
        });

        settings.group = roleArray;
        return settings;
    }

    function setSettings(name, group,url) {
        $("#txtName").val(name);
        loadRoleGrid(group,url);
    }

    window.SMF = {
        init: init,
        setSettings: setSettings,
        getSettings: getSettings
    };
})();