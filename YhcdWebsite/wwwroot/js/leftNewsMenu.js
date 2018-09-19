; var leftNewsMenuFunc = (function () {
    var menuObjs = $(".menu-left ul")
    var currentUrl = window.location.href

    //菜单初始化
    var _leftNewsMenuInit = function () {
        menuObjs.empty()
        for (var p in leftNewsMenu) {
            var li = '<li><a href="' + leftNewsMenu[p].menuLink + '?id=' + leftNewsMenu[p].id + '">' + leftNewsMenu[p].menuTitle + '</a></li>'
            menuObjs.append(li)
        }
    }

    //菜单匹配当前菜单
    var _leftNewsMenuMatch = function () {
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
        leftNewsMenuInit: _leftNewsMenuInit,
        leftNewsMenuMatch: _leftNewsMenuMatch
    }
}())

$(function () {
    leftNewsMenuFunc.leftNewsMenuInit()
    leftNewsMenuFunc.leftNewsMenuMatch()
});