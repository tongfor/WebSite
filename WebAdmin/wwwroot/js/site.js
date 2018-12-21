var zTreeDropdownMenu = (function () {
    //基于zTree的下拉菜单树
    var setting = {
        view: {
            dblClickExpand: false,
            selectedMulti: false,
            showLine: false
        },
        async: {
            enable: true,
            url: "/ArticleClass/GetAllArticleClassTreeJsonForzTree?classid=0",
            type: "post",
            autoParam: ["id=classId"],
            dataFilter: filter
        },
        callback: {
            beforeClick: beforeClick,
            onClick: onClick,
            onAsyncSuccess: expandAll
        }
    };

    var zdmId = "";
    var classId = null;

    function beforeClick(treeId, treeNode) {
        var check = (treeNode && !treeNode.isParent);
        if (!check) alert("只能选择子栏目...");
        return check;
    }

    function onClick(e, treeId, treeNode) {
        var zTree = $.fn.zTree.getZTreeObj(zdmId),
            nodes = zTree.getSelectedNodes(),
            v = "", ids = "";
        nodes.sort(function compare(a, b) { return a.id - b.id; });
        for (var i = 0, l = nodes.length; i < l; i++) {
            v += nodes[i].name + ",";
            ids += nodes[i].id + ",";
        }
        if (v.length > 0) v = v.substring(0, v.length - 1);
        if (ids.length > 0) ids = ids.substring(0, ids.length - 1);
        var cityObj = $("#classSel");
        cityObj.attr("value", v);
        var hideClassObj = $("#ClassId");
        hideClassObj.attr("value", ids);
        hideMenu();
    }

    function showMenu() {
        var cityObj = $("#classSel");
        var cityOffset = $("#classSel").offset();
        $("#zTreeDropdownMenu").css({ left: cityOffset.left + "px", top: cityOffset.top + cityObj.outerHeight() + "px" }).slideDown("fast");

        $("body").bind("mousedown", onBodyDown);
    }

    function hideMenu() {
        $("#zTreeDropdownMenu").fadeOut("fast");
        $("body").unbind("mousedown", onBodyDown);
    }

    function onBodyDown(event) {
        if (!(event.target.id === "menuBtn" || event.target.id === "zTreeDropdownMenu" || $(event.target).parents("#zTreeDropdownMenu").length > 0)) {
            hideMenu();
        }
    }

    function filter(treeId, parentNode, childNodes) {
        if (!childNodes) return null;
        for (var i = 0, l = childNodes.length; i < l; i++) {
            childNodes[i].name = childNodes[i].name.replace(/\.n/g, '.');
        }
        return childNodes;
    }

    function expandAll() {
        var zTree = $.fn.zTree.getZTreeObj(zdmId);
        zTree.expandAll(true);
        classBind();
    }

    function classBind () {
        if (!classId) { return; }
        var zTree = $.fn.zTree.getZTreeObj(zdmId);
        var node = zTree.getNodeByParam("id", classId, null);
        zTree.selectNode(node, false, false);
        onClick(event, classId, node, true);
    }

    return {
        init: function (id) {
            zdmId = id;
            $.fn.zTree.init($("#" + id), setting);
            $(".zTreeSel").click(function () {
                showMenu();
                return false;
            });
        },
        setClassId: function (val) {
            classId = val;
        }
    };
})();