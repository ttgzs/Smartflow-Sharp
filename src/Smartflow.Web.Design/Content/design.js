/*
 License: https://github.com/chengderen/Smartflow/blob/master/LICENSE 
 Home page: https://www.smartflow-sharp.com
 */
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
            wfName = window.prompt(designConfig.message,'');
        }
        if (wfName) {
            var data = $.extend(exportToObject, {
                APPELLATION: wfName,
                IDENTIFICATION: designConfig.id
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
                    flowName = serverData.APPELLATION;
                    SMF.revert(serverData.STRUCTUREXML);
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


    function openWin(elementId) {
        var ht = $("#" + elementId).html();
        //自定页
        layer.open({
            title: '注意事项',
            type: 1,
            closeBtn: 1,
            //skin: 'layui-layer-rim', //加上边框
            area: ['520px', '300px'], //宽高
            anim: 2,
            shadeClose: true, //开启遮罩关闭
            content: ht
        });
    }

    window.design = {
        init: initConfig,
        openWin: openWin,
        save: saveflow
    }

})(jQuery);