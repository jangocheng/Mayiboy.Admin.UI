(function ($) {
    $.maintab = {
        requestFullScreen: function () {
            var de = document.documentElement;
            if (de.requestFullscreen) {
                de.requestFullscreen();
            } else if (de.mozRequestFullScreen) {
                de.mozRequestFullScreen();
            } else if (de.webkitRequestFullScreen) {
                de.webkitRequestFullScreen();
            }
        },
        exitFullscreen: function () {
            var de = document;
            if (de.exitFullscreen) {
                de.exitFullscreen();
            } else if (de.mozCancelFullScreen) {
                de.mozCancelFullScreen();
            } else if (de.webkitCancelFullScreen) {
                de.webkitCancelFullScreen();
            }
        },
        refreshTab: function () {
            var currentId = $('.page-tabs-content').find('.active').attr('data-id');
            var target = $('.LRADMS_iframe[data-id="' + currentId + '"]');
            var url = target.attr('src');
            //$.loading(true);
            target.attr('src', url).load(function () {
                //$.loading(false);
            });
        },
        activeTab: function () {
            var currentId = $(this).data('id');
            if (!$(this).hasClass('active')) {
                $('.mainContent .LRADMS_iframe').each(function () {
                    if ($(this).data('id') == currentId) {
                        $(this).show().siblings('.LRADMS_iframe').hide();
                        return false;
                    }
                });
                $(this).addClass('active').siblings('.menuTab').removeClass('active');
                $.maintab.scrollToTab(this);
            }
        },
        closeOtherTabs: function () {
            $('.page-tabs-content').children("[data-id]").find('.fa-remove').parents('a').not(".active").each(function () {
                $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').remove();
                $(this).remove();
            });
            $('.page-tabs-content').css("margin-left", "0");
        },
        closeTab: function () {
            var closeTabId = $(this).parents('.menuTab').data('id');
            var currentWidth = $(this).parents('.menuTab').width();
            if ($(this).parents('.menuTab').hasClass('active')) {
                if ($(this).parents('.menuTab').next('.menuTab').size()) {
                    var activeId = $(this).parents('.menuTab').next('.menuTab:eq(0)').data('id');
                    $(this).parents('.menuTab').next('.menuTab:eq(0)').addClass('active');

                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == activeId) {
                            $(this).show().siblings('.LRADMS_iframe').hide();
                            return false;
                        }
                    });
                    var marginLeftVal = parseInt($('.page-tabs-content').css('margin-left'));
                    if (marginLeftVal < 0) {
                        $('.page-tabs-content').animate({
                            marginLeft: (marginLeftVal + currentWidth) + 'px'
                        }, "fast");
                    }
                    $(this).parents('.menuTab').remove();
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
                if ($(this).parents('.menuTab').prev('.menuTab').size()) {
                    var activeId = $(this).parents('.menuTab').prev('.menuTab:last').data('id');
                    $(this).parents('.menuTab').prev('.menuTab:last').addClass('active');
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == activeId) {
                            $(this).show().siblings('.LRADMS_iframe').hide();
                            return false;
                        }
                    });
                    $(this).parents('.menuTab').remove();
                    $('.mainContent .LRADMS_iframe').each(function () {
                        if ($(this).data('id') == closeTabId) {
                            $(this).remove();
                            return false;
                        }
                    });
                }
            }
            else {
                $(this).parents('.menuTab').remove();
                $('.mainContent .LRADMS_iframe').each(function () {
                    if ($(this).data('id') == closeTabId) {
                        $(this).remove();
                        return false;
                    }
                });
                $.maintab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        addTab: function () {
            //$(".navbar-custom-menu>ul>li.open").removeClass("open");
            var dataId = $(this).attr('data-id');
            if (dataId != "") {
                //top.$.cookie('nfine_currentmoduleid', dataId, { path: "/" });
            }
            var dataUrl = $(this).data('href');
            var menutype = $(this).data('menutype');
            var menuName = $.trim($(this).text());
            var flag = true;
            if (dataUrl == undefined || $.trim(dataUrl).length == 0) {
                return false;
            }

            //判断菜单类型
            if (menutype == 1) {
                window.open(dataUrl);
                return false;
            }


            $('.menuTab').each(function () {
                if ($(this).data('id') == dataUrl) {
                    if (!$(this).hasClass('active')) {
                        $(this).addClass('active').siblings('.menuTab').removeClass('active');
                        $.maintab.scrollToTab(this);
                        $('.mainContent .LRADMS_iframe').each(function () {
                            if ($(this).data('id') == dataUrl) {
                                $(this).show().siblings('.LRADMS_iframe').hide();
                                return false;
                            }
                        });
                    }
                    flag = false;
                    return false;
                }
            });
            if (flag) {
                var str = '<a href="javascript:;" class="active menuTab" data-id="' + dataUrl + '">' + menuName + ' <i class="fa fa-remove"></i></a>';
                $('.menuTab').removeClass('active');
                var str1 = '<iframe class="LRADMS_iframe" id="iframe' + dataId + '" name="iframe' + dataId + '"  width="100%" height="100%" src="' + dataUrl + '" frameborder="0" data-id="' + dataUrl + '" seamless></iframe>';
                $('.mainContent').find('iframe.LRADMS_iframe').hide();
                $('.mainContent').append(str1);
                //$.loading(true);
                $('.mainContent iframe:visible').load(function () {
                    //$.loading(false);
                });
                $('.menuTabs .page-tabs-content').append(str);
                $.maintab.scrollToTab($('.menuTab.active'));
            }
            return false;
        },
        scrollTabRight: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.maintab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                scrollVal = $.maintab.calSumWidth($(tabElement).prevAll());
                if (scrollVal > 0) {
                    $('.page-tabs-content').animate({
                        marginLeft: 0 - scrollVal + 'px'
                    }, "fast");
                }
            }
        },
        scrollTabLeft: function () {
            var marginLeftVal = Math.abs(parseInt($('.page-tabs-content').css('margin-left')));
            var tabOuterWidth = $.maintab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").width() < visibleWidth) {
                return false;
            } else {
                var tabElement = $(".menuTab:first");
                var offsetVal = 0;
                while ((offsetVal + $(tabElement).outerWidth(true)) <= marginLeftVal) {
                    offsetVal += $(tabElement).outerWidth(true);
                    tabElement = $(tabElement).next();
                }
                offsetVal = 0;
                if ($.maintab.calSumWidth($(tabElement).prevAll()) > visibleWidth) {
                    while ((offsetVal + $(tabElement).outerWidth(true)) < (visibleWidth) && tabElement.length > 0) {
                        offsetVal += $(tabElement).outerWidth(true);
                        tabElement = $(tabElement).prev();
                    }
                    scrollVal = $.maintab.calSumWidth($(tabElement).prevAll());
                }
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        scrollToTab: function (element) {
            var marginLeftVal = $.maintab.calSumWidth($(element).prevAll());
            var marginRightVal = $.maintab.calSumWidth($(element).nextAll());
            var tabOuterWidth = $.maintab.calSumWidth($(".content-tabs").children().not(".menuTabs"));
            var visibleWidth = $(".content-tabs").outerWidth(true) - tabOuterWidth;
            var scrollVal = 0;
            if ($(".page-tabs-content").outerWidth() < visibleWidth) {
                scrollVal = 0;
            } else if (marginRightVal <= (visibleWidth - $(element).outerWidth(true) - $(element).next().outerWidth(true))) {
                if ((visibleWidth - $(element).next().outerWidth(true)) > marginRightVal) {
                    scrollVal = marginLeftVal;
                    var tabElement = element;
                    while ((scrollVal - $(tabElement).outerWidth()) > ($(".page-tabs-content").outerWidth() - visibleWidth)) {
                        scrollVal -= $(tabElement).prev().outerWidth();
                        tabElement = $(tabElement).prev();
                    }
                }
            } else if (marginLeftVal > (visibleWidth - $(element).outerWidth(true) - $(element).prev().outerWidth(true))) {
                scrollVal = marginLeftVal - $(element).prev().outerWidth(true);
            }
            $('.page-tabs-content').animate({
                marginLeft: 0 - scrollVal + 'px'
            }, "fast");
        },
        calSumWidth: function (element) {
            var width = 0;
            $(element).each(function () {
                width += $(this).outerWidth(true);
            });
            return width;
        },
        contextmenuTab: function () {
            $("#yjmenu").show().offset({ left: $(this).offset().left });
            return false;
        },
        init: function () {
            $('.menuTabs').on('click', '.menuTab i', $.maintab.closeTab);
            $('.menuTabs').on('click', '.menuTab', $.maintab.activeTab);
            $('.menuTabs').on('contextmenu', '.active', $.maintab.contextmenuTab);
            $('.tabLeft').on('click', $.maintab.scrollTabLeft);
            $('.tabRight').on('click', $.maintab.scrollTabRight);
            $('.tabReload').on('click', $.maintab.refreshTab);
            $(".content-tabs,#yjmenu,.main-header,.main-sidebar").on('click', function () {
                $("#yjmenu").hide();
            });
            $('.tabCloseCurrent').on('click', function () {
                $('.page-tabs-content').find('.active i').trigger("click");
            });
            $('.tabCloseAll').on('click', function () {
                $('.page-tabs-content').children("[data-id]").find('.fa-remove').each(function () {
                    $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').remove();
                    $(this).parents('a').remove();
                });
                $('.page-tabs-content').children("[data-id]:first").each(function () {
                    $('.LRADMS_iframe[data-id="' + $(this).data('id') + '"]').show();
                    $(this).addClass("active");
                });
                $('.page-tabs-content').css("margin-left", "0");
            });
            $('.tabCloseOther').on('click', $.maintab.closeOtherTabs);
            $('.fullscreen').on('click', function () {
                if (!$(this).attr('fullscreen')) {
                    $(this).attr('fullscreen', 'true');
                    $.maintab.requestFullScreen();
                } else {
                    $(this).removeAttr('fullscreen');
                    $.maintab.exitFullscreen();
                }
            });
            $("#changepassword").on('click', function () {

                $('#txtoldpwd,#txtnewpwd,#txtconfirmpwd').val("");

               var opennum= layer.open({
                    title: "修改密码",
                    resize: false,
                    type: 1,
                    area: ['400px', '300px'],
                    offset: '100px',
                    content: $('#changepasswordpage'),
                    btn: ["保存", "取消"],
                    btn1: function () {

                        var isb = layer.IsValidation("#changepasswordpage");

                        //验证两次输入是否相同
                        if ($("#txtnewpwd").val() != $("#txtconfirmpwd").val()) {
                            layer.msg("两次密码输入不同", { icon: 5, shift: 6 });
                            return false;
                        }

                        if (isb == null) {
                            $.ajax({
                                type: 'get',
                                url: $("#changepasswordpage").data("url"),
                                data: {
                                    oldpwd: $('#txtoldpwd').val(),
                                    newpwd: $("#txtnewpwd").val()
                                },
                                success: function (res) {
                                    if (res.status == 0) {
                                        layer.msg("修改成功");
                                        layer.close(opennum);
                                    } else {
                                        layer.msg(res.msg);
                                    }
                                }
                            });
                        }
                    },
                    btn2: function () {
                        // alert("取消");
                    },
                    cancel: function () {
                        // alert("退出");
                    }
                });
            });

            $("#userinfo").on("click", $.maintab.addTab);
        }
    };

    $.mainindex = {
        Init: function () {
            $("#sysnavbar li").click(function () {
                if ($(this).data("url").length > 0) {
                    location.href = $(this).data("url");
                    return false;
                }

                $("#sysnavbar li").removeClass("selectnav");
                $(this).addClass("selectnav");
                $.mainindex.loadMenu();
            });

            $.mainindex.loadMenu();

            //检查用户登录状态
            $.mainindex.EventUserLoginStatus();
        },
        load: function () {
            $("body").removeClass("hold-transition");
            $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
            $(window).resize(function (e) {
                $("#content-wrapper").find('.mainContent').height($(window).height() - 100);
            });
            $(".sidebar-toggle").click(function () {
                if (!$("body").hasClass("sidebar-collapse")) {
                    $("body").addClass("sidebar-collapse");
                } else {
                    $("body").removeClass("sidebar-collapse");
                }
            });
            $(window).load(function () {
                window.setTimeout(function () {
                    $('#ajax-loader').fadeOut();
                }, 300);
            });
        },
        jsonWhere: function (data, action) {
            if (action == null) return false;
            var reval = new Array();
            $(data).each(function (i, v) {
                if (action(v)) {
                    reval.push(v);
                }
            });
            return reval;
        },
        loadMenu: function () {
            //加载默认菜单
            if ($("#sysnavbar .selectnav").size() > 0) {
                $.ajax({
                    type: "get",
                    url: $("#sysnavbar").data("sysmenu"),
                    data: { id: $("#sysnavbar .selectnav").data("navid") },
                    success: function (res) {
                        if (res.status == 0) {
                            var data = res.data;
                            var html = $.mainindex.getMenuHtml1(data);
                            $(".sidebar-menu .treeview").remove();
                            $("#sidebar-menu").append(html);
                            $('.menuItem').on('click', $.maintab.addTab);
                            $.mainindex.bandmenu();
                        } else {
                            alert(res.msg);
                        }
                    }
                });
            }

            return false;
        },
        bandmenu: function() {
            $("#sidebar-menu li a").click(function () {
                var d = $(this);
                var e = d.next();

                if (e.is(".treeview-menu") && e.is(":visible")) {
                    e.slideUp(500, function () {
                        e.removeClass("menu-open");
                    }), e.parent("li").removeClass("active");
                } else if (e.is(".treeview-menu") && !e.is(":visible")) {
                    var f = d.parents("ul").first();
                    var g = f.find("ul:visible").slideUp(500);
                    g.removeClass("menu-open");
                    var h = d.parent("li");
                    e.slideDown(500, function () {
                        e.addClass("menu-open");
                        f.find("li.active").removeClass("active");
                        h.addClass("active");

                        var height1 = $(window).height() - $("#sidebar-menu >li.active").position().top - 41;
                        var height2 = $("#sidebar-menu li > ul.menu-open").height() + 10;
                        if (height2 > height1) {
                            $("#sidebar-menu >li > ul.menu-open").css({
                                overflow: "auto",
                                height: height1
                            });
                        }
                    });
                }
                e.is(".treeview-menu");
            });
        },
        getMenuHtml1: function (data) {
            var html = '';
            if (data == null || data == NaN) return html;

            $.each(data, function (i, row) {

                html += '<li class="treeview">';
                if (row.UrlAddress == null || row.UrlAddress.length == 0 || row.ChildNodes != null) {
                    html += '<a href="javascript:;">';
                } else {
                    html += '<a class="menuItem"  data-href="' + row.UrlAddress + '" href="javascript:;">';
                }

                html += '<i class="' + row.Icon + '"></i><span>' + row.Name + '</span>';
                if (row.ChildNodes != null && row.ChildNodes.length > 0) {
                    html += '<i class="fa fa-angle-left pull-right"></i>';
                }

                html += '</a>';
                if (row.ChildNodes != null) {
                    html += '<ul class="treeview-menu" style="display: none;">';
                    html += $.mainindex.getMenuHtml2(row.ChildNodes);
                    html += '</ul>';
                }

                html += '</li>';
            });
            html += '';
            return html;
        },
        getMenuHtml2: function (data) {
            var html = "";
            if (data == null || data == NaN) return html;

            $.each(data, function (i, row) {

                html += '<li class="treeview">';
                if (row.UrlAddress == null || row.UrlAddress.length == 0 || row.ChildNodes != null) {
                    html += '<a href="javascript:;">';
                } else {
                    html += '<a class="menuItem" data-id="' + row.Id + '" data-menutype="' + row.MenuType + '" data-href="' + row.UrlAddress + '" href="javascript:;">';
                }

                html += '<i class="' + row.Icon + '"></i><span>' + row.Name + '</span>';
                if (row.ChildNodes != null && row.ChildNodes.length > 0) {
                    html += '<i class="fa fa-angle-left pull-right"></i>';
                }

                html += '</a>';
                if (row.ChildNodes != null) {
                    html += '<ul class="treeview-menu" style="display: none;">';
                    html += $.mainindex.getMenuHtml3(row.ChildNodes);
                    html += '</ul>';
                }

                html += '</li>';
            });
            return html;
        },
        getMenuHtml3: function (data) {
            var html = "";
            if (data == null || data == NaN) return html;

            $.each(data, function (i, row) {

                html += '<li class="treeview">';
                if (row.UrlAddress == null || row.UrlAddress.length == 0 || row.ChildNodes != null) {
                    html += '<a href="javascript:;">';
                } else {
                    html += '<a class="menuItem" data-id="' + row.Id + '" data-href="' + row.UrlAddress + '" href="javascript:;">';
                }

                html += '<i class="' + row.Icon + '"></i><span>' + row.Name + '</span>';
                if (row.ChildNodes != null && row.ChildNodes.length > 0) {
                    html += '<i class="fa fa-angle-left pull-right"></i>';
                }

                html += '</a>';
                if (row.ChildNodes != null) {
                    html += '<ul class="treeview-menu" style="display: none;">';
                    html += $.mainindex.getMenuHtml4(row.ChildNodes);
                    html += '</ul>';
                }

                html += '</li>';
            });
            return html;
        },
        getMenuHtml4: function (data) {
            var html = "";
            if (data == null || data == NaN) return html;

            $.each(data, function (i, row) {

                html += '<li class="treeview">';
                if (row.UrlAddress == null || row.UrlAddress.length == 0 || row.ChildNodes != null) {
                    html += '<a href="javascript:;">';
                } else {
                    html += '<a class="menuItem" data-id="' + row.Id + '" data-href="' + row.UrlAddress + '" href="javascript:;">';
                }

                html += '<i class="' + row.Icon + '"></i><span>' + row.Name + '</span>';
                if (row.ChildNodes != null && row.ChildNodes.length > 0) {
                    html += '<i class="fa fa-angle-left pull-right"></i>';
                }

                html += '</a>';
                if (row.ChildNodes != null) {
                    html += '<ul class="treeview-menu" style="display: none;">';
                    html += '<li>太深了</li>';
                    html += '</ul>';
                }

                html += '</li>';
            });
            return html;
        },
        EventUserLoginStatus: function() {
            setInterval(function () {
                $.get($("#wrapper").data("loginstatus"), function (res) {
                    if (res.status != 0) {
                        location.reload();
                    }
                });
            }, 1000 * 60 * 5);
        }
    };

    $(function () {
        layui.use(["layer", "myvalidation"], function () { });

        $.mainindex.Init();
        $.mainindex.load();
        $.maintab.init();
    });
})(jQuery);