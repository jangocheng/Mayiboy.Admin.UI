$(function () {
    var edituserinfonum;
    var edituserorlenum;
    var edituserid;
    var form;
    var thisPage = {
        Buttons: {
            UserInfoTable: null,
            UserRoleTable: null
        },
        Init: function () {
            thisPage.InitUserInfoTable();

            $("#btnadd").click(function () {
                thisPage.ShowEditPage();
            });

            $("#btnquery").click(function () {
                thisPage.QueryUserInfo();
            });

            //Layui坑
            layui.use(["table", "form"], function () {
                form = layui.form;
            });
        },
        InitUserInfoTable: function () {
            layui.use("table", function () {
                var table = layui.table;

                thisPage.Buttons.UserInfoTable = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    page: true, //开启分页
                    even: true, //各行变色
                    cols: [
                        [
                            { field: 'id', title: '序号', type: "numbers", width: 60 },
                            { field: 'LoginName', title: '账号', width: 150 },
                            { field: 'Name', title: '姓名', width: 150 },
                            { field: 'Email', title: '邮箱', width: 200 },
                            {
                                field: 'Sex',
                                title: '性别',
                                width: 200,
                                templet: function (row) {
                                    switch (row.Sex) {
                                        case 0:
                                            return "女";
                                        case 1:
                                            return "男";
                                        default:
                                            return "";
                                    }
                                }
                            },
                            { field: 'Mobile', title: "手机号", width: 200 },
                            {
                                field: 'CreateTime',
                                title: '创建时间',
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
                            {
                                field: 'look',
                                title: '操作',
                                width: 200,
                                align: 'center',
                                templet: function (row) {
                                    var html = "";
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>';
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="edituserrole">设置用户角色</a>';
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
                    edituserid = data.Id;

                    switch (obj.event) {
                        case "edit":
                            thisPage.ShowEditPage(data);
                            break;
                        case "edituserrole":
                            thisPage.ShowEditUserRolePage(data);
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
        QueryUserInfo: function () {
            thisPage.Buttons.UserInfoTable.reload({
                where: {
                    account: $("#account").val(),
                    sex: $("#dbdsex").val()
                },
                page: {
                    curr: 1 //重新从第 1 页开始
                }
            });
        },
        ShowEditPage: function (data) {
            var title = "";
            if (data == null) {
                $("#txtid").val("0");
                $("#txtloginname").val("");
                $("#txtemail").val("");
                $("#txtname").val("");
                $("#selectsex").val("0");
                $("#txtmobile").val("");
                $("#txtremark").val("");
                title = "新增用户";
            } else {
                $("#txtid").val(data.Id);
                $("#txtloginname").val(data.LoginName);
                $("#txtemail").val(data.Email);
                $("#txtname").val(data.Name);
                $("#selectsex").val(data.Sex);
                $("#txtmobile").val(data.Mobile);
                $("#txtremark").val(data.Remark);
                title = "修改用户";
            }

            form.render('select');

            edituserinfonum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '520px'],
                offset: '20px',
                content: $('#edituserinfo'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveSysNavbar();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });
        },
        ShowEditUserRolePage: function (data) {
            //初始化角色列表
            var table = layui.table;
            thisPage.Buttons.UserRoleTable = table.render({
                elem: '#tbtableuserrole',
                url: $("#tbtableuserrole").data("url"),
                where: {
                    userid:data.Id
                },
                page: false,
                even: true,
                cols: [[
                    { type:'checkbox'},
                    { field: 'Name', title: "角色名",width:200 },
                    { field: 'Remark', title: "备注",width:500 }
                ]],
                done: function (res, curr, count) {
                    var a = res;
                }
            });

            edituserorlenum = layer.open({
                title: "设置用户角色",
                resize: false,
                type: 1,
                area: ['800px', '400px'],
                offset: '20px',
                content: $('#edituserrole'),
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

            var selectdata = layui.table.checkStatus("tbtableuserrole").data;

            var roleid = [];
            $.each(selectdata, function(i,v) {
                roleid.push(v.Id);
            });

            $.ajax({
                type: "post",
                url: $("#tbtableuserrole").data("savaurl"),
                data: {
                    userid: edituserid,
                    roleid: roleid
                },
                success:function(res) {
                    if (res.status == 0) {
                        layer.close(edituserorlenum);
                    } else {
                        layer.msg(res.msg);
                    }
                }
            });
        },
        SaveSysNavbar: function () {
            //验证表单
            var isb = layer.IsValidation("#edituserinfo");

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#edituserinfo").data("url"),
                    data: {
                        id: $("#txtid").val(),
                        LoginName: $("#txtloginname").val(),
                        Email: $("#txtemail").val(),
                        Name: $("#txtname").val(),
                        Sex: $("#selectsex").val(),
                        Mobile: $("#txtmobile").val(),
                        Remark: $("#txtremark").val()
                    },
                    success(res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Buttons.UserInfoTable.reload();
                            layer.close(edituserinfonum);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        Del: function (row, index) {
            $.ajax({
                type: "get",
                url: $("#dttable").data("delurl"),
                data: { id: row.Id },
                success: function (res) {
                    if (res.status == 0) {
                        thisPage.Buttons.UserInfoTable.reload();
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