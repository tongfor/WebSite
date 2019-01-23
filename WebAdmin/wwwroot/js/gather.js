//互联网信息采集对象
; var gatherMenuFunc = (function () {
    var _gatherSiteMenu = [
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
        }
    ];
    var menuObjs = $("#gather-list>ul")

    //菜单初始化
    var _gatherMenuInit = function () {
        menuObjs.empty()
        for (var p in gatherMenu) {
            var li = '<li class="floatleft">\n'
            li += '<li><a href="' + gatherMenu[p].menuLink + '">' + gatherMenu[p].menuTitle + '</a></li>'
            menuObjs.append(li)
        }
    }

    return {
        gatherMenuInit: _gatherMenuInit,
        gatherSiteMenu: _gatherSiteMenu
    }
}())

$(function () {
    gatherMenuFunc.gatherMenuInit()
});