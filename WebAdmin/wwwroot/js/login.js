jQuery(document)
    .ready(function () {
        FirefoxOrIE();
        App.initLogin();
    });
$(document)
    .ready(function () {
        $("#VerifyCodeok").hide();
        $("#username")
            .mousedown(function () {
                $("#LoginValidationMessage").html("");
            });
        $("password")
            .mousedown(function () {
                $("#LoginValidationMessage").html("");
            });
        $("#VerifyCode")
            .mousedown(function () {
                $("#LoginValidationMessage").html("");
            });
        var verifyImageCanClick = true;
        $("#VerifyCode")
            .keyup(function () {
                if ($("#VerifyCode").val().length >= 4) {
                    $("#VerifyCodeok").show();
                    $.ajax({
                        url: "/Account/VerifyCodeValidate",
                        type: "post",
                        data: { "verifycode": $("#VerifyCode").val() },
                        dataType: "json",
                        timeout: 5000,
                        success: function (data) {
                            if (data.Status === 0 || data.status === 0) {
                                $("#VerifyCodeok").removeClass("icon-remove");
                                $("#VerifyCodeok").addClass("icon-ok");
                                $("#VerifyCode").attr("readonly", "readonly");
                                verifyImageCanClick = false;
                            } else {
                                $("#VerifyCodeok").removeClass("icon-ok");
                                $("#VerifyCodeok").addClass("icon-remove");
                            }
                        }
                    });
                }
            });
        $("#VerifyImage").click(function () {
            if (verifyImageCanClick) {
                this.src = '/Account/VerifyImage?r=' + Math.random();
            }
            return false;
        });
    });

function FirefoxOrIE() {
    /*
    IE浏览器版本检测
    */
    var user_agent = navigator.userAgent.toLowerCase();
    var b = false;
    var c = '';
    if (user_agent.indexOf("msie 9.0") > -1 && user_agent.indexOf("trident/6.0") > -1) {
        //IE10（兼容模式）
        b = false;
        c = 'IE10（兼容模式）';
    } else if (user_agent.indexOf("msie 8.0") > -1 && user_agent.indexOf("trident/6.0") > -1) {
        //IE10（兼容模式）
        b = false;
        c = 'IE10（兼容模式）';
    } else if (user_agent.indexOf("msie 7.0") > -1 && user_agent.indexOf("trident/6.0") > -1) {
        //IE10（兼容模式）
        b = false;
        c = 'IE10（兼容模式）';
    } else if (user_agent.indexOf("msie 9.0") > -1) {
        //IE9
        b = false;
        c = 'IE9';
    } else if (user_agent.indexOf("msie 7.0") > -1 && user_agent.indexOf("trident/5.0") > -1) {
        //IE9（兼容模式）
        b = false;
        c = 'IE9（兼容模式）';
    } else if (user_agent.indexOf("msie 8.0") > -1 && user_agent.indexOf("trident/5.0") > -1) {
        //IE9（兼容模式）
        b = false;
        c = 'IE9（兼容模式）';
    } else if (user_agent.indexOf("msie 8.0") > -1) {
        //IE8
        b = true;
        c = 'IE8';
    } else if (user_agent.indexOf("msie 7.0") > -1 && user_agent.indexOf("trident/4.0") > -1) {
        //IE8（兼容模式）
        b = true;
        c = 'IE8（兼容模式）';
    } else if (user_agent.indexOf("msie 7.0") > -1) {
        //IE7
        b = true;
        c = 'IE7';
    } else if (user_agent.indexOf("msie 6.0") > -1) {
        //IE6
        b = true;
        c = 'IE6';
    }

    if (b) {
        if (confirm('提示：您当前IE浏览器版本为' + c + '，由于版本太旧,页面无法正常加载，请升级浏览器至IE9及以上版本，或下载其他浏览器！')) {
            window.open('http://rj.baidu.com/soft/lists/3');
        }
    }
}