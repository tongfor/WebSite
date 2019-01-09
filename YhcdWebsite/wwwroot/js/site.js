// 对Date的扩展，将 Date 转化为指定格式的String   
// 月(M)、日(d)、小时(h)、分(m)、秒(s)、季度(q) 可以用 1-2 个占位符，   
// 年(y)可以用 1-4 个占位符，毫秒(S)只能用 1 个占位符(是 1-3 位的数字)   
// 例子：   
// (new Date()).Format("yyyy-MM-dd hh:mm:ss.S") ==> 2006-07-02 08:09:04.423   
// (new Date()).Format("yyyy-M-d h:m:s.S")      ==> 2006-7-2 8:9:4.18   
Date.prototype.Format = function (fmt) { //author: meizz   
    var o = {
        "M+": this.getMonth() + 1,                 //月份   
        "d+": this.getDate(),                    //日   
        "h+": this.getHours(),                   //小时   
        "m+": this.getMinutes(),                 //分   
        "s+": this.getSeconds(),                 //秒   
        "q+": Math.floor((this.getMonth() + 3) / 3), //季度   
        "S": this.getMilliseconds()             //毫秒   
    };
    if (/(y+)/.test(fmt))
        fmt = fmt.replace(RegExp.$1, (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o)
        if (new RegExp("(" + k + ")").test(fmt))
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length === 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

$(document).ready(function () {
    if (window.ActiveXObject || "ActiveXObject" in window) {
        //ie brower
        $(".logo").hide();
        $(".logo-small").removeClass("hidden-lg").removeClass("hidden-md");
    }

    $("#top .navBtn").click(function () {
        if ($("#top ul:visible").length === 0) {
            $("#top ul").show()
        } else {
            $("#top ul").hide()
        }

        $("#top .userBtn").click(function () {
            if ($("#top .searck:visible").length === 0) {
                $("#top .searck").show()
            } else {
                $("#top .searck").hide()
            }
        })
    })

    $("#banner").slide({ mainCell: ".bd ul", autoPlay: true });


    $("#leftsead a").hover(function () {
        if ($(this).prop("className") === "youhui") {
            $(this).children("img.hides").show();
        } else {
            $(this).children("img.hides").show();
            $(this).children("img.shows").hide();
            $(this).children("img.hides").animate({ marginRight: '0px' }, 'slow');
        }
    }, function () {
        if ($(this).prop("className") === "youhui") {
            $(this).children("img.hides").hide('slow');
        } else {
            $(this).children("img.hides").animate({ marginRight: '-143px' }, 'slow', function () { $(this).hide(); $(this).next("img.shows").show(); });
        }
    });

    $("#top_btn").click(function () { if (scroll === "off") return; $("html,body").animate({ scrollTop: 0 }, 600); });

    setTimeout(animation_gradient_run, 1500);

    //文章页面进行首行缩进
    var objs = $(".news-content p:not([style*='text-indent'])");
    if (objs) {
        objs.css("text-indent", "2em");
        //去除多余的空格
        objs.each(function () {
            $(this).html(myTrimLeft($(this).html(), 4));
        });
    }
    var first_P = $(".news-content p:first");
    var first_P_Text = first_P.text().trim();
    if (":" === first_P_Text.charAt(first_P_Text.length - 1) || "：" === first_P_Text.charAt(first_P_Text.length - 1)) {
        //通知首行不缩进
        first_P.css("text-indent", "");
        first_P.html(myTrimLeft(first_P.html(), 4));
        //first_P.html(first_P.html().replace(/&nbsp;/g, "");
    }
});

function animation_gradient_run() {
    $(".logo").removeClass("logo-animation-revolveDrop");
    $(".logo").addClass("logo-animation-gradient");
}

//String.prototype.
function myTrimLeft(s, n) {
    //替换前n个&nbsp;
    var strLeft = s.substr(0, n * 6);
    var strRight = s.length < n * 6 ? "" : s.substr(n * 6 - 1, s.length);
    for (var i = 0; i < n; i++) {
        strLeft = strLeft.replace("&nbsp;", "");
    }
    s = strLeft + strRight;
    return s;
}