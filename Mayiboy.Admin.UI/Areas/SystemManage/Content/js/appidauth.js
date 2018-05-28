$(function () {
    var editappidauthnul;
    var form;

    //Layui坑
    layui.use(["table", "form"], function () {
        form = layui.form;
    });

    var thisPage = {
        Buttons: {
            AppIdAuthTable: null
        },
        Init: function () {
            thisPage.InitAppIdAuthTable();

            $("#btnadd").click(function () {
                thisPage.ShowEditPage();
            });

            $("#btnquery").click(function () {
                thisPage.QueryAppIdAuth();
            });

            $("#refauthtoken").click(function () {
                if ($("#txtauthtoken").val().length > 0) {
                    layer.confirm("确认要刷新代码", function (index) {
                        $.get($("#refauthtoken").data("url"), function (res) {
                            if (res.status == 0) {
                                $("#txtauthtoken").val(res.content);
                            } else {
                                layer.msg(res.msg);
                            }
                        });
                        layer.close(index);
                    });
                } else {
                    $.get($("#refauthtoken").data("url"), function (res) {
                        if (res.status == 0) {
                            $("#txtauthtoken").val(res.content);
                        } else {
                            layer.msg(res.msg);
                        }
                    });
                }

            });

        },
        InitAppIdAuthTable: function () {
            layui.use("table", function () {
                var table = layui.table;
                thisPage.Buttons.AppIdAuthTable = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    page: true, //开启分页
                    even: true, //各行变色
                    cols: [
                        [
                            { field: 'id', title: '序号', type: "numbers", width: 60 },
                            { field: 'AppId', title: '应用标识', width: 150 },
                            { field: 'AuthToken', title: '应用授权', width: 400 },
                            {
                                field: 'Status',
                                title: '状态',
                                width: 150,
                                templet: function (row) {
                                    var html = "";
                                    html += '<input type="checkbox" name="lock" value=' + row.Id + ' style="height:20px" title="启用" lay-filter="appidstatus" ' + (row.Status == 1 ? "checked" : "") + ' >';
                                    return html;
                                }
                            },
                            {
                                field: 'CreateTime',
                                title: '新增时间',
                                width: 180,
                                templet: function (row) {
                                    return $.Extend_formatDateTime(row.CreateTime);
                                }
                            },
                            { field: 'Remark', title: '备注' },
                            {
                                field: 'look',
                                title: '操作',
                                width: 200,
                                templet: function (row) {
                                    var html = "";
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>';
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>';
                                    return html;
                                }
                            }
                        ]
                    ],
                    done: function (res, curr, count) {

                    }
                });

                //监听工具栏
                table.on('tool(table-operate)', function (obj) {
                    var data = obj.data; //获得当前行数据
                    var layEvent = obj.event; //获得 lay-event 对应的值（也可以是表头的 event 参数对应的值）
                    var tr = obj.tr; //获得当前行 tr 的DOM对象

                    switch (obj.event) {
                        case "edit":
                            thisPage.ShowEditPage(data);
                            break;
                        case "del":
                            layer.confirm('真的删除行吗？', function (index) {
                                thisPage.Del(data, index);
                            });
                            break;
                        default:
                    }
                });

                //监听状态操作
                layui.use(["form"], function () {
                    layui.form.on('checkbox(appidstatus)', function (obj) {
                        $.ajax({
                            type: "get",
                            url: $("#dttable").data("updatestatusurl"),
                            data: {
                                id: this.value,
                                status: obj.elem.checked ? 1 : 0
                            },
                            success: function (res) {
                                if (res.status == 0) {
                                    layer.msg('成功', { time: 800 });
                                } else {
                                    layer.msg(res.msg);
                                }
                            }
                        });
                    });
                });

            });
        },
        ShowEditPage: function (data) {
            var title = "";
            if (data == null) {
                title = "新增应用授权配置";
                $("#txtid").val("0");
                $("#txtappid").val("");
                $("#txtauthtoken").val("");
                $("#status").prop("checked", true);
                $("#txtremark").val("");

            } else {
                title = "修改应用授权配置";
                $("#txtid").val(data.Id);
                $("#txtappid").val(data.AppId);
                $("#txtauthtoken").val(data.AuthToken);
                $("#status").prop("checked", (data.Status == 1));
                $("#txtremark").val(data.Remark);
            }

            //手动渲染checked
            form.render("checkbox");

            editappidauthnul = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '415px'],
                offset: '20px',
                content: $('#editappidauth'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveAppIdAuth();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });
        },
        SaveAppIdAuth: function () {
            //验证表单
            var isb = layer.IsValidation("#editappidauth");
            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editappidauth").data("url"),
                    data: {
                        id: $("#txtid").val(),
                        appid: $("#txtappid").val(),
                        authtoken: $("#txtauthtoken").val(),
                        status: ($("#status").prop("checked") ? "1" : "0"),
                        remark: $("#txtremark").val()
                    },
                    success: function (res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Buttons.AppIdAuthTable.reload();
                            layer.close(editappidauthnul);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        QueryAppIdAuth: function () {
            thisPage.Buttons.AppIdAuthTable.reload({
                where: {
                    appid: $("#appid").val(),
                    authtoken: $("#authtoken").val()
                },
                page: {
                    curr: 1 //重新从第 1 页开始
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
                        thisPage.Buttons.AppIdAuthTable.reload();
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