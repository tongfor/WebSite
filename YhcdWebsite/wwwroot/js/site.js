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
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
    return fmt;
};

(function () {
    var slideMenu = function () {
        //竖风琴特效
        var sp, st, t, m, sa, l, w, gw, ot;
        return {
            destruct: function () {
                if (m) {
                    clearInterval(m.htimer);
                    clearInterval(m.timer);
                }
            },
            build: function (sm, sw, mt, s, sl, h) {
                sp = s;
                st = sw;
                t = mt;
                m = document.getElementById(sm);
                sa = m.getElementsByTagName('li');
                l = sa.length;
                w = m.offsetWidth;
                gw = w / l;
                ot = Math.floor((w - st) / (l - 1));
                var i = 0;
                for (i; i < l; i++) {
                    s = sa[i];
                    s.style.width = gw + 'px';
                    this.timer(s)
                }
                if (sl != null) {
                    m.timer = setInterval(function () {
                        slideMenu.slide(sa[sl - 1])
                    }, t)
                }
            },
            timer: function (s) {
                s.onmouseover = function () {
                    clearInterval(m.htimer);
                    clearInterval(m.timer);
                    m.timer = setInterval(function () {
                        slideMenu.slide(s)
                    }, t);
                    //console.log($(this).find('.mask_b').html());
                    $(this).find('.mask_b').hide();
                    //console.log($(this).find('.mask_b').attr("style"));
                }
                s.onmouseout = function () {
                    clearInterval(m.timer);
                    clearInterval(m.htimer);
                    m.htimer = setInterval(function () {
                        slideMenu.slide(s, true)
                    }, t);
                    //console.log($(this).find('.mask_b').html());
                    $(this).find('.mask_b').show();
                    //console.log($(this).find('.mask_b').attr("style"));
                }
            },
            slide: function (s, c) {
                var cw = parseInt(s.style.width);
                if ((cw < st && !c) || (cw > gw && c)) {
                    var owt = 0; var i = 0;
                    for (i; i < l; i++) {
                        if (sa[i] != s) {
                            var o, ow; var oi = 0; o = sa[i]; ow = parseInt(o.style.width);
                            if (ow < gw && c) {
                                oi = Math.floor((gw - ow) / sp);
                                oi = (oi > 0) ? oi : 1;
                                o.style.width = (ow + oi) + 'px';
                                //console.log(o);
                                //console.log(o.style.width);
                            } else if (ow > ot && !c) {
                                oi = Math.floor((ow - ot) / sp);
                                oi = (oi > 0) ? oi : 1;
                                o.style.width = (ow - oi) + 'px';
                                //console.log(o);
                                //console.log(o.style.width);
                            }
                            if (c) {
                                owt = owt + (ow + oi)
                            } else {
                                owt = owt + (ow - oi)
                            }
                        }
                    }
                    s.style.width = (w - owt) + 'px';
                } else {
                    if (m.htimer)
                        clearInterval(m.htimer)
                    if (m.timer)
                        clearInterval(m.timer);
                }
            }
        };
    }();
    slideMenu.build('sm', 280, 20, 10, 1);
})();

