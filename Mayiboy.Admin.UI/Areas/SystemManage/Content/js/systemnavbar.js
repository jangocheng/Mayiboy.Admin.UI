$(function () {
    var editsysnavbarnum;
    var thisPage = {
        Buttons: {
            Table: null
        },
        Init: function () {
            thisPage.InitTable();
            $("#btnadd").click(function () {
                thisPage.ShowEditPage();
            });
        },
        ShowEditPage: function (data) {

            var title = "";
            if (data == null) {
                //初始化编辑页
                $("#txtid").val("0");
                $("#txtname").val("");
                $("#txturl").val("");
                $("#txtsort").val("0");
                $("#txtremark").val("");
                title = "新增系统栏目";
            } else {
                //初始化编辑页
                $("#txtid").val(data.Id);
                $("#txtname").val(data.Name);
                $("#txturl").val(data.Url);
                $("#txtsort").val(data.Sort);
                $("#txtremark").val(data.Remark);
                title = "修改系统栏目";
            }


            editsysnavbarnum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '415px'],
                offset: '20px',
                content: $('#editsysnavbar'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
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
        SaveSysNavbar: function () {

            //验证表单
            var isb = layer.IsValidation("#editsysnavbar");

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editsysnavbar").data("url"),
                    data: {
                        id: $("#txtid").val(),
                        name: $("#txtname").val(),
                        url: $("#txturl").val(),
                        sort: $("#txtsort").val(),
                        remark: $("#txtremark").val()
                    },
                    success:function(res) {
                        if (res.status == 0) {
                            layer.msg("保存成功");
                            thisPage.Buttons.Table.reload();
                            layer.close(editsysnavbarnum);
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        InitTable: function () {
            layui.use("table", function () {
                var table = layui.table;
                //第一个实例
                thisPage.Buttons.Table = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    page: true, //开启分页
                    even: true, //各行变色
                    cols: [[
                            { field: 'id', title: '序号', type: "numbers", width: 60 },
                            { field: 'Name', title: '名称', width: 200 },
                            { field: 'Url', title: '地址' },
                            { field: 'Sort', title: '排序', width: 200 },
                            { field: 'Remark', title: '备注', width: 200 },
                            {
                                field: 'CreateTime',title: '创建时间',width: 180,templet: function (row) {
                                    return $.Extend_formatDateTime(row.CreateTime);
                                }
                            },
                            {
                                field: 'look',title: '操作',width: 150,templet: function (row) {
                                    var html = "";
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-xs" lay-event="edit">编辑</a>';
                                    html += '<a href="javascript:;" class="layui-btn layui-btn-danger layui-btn-xs" lay-event="del">删除</a>';
                                    return html;
                                }
                            }
                    ]],
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
                        case "del":
                            layer.confirm('真的删除吗？', function (index) {
                                thisPage.Del(data, index);
                            });
                            break;
                        default:
                    }
                });
            });
        },
        Del:function (row, index) {
            $.ajax({
                type: "get",
                url: $("#dttable").data("delurl"),
                data: { id: row.Id },
                success: function (res) {
                    if (res.status == 0) {
                        thisPage.Buttons.Table.reload();
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