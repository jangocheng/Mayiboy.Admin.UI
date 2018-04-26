$(function () {
    tabClose();
    tabCloseEven();
    writeDateInfo();

    window.OutlookFunctionPanel = {
        //初始化
        Init: function () {
            
        },
        //添加Tab
        AddTab: function(subtitle, url, iconcls) {
            if (!$('#tabs').tabs('exists', subtitle)) {
                $('#tabs').tabs('add', {
                    title: subtitle,
                    content: createFrame(url),
                    closable: true,//显示关闭按钮
                    iconCls: iconcls,//
                    width: $('#mainPanle').width() - 10,
                    height: $('#mainPanle').height() - 26
                });
            } else {
                $('#tabs').tabs('select', subtitle);
            }
            tabClose();
        }
    };

    $("#loginOut").bind("click", function () {
        if (confirm("您确定要退出本次登录吗?")) {
            location.href = 'LoginOut.ashx';
        }
    });
});

//初始化导航栏
function InitLeftMenu(menus) {

    //没有菜单
    if (menus.length === 0) { $("#menus").text("..没有菜单⊙▂⊙..."); return false; }

    $("#menus").text("");//清除提示信息
    var menulists = "";

    $.each(menus, function (i, n) {

        var menulist = "<div id='" + n.MenuId + "' title='" + n.MenuName + "' class='navPanelMini' " + 'data-options="iconCls:\'' + n.MenuClassName + '\'"' + ">";

        if (n.Menus != null) {
            $.each(n.Menus, function (j, o) {
                menulist += "<div  style='clear:both;line-height: 23px;' id='" + o.MenuId + "' data-icon='" + o.MenuClassName + "' data-target='" + o.Target + "' data-url='" + o.Url + "' data-val='" + o.MenuName + "'>" + "<div class='" + o.MenuClassName + "' style=' height: 6px;float: left;margin-top: 3px;margin-left: 20px; width:16px;'></div>" + o.MenuName + "</div>";
            });
        }
        menulist += "</div>";
        menulists += menulist;
    });

    $("#menus").append(menulists);

    //绑定事件
    $(".navPanelMini>div").bind("click", function () {

        var title = $(this).data("val");
        var url = $(this).data("url");
        var target = $(this).data("target");
        var icon = $(this).data("icon");

        //动作类型
        switch (target) {
            case "Iframe":
                addTab(title, url, icon);
                break;
            case "Href":
                window.open(url);
                break;;
            default:
                $.messager.alert("提示", "目标类型不对");
                break;;
        }

        $('.navPanelMini div').removeClass("selected");
        $(this).addClass("selected");
    }).hover(function () {
        $(this).addClass("hover");
    }, function () {
        $(this).removeClass("hover");
    });

    $("#menus").accordion({ border: false, fit: true });
};

function addTab(subtitle, url, iconcls) {
    if (!$('#tabs').tabs('exists', subtitle)) {
        $('#tabs').tabs('add', {
            title: subtitle,
            content: createFrame(url),
            closable: true,//显示关闭按钮
            iconCls: iconcls,//
            width: $('#mainPanle').width() - 10,
            height: $('#mainPanle').height() - 26
        });
    } else {
        $('#tabs').tabs('select', subtitle);
    }
    tabClose();
}

function createFrame(url) {
    var s = '<iframe name="mainFrame" scrolling="auto" frameborder="0"  src="' + url + '" style="width:100%;height:100%;"></iframe>';
    return s;
}

//关闭Tab页签
function tabClose() {

    /*双击关闭TAB选项卡*/
    $(".tabs-inner").dblclick(function () {
        var subtitle = $(this).children(".tabs-closable").text();

        if (subtitle != "主页")
            $('#tabs').tabs('close', subtitle);
    });

    /*为选项卡绑定右键*/
    $(".tabs-inner").bind('contextmenu', function (e) {
        $('#mm').menu('show', {
            left: e.pageX,
            top: e.pageY
        });

        var subtitle = $(this).children(".tabs-closable").text();

        $('#mm').data("currtab", subtitle);
        $('#tabs').tabs('select', subtitle);
        return false;
    });
}

//绑定右键菜单事件
function tabCloseEven() {

    //刷新
    $('#mm-tabupdate').click(function () {
        var currTab = $('#tabs').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');

        if (url === undefined || url === "undefined")
            url = currTab.panel('options').url;

        $('#tabs').tabs('update', {
            tab: currTab,
            options: {
                content: createFrame(url)
            }
        });
    });

    //关闭当前
    $('#mm-tabclose').click(function () {
        var currtabTitle = $('#mm').data("currtab");
        $('#tabs').tabs('close', currtabTitle);
    });

    //全部关闭
    $('#mm-tabcloseall').click(function () {
        $('.tabs-inner span').each(function (i, n) {
            var t = $(n).text();
            if (t !== "主页")
                $('#tabs').tabs('close', t);
        });
    });

    //关闭除当前之外的TAB
    $('#mm-tabcloseother').click(function () {
        $('#mm-tabcloseright').click();
        $('#mm-tabcloseleft').click();
    });

    //关闭当前右侧的TAB
    $('#mm-tabcloseright').click(function () {
        var nextall = $('.tabs-selected').nextAll();
        if (nextall.length == 0) {
            //msgShow('系统提示','后边没有啦~~','error');
            //alert('后边没有啦~~');
            return false;
        };
        nextall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            if (t !== "主页")
                $('#tabs').tabs('close', t);
        });
        return false;
    });

    //关闭当前左侧的TAB
    $('#mm-tabcloseleft').click(function () {
        var prevall = $('.tabs-selected').prevAll();
        if (prevall.length == 0) {
            //alert('到头了，前边没有啦~~');
            return false;
        }
        prevall.each(function (i, n) {
            var t = $('a:eq(0) span', $(n)).text();
            if (t !== "主页")
                $('#tabs').tabs('close', t);
        });
        return false;
    });

    //退出
    $("#mm-exit").click(function () {
        $('#mm').menu('hide');
    });
}

//当前日期
function writeDateInfo() {
    var myweekday = "";
    var year = "";
    var mydate = new Date();
    myweekday = mydate.getDay();
    var mymonth = mydate.getMonth() + 1;
    var myday = mydate.getDate();
    var myyear = mydate.getYear();
    year = (myyear > 200) ? myyear : 1900 + myyear;
    var weekday;
    if (myweekday === 0)
        weekday = " 星期日";
    else if (myweekday === 1)
        weekday = " 星期一";
    else if (myweekday === 2)
        weekday = " 星期二";
    else if (myweekday === 3)
        weekday = " 星期三";
    else if (myweekday === 4)
        weekday = " 星期四";
    else if (myweekday === 5)
        weekday = " 星期五";
    else if (myweekday === 6)
        weekday = " 星期六";
    $("#datetime").text(year + "年" + mymonth + "月" + myday + "日 " + weekday);
}

