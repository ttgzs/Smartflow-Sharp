(function () {
    var
        designConfig = {
            message: '请输入流程名称',
            success: '操作成功'
        }

    function saveflow() {
        var json = SMF.exportToJSON(),
            wfName = window.prompt(designConfig.message);
        if (wfName) {
            var data = {
                NAME: wfName, XML: json.XML, IMAGE: json.IMAGE, WFID: designConfig.id
            },
            settings = {
                url: designConfig.saveUrl,
                data: data,
                success: function () {
                    alert(designConfig.success);
                }
            };
            ajaxService(settings);
        } else {
            alert(designConfig.message);
        }
    }

    function openConfig(nx) {
        var settings = {
            type: 2,
            title: false,
            area: ['630px', '360px'],
            shade: 0.8,
            closeBtn: 0,
            shadeClose: false,
            content: [designConfig.windowUrl, 'no'],
            btn: ['确定', '关闭'],
            btn2: function () {
                layer.closeAll();
            }
        };

        settings.yes = function (dom, index) {
            var frameContent = getDOMFrame(dom),
                settings = frameContent.SMF.getSettings();

            if (settings.name) {
                nx.name = settings.name;
                nx.brush.text(nx.name);
            }

            nx.group = settings.group;
            nx.actors = settings.actors;
            nx.command = settings.command;
            nx.setExpression(settings.expressions);
            layer.close(index);
        };
        settings.success = function (dom, index) {
            var frameContent = getDOMFrame(dom);
            frameContent.SMF.setSettings(nx);
        };
        layer.open(settings);
    }

    function initConfig(config) {
        designConfig = $.extend(designConfig, config);
        SMF.init(designConfig.container, { dblClick: openConfig });

        if (designConfig.id) {
            var settings = {
                url: designConfig.instanceUrl,
                data: { WFID: designConfig.id },
                success: function (serverData) {
                    SMF.revert(serverData.IMAGE);
                }
            };
            ajaxService(settings);
        } else {
            $.each(['start', 'end'], function (i, value) {
                SMF.create(value);
            });
        }
    }

    function ajaxService(settings) {
        var defaultSettings = $.extend({
            dataType: 'json',
            type: 'post',
            cache:false
        }, settings);
        $.ajax(defaultSettings);
    }

    function getDOMFrame(dom) {
        var frameId = dom.find("iframe").attr('id');
        return (document.getElementById(frameId).contentWindow);
    }

    window.design = {
        init: initConfig,
        save: saveflow
    }

})(jQuery);