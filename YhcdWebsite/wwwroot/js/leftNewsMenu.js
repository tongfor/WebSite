; var leftNewsMenuFunc = (function () {
    var menuObjs = $(".menu-left ul")
    var currentUrl = window.location.href

    //菜单初始化
    var _leftNewsMenuInit = function () {
        menuObjs.empty()
        for (var p in leftNewsMenu) {
            var li = '<li><a href="' + leftNewsMenu[p].menuLink + '-' + leftNewsMenu[p].id + '.html">' + leftNewsMenu[p].menuTitle + '</a></li>'
            menuObjs.append(li)
        }
    }

    //菜单匹配当前菜单
    var _leftNewsMenuMatch = function () {
        menuObjs.children("li").each(function (i, e) {
            var href = $(this).find("a").attr("href")
            if (currentUrl.indexOf(href) !== -1) {
                $(this).addClass("current")
                _appendCrumb($(this).find("a").text())
            }
            else {
                $(this).removeClass("current")
            }
        })
    }

    //设定面包屑导航
    var _appendCrumb = function (title) {
        var str = '> ' + title.trim()
        $(".crumb").append(str)
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