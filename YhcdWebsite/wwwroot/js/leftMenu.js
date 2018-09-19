; var leftMenuFunc = (function () {
    var menuObjs = $(".menu-left ul")
    var currentUrl = window.location.href

    //菜单初始化
    var _leftMenuInit = function () {
        menuObjs.empty()
        for (var p in leftMenu) {
            var li = '<li><a href="' + leftMenu[p].menuLink + '">' + leftMenu[p].menuTitle + '</a></li>'
            menuObjs.append(li)
        }
    }

    //菜单匹配当前菜单
    var _leftMenuMatch = function () {
        menuObjs.children("li").each(function (i, e) {
            var href = $(this).find("a").attr("href")
            if (currentUrl.indexOf(href) !== -1) {
                $(this).addClass("current")
            }
            else {
                $(this).removeClass("current")
            }
        })
    }
    return {
        leftMenuInit: _leftMenuInit,
        leftMenuMatch: _leftMenuMatch
    }
}())

$(function () {
    leftMenuFunc.leftMenuInit()
    leftMenuFunc.leftMenuMatch()
});