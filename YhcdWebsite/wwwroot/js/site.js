// leftMenuMatch
; var leftMenuMatch = function () {
    var menuObjs = $(".menu-left ul")
    var currentUrl = window.location.href

    menuObjs.children("li").each(function (i, e) {
        var href = $(this).find("a").attr("href")
        if (currentUrl.indexOf(href) != -1) {
            $(this).addClass("current")
        }
        else {
            $(this).removeClass("current")
        }
        console.log($(this).html())
    })    
}

// leftMenuMatch End
$(function () {
    leftMenuMatch();
});