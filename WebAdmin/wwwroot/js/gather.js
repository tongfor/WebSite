//互联网信息采集对象
; var siteGather = (function () {
    var _menuData = [
        {
            "id": 1,
            "siteName": "市经委",
            "siteKey": "cdgy"
        }, {
            "id": 2,
            "siteName": "省经信厅",
            "siteKey": "jxt"
        }, {
            "id": 3,
            "siteName": "市科技局",
            "siteKey": "cdst"
        }, {
            "id": 4,
            "siteName": "省科技厅",
            "siteKey": "kjt"
        }, {
            "id": 5,
            "siteName": "高新区",
            "siteKey": "cdht"
        }, {
            "id": 6,
            "siteName": "天府新区",
            "siteKey": "cdtf"
        }, {
            "id": 7,
            "siteName": "金牛区",
            "siteKey": "jnq"
        }, {
            "id": 8,
            "siteName": "锦江区",
            "siteKey": "jjq"
        }, {
            "id": 9,
            "siteName": "武侯区经科局",
            "siteKey": "whqjkj"
        }, {
            "id": 10,
            "siteName": "青羊区科经局",
            "siteKey": "qyqkjj"
        }, {
            "id": 11,
            "siteName": "成华区经科局",
            "siteKey": "chqjkj"
        }
    ];
    var menuObjs = $("#gather-list")

    //菜单初始化
    var _gatherMenuInit = function () {
        menuObjs.empty()
        var ul = "<ul>\n"
        for (var i in _menuData) {            
            ul += '    <li class="floatleft">\n'
            ul += '<input value="采集' + _menuData[i].siteName
            ul += '信息" id="g_' + _menuData[i].siteKey + '_classId_2" data-classId="2"'
            ul += 'data-siteKey="' + _menuData[i].siteKey
            ul += '" type = "button" class="gatherbtn" />\n'
            ul += '<br />\n'
            ul += '<span id="submitloading_' + _menuData[i].siteKey
            ul += '_classId_2" style="display:none;text-align:center;">采集开始'
            ul += '<br /><img src="/images/loading.gif" /></span >\n'
        }
        ul += '</ul>\n'
        menuObjs.append(ul)
    }

    //在标签中显示采集结果
    var _showGatherResult = function (tagClassName, data) {
        if (!tagClassName) {
            return
        }
        var o = $("." + tagClassName)
        var showHtml = ""
        if (data && data.gatheredArticleList) {
            showHtml += '<p>在' + data.gatherTime
            showHtml += '从' + data.siteName + '采集了' + data.gatheredArticleList.length + '条信息:</p>\n'
            for (var i in data.gatheredArticleList) {
                showHtml += '<p><a href="' + data.resultShowDomainName
                showHtml += '/Article/Details-' + data.gatheredArticleList[i].id
                showHtml += '.html" title="' + data.gatheredArticleList[i].title
                showHtml += '" target="_blank">' + data.gatheredArticleList[i].title
                showHtml += '</a>'
                showHtml += isRecent(data.gatheredArticleList[i].addTime, 3)
                    ? '[<span class="text-error">' + data.gatheredArticleList[i].addTime + '</span>]'
                    : '[<span>' + data.gatheredArticleList[i].addTime + '</span>]'
                showHtml += '&nbsp; <a href="' + data.gatheredArticleList[i].gatherurl
                showHtml += '" title="原文地址" target="_blank">原文地址</a>'
                showHtml+= '</p>\n'
            }
        }
        if (0 === o.html().length || "&nbsp;" === o.html()) {
            showHtml = '<p>采集结果如下：</p>\n' + showHtml
        }
        o.append(showHtml)
        o.scrollTop(o.prop('scrollHeight'))
    }

    function isRecent(strDate, differDays) {
        var d1 = new Date(strDate);
        if (!d1 || !differDays || 0 === differDays.length || isNaN(differDays)) {
            return false;
        }
        var d2 = new Date();
        var differMilliseconds = d2.getTime() - d1.getTime();
        return differMilliseconds <= differDays * 24 * 3600 * 1000
    }

    return {
        menuData: _menuData,
        gatherMenuInit: _gatherMenuInit,
        showGatherResult: _showGatherResult
    }
}())
$(function () {
    siteGather.gatherMenuInit()

    $(".gatherbtn").click(function (e) {
        var obj = $(e.target);
        var classId = obj.attr("data-classId");
        var siteKey = obj.attr("data-siteKey")
        var loadingId = "submitloading_" + siteKey + "_" + "classId" + "_" + classId;
        var paras = {
            "siteKey": siteKey,
            "pageStartNo": 1,
            "pageEndNo": 2,
            "classId": classId
        };
        $.ajax({
            url: "/Spider/GatherWebsite",
            type: "POST",
            data: paras,
            timeout: 3000000, //超时时间设置，单位毫秒
            beforeSend: function () {
                obj.attr("disabled", "true");//防止连击
                $("#" + loadingId).show();
            },
            success: function (result, status) {
                if (result && "success" === status && 0 === result.status) {
                    //alert(result.msg);
                    //window.top.location.reload();
                    //window.top.tb_remove();
                    siteGather.showGatherResult("gather-result", result.data)
                }
                else if (result && "error" === status && 1 === result.status && result.msg) {
                    alert(result.msg)
                }
                $("#" + loadingId).hide();
                obj.removeAttr("disabled");
            },
            error: function (xhr, status, error) {
                $("#" + loadingId).hide();
                obj.removeAttr("disabled");
                document.write(xhr.responseText);
                console.log(xhr);
                console.log(status);
                console.log(error);
            },
            complete: function (XMLHttpRequest, status) { //请求完成后最终执行参数
                if ('timeout' === status) {//超时,status还有success,error等值的情况
                    ajaxTimeoutTest.abort();
                    console.log("超时");
                }
            }
        });
        return false;
    });
});