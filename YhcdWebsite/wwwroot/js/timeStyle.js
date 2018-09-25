$(function () {
    var timeObjs = $(".calendar")
    timeObjs.each(function (i, e) {
        var addTime = new Date($(e).text())
        var str = '<div class="calendar-day">' + addTime.Format("dd")
        str += '</div><div class="calendar-date">' + addTime.Format("yyyy-MM")
        str += '</div>'
        $(e).empty()
        $(e).append(str)
    })
})