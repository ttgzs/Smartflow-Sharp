(function () {
    var
        flowName,
        designConfig = {
            message: '请输入流程名称',
            success: '操作成功'
        }

    function saveflow() {
        var exportToObject = SMF.exportToJSON(),
            wfName;

        if (!exportToObject) return;

        if (flowName) {
            wfName = window.prompt(designConfig.message, flowName);
        } else {
            wfName = window.prompt(designConfig.message);
        }
        if (wfName) {
            var data = $.extend(exportToObject, {
                NAME: wfName,
                WFID: designConfig.id
            });

            var settings = {
                url: designConfig.saveUrl,
                data: data,
                success: function () {
                    alert(designConfig.success);
                }
            };
            ajaxService(settings);
        } 
    }

    function openConfig(nx) {
        var settings = {
            type: 2,
            title: false,
            area: ['630px', '460px'],
            shade: 0.8,
            closeBtn: 0,
            shadeClose: false,
            content: [designConfig.windowUrl, 'no'],
            btn: ['确定', '关闭'],
            btn2: function () {
                layer.closeAll();
            }
        };

        settings.yes = function (index, dom) {
            var frameContent = getDOMFrame(dom);
            frameContent.SMF.setSettingsToNode(nx);
            layer.close(index);
        };
        settings.success = function (dom, index) {
            var frameContent = getDOMFrame(dom);
            frameContent.SMF.setNodeToSettings(nx);
        };
        layer.open(settings);
    }

    function initConfig(config) {
        designConfig = $.extend(designConfig, config);
        SMF.init($.extend({ container: designConfig.container }, { dblClick: openConfig }));

        if (designConfig.id) {
            var settings = {
                url: designConfig.instanceUrl,
                data: { WFID: designConfig.id },
                success: function (serverData) {
                    flowName = serverData.NAME;
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
            cache: false
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