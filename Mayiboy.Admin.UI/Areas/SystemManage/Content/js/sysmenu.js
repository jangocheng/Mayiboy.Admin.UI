$(function () {
    var editsysmenunum;
    var editmenupnum;
    var sysmenudata;
    var selelctdefault = 0;
    var form;
    var thisPage = {
        Buttons: {
            menuptable:null
        },
        Init: function () {
            thisPage.InitSysMenu();
            $("#sysnavbar").change(function () {
                thisPage.InitSysMenu();
            });

            $("#btnadd").click(function () {
                var row = $("#treetable").treegrid("getSelected");
                if (row != null) {
                    selelctdefault = row.Id;
                }
                thisPage.ShowEditPage();
            });

            $("#btnaddp").click(function () {
                thisPage.ShowEditPermissionsPage();
            });

            $("#refreshcode").click(function () {
                $.get($("#refreshcode").data("url"), function (res) {
                    if (res.status == 0) {
                        $("#txtcode").val(res.code);
                    } else {
                        layer.msg(res.msg);
                    }
                });
            });

            //Layui坑
            layui.use(["table", "form"], function () {
                form = layui.form;
            });
        },
        InitSysMenu: function () {
            $("#treetable").treegrid({
                url: $("#treetable").data("url"),
                method: "get",
                queryParams: {
                    id: $("#sysnavbar").val()
                },
                rownumbers: true, //如果为true，则显示一个行号列。
                idField: 'Id', //定义标识树节点的键名字段
                treeField: 'Name', //定义树节点的字段
                //lines:true,
                //fitColumns: true, //设置为 true，则会自动扩大或缩小列的尺寸以适应网格的宽度并且防止水平滚动。
                columns: [
                    [
                        { field: 'Name', title: '菜单名称', width: 180 },
                        { field: 'UrlAddress', title: '地址', width: 300 },
                        { field: 'Sort', title: '排序', width: 40, align: 'center' },
                        {
                            field: 'CreateTime',
                            title: "创建时间",
                            width: 120,
                            formatter: function (v, r) {
                                return $.Extend_formatDateTime(v);;
                            }
                        },
                        {
                            field: 'UpdateTime',
                            title: "修改时间",
                            width: 120,
                            formatter: function (v, r) {
                                return $.Extend_formatDateTime(v);;
                            }
                        },
                        { field: 'Remark', title: '备注' },
                        {
                            field: 'oper',
                            title: '操作',
                            width: 200,
                            align: 'center',
                            formatter: function (v, r) {
                                var html = "";

                                html += '<a href="javascript:;" data-id="' + r.Id + '" class="layui-btn layui-btn-xs editmenu">编辑菜单</a>';
                                html += '<a href="javascript:;" data-id="' + r.Id + '" class="layui-btn layui-btn-xs layui-btn-normal editp">配置操作</a>';
                                html += '<a href="javascript:;" data-id="' + r.Id + '" class="layui-btn layui-btn-danger layui-btn-xs delmenu" lay-event="del">删除</a>';
                                return html;
                            }

                        }
                    ]
                ],
                onLoadSuccess: function (v, data) {
                    sysmenudata = data.rows;

                    //编辑菜单
                    $(".editmenu").click(function () {
                        var row = thisPage.GetSysMenuById($(this).data("id"));
                        thisPage.ShowEditPage(row);
                    });

                    //配置权限
                    $(".editp").click(function () {
                        var row = thisPage.GetSysMenuById($(this).data("id"));
                        thisPage.ShowPermissionsPage(row);
                    });

                    //删除菜单
                    $(".delmenu").click(function () {
                        var id = $(this).data("id");
                        layer.confirm('真的删除行吗？', function (index) {
                            thisPage.DelSysMenu(id, index);
                        });
                    });
                }
            });
        },
        ShowPermissionsPage: function (row) {
            thisPage.InitMenuPermissions(row.Id);
            layer.open({
                title: "配置权限",
                resize: false,
                type: 1,
                area: ['950px', '520px'],
                offset: '20px',
                content: $("#showmenup")
            });
        },
        //初始化菜单权限列表
        InitMenuPermissions: function (id) {
          var menuptable=  layui.table;
          thisPage.Buttons.menuptable = menuptable.render({
                elem: '#tbtablep',
                url: $("#tbtablep").data("url"),
                where: {
                    id: id
                },
                page: false,
                even: true,
                cols: [[
                    { field: 'Name', title: '名称', width: 150 },
                    { field: 'Action', title: '动作路径', width: 200 },
                    { field: 'Code', title: '代码', width: 120 },
                    { field: 'Remark', title: '备注', width: 294 },
                    {
                        field: 'oper', title: "操作", width: 163, templet: function (row) {
                            var html = '';
                            html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="editp">编辑</a>';
                            html += '<a href="javascript:;" class="layui-btn layui-btn-danger layui-btn-xs" lay-event="delp">删除</a>';
                            return html;
                        }
                    }
                ]],
                done: function(res,curr,count) {
                    
                }
            });

             //监听工具条
             menuptable.on('tool(table-menup)', function (obj) {
                var data = obj.data; //获得当前行数据
                var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                var tr = obj.tr; //获得当前行 tr 的DOM对象

                switch (obj.event) {
                    case "editp":
                        thisPage.ShowEditPermissionsPage(data);
                        break;
                    case "delp":
                        layer.confirm('真的删除行吗？', function (index) {
                            thisPage.DelMenuP(data, index);
                        });
                        break;
                 default:
                 }
            });
        },
        DelMenuP: function(row,index) {
            $.ajax({
                type: "get",
                url: $("#tbtablep").data("delurl"),
                data: { id: row.Id },
                success:function(res) {
                    if (res.status == 0) {
                        thisPage.Buttons.menuptable.reload();
                        layer.close(index);
                    } else {
                        layer.msg(res.msg);
                    }
                }
            });
        },
        ShowEditPermissionsPage: function (data) {
            var title = "";

            if (data == null) {
                title = "添加菜单权限";
                $("#txtpid").val("0");
                $("#txtnamep").val("");
                $("#txtaction").val("");
                $("#txtcode").val("");
                $("#txtremarkp").val("");
            } else {
                title = "编辑菜单权限";
                $("#txtpid").val(data.Id);
                $("#txtnamep").val(data.Name);
                $("#txtaction").val(data.Action);
                $("#txtcode").val(data.Code);
                $("#txtremarkp").val(data.Remark);
            }

            editmenupnum= layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['600px', '420px'],
                offset: '20px',
                content: $("#editp"),
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveMenuP();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });
        },
        SaveMenuP: function () {
            var isb = layer.IsValidation("#editp");

            var row = $("#treetable").treegrid("getSelected");
            if (row != null) {
                selelctdefault = row.Id;
            } else {
                layer.msg("菜单选择有误");
                return false;
            }

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editp").data("url"),
                    data: {
                        Id: $("#txtpid").val(),
                        MenuId: row.Id,
                        Name: $("#txtnamep").val(),
                        Action: $("#txtaction").val(),
                        Code: $("#txtcode").val(),
                        Remark: $("#txtremarkp").val()
                    },
                    success:function(res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Buttons.menuptable.reload();
                            layer.close(editmenupnum);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        InitSelectSysMenu: function () {
            $.ajax({
                url: $("#treetable").data("url"),
                data: { id: $("#sysnavbar").val() },
                success: function (res) {
                    if (res.status == 0) {

                        res.rows.push({ Id: 0, Pid: -1, Name: "根基菜单" });

                        var treelist = thisPage.ConvertListToTree(res.rows, -1);

                        $("#selectsysmenu").combotree('loadData', treelist);

                    } else {
                        console.log(res.msg);
                    }
                }
            });
        },
        ConvertListToTree: function (list, parentId) {

            var itemArr = [];

            for (var i = 0; i < list.length; i++) {
                var node = list[i];
                if (node.Pid == parentId) {
                    var newNode = {
                        id: node.Id,
                        text: node.Name,
                        parentId: node.Pid,
                        children: thisPage.ConvertListToTree(list, node.Id)
                    };
                    itemArr.push(newNode);
                }
            }
            return itemArr;
        },
        GetSysMenuById: function (id) {
            for (var item in sysmenudata) {
                if (sysmenudata[item].Id == id) {
                    return sysmenudata[item];
                }
            }
            return null;
        },
        ShowEditPage: function (data) {
            var title = "";
            thisPage.InitSelectSysMenu();

            if (data == null) {
                title = "新增系统菜单";
                $("#txtid").val("0");
                $('#selectsysmenu').combotree('setValue', selelctdefault);
                $("#txtname").val("");
                $("#txturl").val("");
                $("#selectmenutype").val("0");
                $("#txtsort").val("0");
                $("#txtremark").val("");
            } else {
                title = "修改系统菜单";
                $("#txtid").val(data.Id);
                $('#selectsysmenu').combotree('setValue', data.Pid);
                $("#txtname").val(data.Name);
                $("#txturl").val(data.UrlAddress);
                $("#selectmenutype").val(data.MenuType);
                $("#txtsort").val(data.Sort);
                $("#txtremark").val(data.Remark);
            }

            form.render('select');

            editsysmenunum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '520px'],
                offset: '20px',
                content: $('#editsysmenu'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveSysMenu();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });

        },
        SaveSysMenu: function () {

            //验证表单
            var isb = layer.IsValidation("#editsysmenu");
            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editsysmenu").data("url"),
                    data: {
                        Id: $("#txtid").val(),
                        Pid: $('#selectsysmenu').combotree('getValue'),
                        Name: $("#txtname").val(),
                        UrlAddress: $("#txturl").val(),
                        NavbarId: $("#sysnavbar").val(),
                        MenuType: $("#selectmenutype").val(),
                        Sort: $("#txtsort").val(),
                        Remark: $("#txtremark").val()
                    },
                    success: function (res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            layer.close(editsysmenunum);
                            thisPage.InitSysMenu();

                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        DelSysMenu: function (id, index) {
            $.ajax({
                type: "get",
                url: $("#treetable").data("delurl"),
                data: { id: id },
                success: function (res) {
                    if (res.status == 0) {
                        layer.close(index);
                        $("#treetable").treegrid('remove', id); //移除当前行
                        //thisPage.InitSysMenu();
                    } else {
                        layer.alert(res.msg);
                    }

                }
            });
        }
    }

    thisPage.Init();
})