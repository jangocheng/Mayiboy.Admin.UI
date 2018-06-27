$(function () {
    var edituserrolenum;
    var editrolepnum;
    var selectrole;
    var thisPage = {
        Buttons: {
            UserRoleTable: null,
            RolePermissionsTable:null
        },
        Init: function () {
            thisPage.InitUserRoleTable();

            $("#btnadd").click(function () {
                thisPage.ShowEditPage();
            });

            $("#btnquery").click(function () {
                thisPage.QueryUserRole();
            });

            $("#sysnavbar").change(function () {
                thisPage.InitSysMenu();
            });

        },
        InitUserRoleTable: function () {
            layui.use("table", function () {
                var table = layui.table;

                thisPage.Buttons.UserRoleTable = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    page: true, //开启分页
                    limit:20,
                    even: true, //各行变色
                    cols: [
                        [
                            { field: 'id', title: '序号', type: "numbers", width: 60 },
                            { field: 'Name', title: '角色名', width: 150 },
                            {
                                field: 'CreateTime',
                                title: '新增时间',
                                width: 180,
                                templet: function (row) {
                                    return $.Extend_formatDateTime(row.CreateTime);
                                }
                            },
                            {
                                field: 'UpdateTime',
                                title: '更新时间',
                                width: 180,
                                templet: function (row) {
                                    return $.Extend_formatDateTime(row.CreateTime);
                                }
                            },
                            { field: 'Remark', title: "备注", width: 250 },
                            {
                                field: 'look',
                                fixed: 'right',
                                title: '操作',
                                width: 200,
                                templet: function (row) {
                                    var html = "";
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>';
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="editp">配置权限</a>';
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>';
                                    return html;
                                }
                            }
                        ]
                    ],
                    done: function (res, curr, count) {

                    }
                });

                //监听工具条
                table.on('tool(table-operate)', function (obj) {
                    var data = obj.data; //获得当前行数据
                    var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                    var tr = obj.tr; //获得当前行 tr 的DOM对象

                    switch (obj.event) {
                        case "edit":
                            thisPage.ShowEditPage(data);
                            break;
                        case "editp":
                            selectrole = data;
                            thisPage.ShowSetRolePage();
                            break;
                        case "del":
                            layer.confirm('真的删除行吗？', function (index) {
                                thisPage.Del(data, index);
                            });
                            break;
                        default:
                    }
                });
            });
        },
        InitSysMenu: function () {

            $("#treetable").treegrid({
                method: "get",
                url: $("#treetable").data("url"),
                queryParams: {
                    id: $("#sysnavbar").val()
                },
                rownumbers: true, //如果为true，则显示一个行号列。
                idField: 'Id', //定义标识树节点的键名字段
                treeField: 'Name', //定义树节点的字段
                width:380,
                columns: [[
                    { field: 'Name', title: "名称", width: 160 },
                    { field: 'Remark', title: "备注",width:170}
                ]],
                onClickRow: function (row) {
                    thisPage.InitMenuOper(row.Id);
                },
                onLoadSuccess:function() {
                    thisPage.InitMenuOper(-1);
                }
            });
        },
        ShowSetRolePage: function () {

            //初始化权限树结构
            thisPage.InitSysMenu();
            layer.open({
                title: "配置角色权限",
                resize: false,
                type: 1,
                area: ['700px', '500px'],
                offset: '20px',
                content: $('#setrolep'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存"],
                btn1: function () {
                    thisPage.SaveRolePermissions();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });

            //alert("配置角色权限");
        },
        InitMenuOper: function(id) {
            var table = layui.table;
            var selectdata = layui.table.checkStatus("tableaction").data;
             var row=  $('#treetable').datagrid('getSelected');

            thisPage.Buttons.RolePermissionsTable = table.render({
                elem: '#tableaction',
                url: $("#tableaction").data("url"),
                where: {
                    id: id,
                    navbarid: $("#sysnavbar").val(),
                    menuid: row.Id,
                    roleid: selectrole.Id
                },
                page: false,
                even: true,
                height:350,
                cols: [[
                    { type: 'checkbox' },
                    { field: 'Name', title: "操作名称",width:120 },
                    { field: 'Remark', title: "备注"}
                ]],
                done: function (res, curr, count) {
                    var a = res;
                }
            });
        },
        QueryUserRole: function () {
            thisPage.Buttons.UserRoleTable.reload({
                where: {
                    name: $("#name").val()
                },
                page: {
                    curr: 1 //重新从第 1 页开始
                }
            });
        },
        ShowEditPage: function (data) {
            var title = "";
            if (data == null) {
                title = "新增角色";
                $("#txtid").val("0");
                $("#txtname").val("");
                $("#txtremark").val("");

            } else {
                title = "修改角色";
                $("#txtid").val(data.Id);
                $("#txtname").val(data.Name);
                $("#txtremark").val(data.Remark);
            }

            edituserrolenum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '320px'],
                offset: '20px',
                content: $('#edituserrole'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveUserRole();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });
        },
        SaveUserRole: function () {
            //验证表单
            var isb = layer.IsValidation("#edituserinfo");

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#edituserrole").data("url"),
                    data: {
                        id: $("#txtid").val(),
                        name: $("#txtname").val(),
                        remark: $("#txtremark").val()
                    },
                    success: function (res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Buttons.UserRoleTable.reload();
                            layer.close(edituserrolenum);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        SaveRolePermissions: function() {

            var selectdata = layui.table.checkStatus("tableaction").data;

            var row=  $('#treetable').datagrid('getSelected');

            var permissionsids = [];
            $.each(selectdata, function(i,v) {
                permissionsids.push(v.Id);
            });

            $.ajax({
                type: "post",
                url: $("#setrolep").data("url"),
                data: {
                    nvabarid: $("#sysnavbar").val(),
                    menuid: row.Id,
                    roleid: selectrole.Id,
                    pid: permissionsids
                },
                success:function(res) {
                    if (res.status == 0) {

                        layer.msg("保存成功");
                    } else {
                        layer.msg(res.msg);
                    }
                }
            });
        },
        Del: function (row, index) {
            $.ajax({
                type: "get",
                url: $("#dttable").data("delurl"),
                data: { id: row.Id },
                success: function (res) {
                    if (res.status == 0) {
                        thisPage.Buttons.UserRoleTable.reload();
                        layer.close(index);
                    } else {
                        layer.msg(res.msg);
                    }
                }
            });
        }
    }

    thisPage.Init();
})