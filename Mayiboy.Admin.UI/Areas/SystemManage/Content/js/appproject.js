$(function () {
    var selectappproject;
    var editappprojectnum;

    var thisPage = {
        Controls: {
            AppProjectTable: null
        },
        Init: function () {
            thisPage.InitAppProjectTable();

            $("#btnadd").click(function () {
                thisPage.ShowEditPage();
            });

            $("#btnquery").click(function () {
                thisPage.QueryAppProject();
            });

        },
        InitAppProjectTable: function () {
            layui.use("table", function () {
                var table = layui.table;

                thisPage.Controls.AppProjectTable = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    page: true, //开启分页
                    limit: 20,
                    even: true, //各行变色
                    cols: [
                        [
                            { field: 'id', title: '序号', type: "numbers", width: 60 },
                            { field: 'ProjectName', title: '项目名称', width: 150 },
                            { field: 'ApplicationId', title: '应用Id', width: 150 },
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
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>';
                                    return html;
                                }
                            }
                        ]
                    ],
                    done: function (res, curr, count) {

                    }
                });

                table.on("tool(table-operate)", function (obj) {
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
            });
        },
        ShowEditPage: function (data) {
            var title = "";

            if (data == null) {
                title = "新增项目";
                $("#txtname").val("");
                $("#txtappid").val("");
                $("#txtremark").val("");
            } else {
                title = "编辑项目";
                $("#txtname").val(data.ProjectName);
                $("#txtappid").val(data.ApplicationId);
                $("#txtremark").val(data.Remark);
            }

            editappprojectnum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '400px'],
                offset: '20px',
                content: $('#editappproject'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveAppProject();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });
        },
        SaveAppProject: function () {
            var isb = layer.IsValidation("#editappproject");

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editappproject").data("url"),
                    data: {
                        name: $("#txtname").val(),
                        appid: $("#txtappid").val(),
                        remark: $("#txtremark").val()
                    },
                    success: function (res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Controls.AppProjectTable.reload();
                            layer.close(editappprojectnum);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        QueryAppProject: function () {
            thisPage.Buttons.UserRoleTable.reload({
                where: {
                    name: $("#name").val(),
                    appid: $("#appid").val()
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
                        thisPage.Controls.AppProjectTable.reload();
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