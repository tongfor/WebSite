; var topMenuFunc = (function () {
    var menuObjs = $("#top>div>ul")
    var currentUrl = window.location.href

    //菜单初始化
    var _topMenuInit = function () {
        menuObjs.empty()
        for (var p in topMenu) {
            var li = '<li><a href="' + topMenu[p].menuLink + '">' + topMenu[p].menuTitle + '</a></li>'
            menuObjs.append(li)
        }
    }

    //按当前链接匹配菜单
    var _topMenuMatch = function () {
        menuObjs.children("li").each(function (i, e) {
            var href = $(this).find("a").attr("href")
            if (currentUrl.split('?')[0].indexOf(href.split('?')[0].split('.')[0]) !== -1 && href !== "/") {
                $(this).addClass("current")
            }
            else {
                $(this).removeClass("current")
            }
        })
    }
    return {
        
        topMenuInit: _topMenuInit,
        topMenuMatch: _topMenuMatch
    }
}())

$(function () {
    topMenuFunc.topMenuInit()
    topMenuFunc.topMenuMatch()
});