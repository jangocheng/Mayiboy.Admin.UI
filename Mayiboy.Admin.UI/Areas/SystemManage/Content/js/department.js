$(function () {
    var editdepartmentnum;
    var departmentdata;
    var selelctdefault = 0;
    var form;

    var thisPage = {
        Init: function () {
            thisPage.InitDepartment();

            $("#btnadd").click(function () {
                var row = $("#treetable").treegrid("getSelected");
                if (row != null) {
                    selelctdefault = row.Id;
                }

                thisPage.ShowEditPage();
            });

            //Layui坑
            layui.use(["table", "form"], function () {
                form = layui.form;
            });
        },
        InitDepartment: function () {
            $("#treetable").treegrid({
                url: $("#treetable").data("url"),
                method: "get",
                rownumbers: true, //如果为true，则显示一个行号列。
                idField: 'Id', //定义标识树节点的键名字段
                treeField: 'Name', //定义树节点的字段
                //lines:true,
                //fitColumns: true, //设置为 true，则会自动扩大或缩小列的尺寸以适应网格的宽度并且防止水平滚动。
                columns: [
                    [
                        { field: 'Name', title: '部门名称', width: 180 },
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
                            field: 'operid',
                            title: '操作',
                            width: 200,
                            align: 'center',
                            formatter: function (v, r) {
                                var html = "";

                                html += '<a href="javascript:;" data-id="' + r.Id + '" class="layui-btn layui-btn-xs editdepartment">编辑部门</a>';
                                html += '<a href="javascript:;" data-id="' + r.Id + '" class="layui-btn layui-btn-danger layui-btn-xs deldepartment" lay-event="del">删除</a>';
                                return html;
                            }

                        }
                    ]
                ],
                onLoadSuccess: function (v, data) {
                    departmentdata = data.rows;

                    $(".editdepartment").click(function () {
                        var row = thisPage.GetDepartmentById($(this).data("id"));
                        thisPage.ShowEditPage(row);
                    });

                    //$(".editp").click(function () {
                    //    console.log("设置权限");
                    //});

                    $(".deldepartment").click(function () {
                        var id = $(this).data("id");
                        layer.confirm('真的删除行吗？', function (index) {
                            thisPage.DelDepartment(id, index);
                        });
                    });
                }
            });
        },
        InitSelectDepartment: function () {
            $.ajax({
                url: $("#treetable").data("url"),
                data: {},
                success: function (res) {
                    if (res.status == 0) {
                        res.rows.push({ Id: 0, Pid: -1, Name: "根基菜单" });

                        var treelist = thisPage.ConvertListToTree(res.rows, -1);

                        $("#selectdepartment").combotree('loadData', treelist);
                    } else {
                        console.log(res.msg);
                    }
                }
            });
        },
        GetDepartmentById: function (id) {
            for (var item in departmentdata) {
                if (departmentdata[item].Id == id) {
                    return departmentdata[item];
                }
            }
            return null;
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
        ShowEditPage: function (data) {
            var title = "";
            thisPage.InitSelectDepartment();

            if (data == null) {
                title = "新增部门";
                $("#txtid").val("0");
                $('#selectdepartment').combotree('setValue', selelctdefault);
                $("#txtname").val("");
                $("#txtremark").val("");

            } else {
                title = "编辑部门";
                $("#txtid").val(data.Id);
                $('#selectdepartment').combotree('setValue', data.Pid);
                $("#txtname").val(data.Name);
                $("#txtremark").val(data.Remark);
            }

            form.render("select");

            editdepartmentnum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['500px', '380px'],
                offset: '20px',
                content: $('#editdepartment'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveDepartment();
                },
                btn2: function () {
                    // alert("取消");
                },
                cancel: function () {
                    // alert("退出");
                }
            });

        },
        SaveDepartment: function () {

            //验证表单
            var isb = layer.IsValidation("#editdepartment");

            if (isb == null) {
                $.ajax({
                    type: "post",
                    url: $("#editdepartment").data("url"),
                    data: {
                        id: $("#txtid").val(),
                        pid: $("#selectdepartment").combotree('getValue'),
                        name: $("#txtname").val(),
                        remark: $("#txtremark").val()
                    },
                    success: function (res) {
                        if (res.status == 0) {
                            layer.msg("保存成功！");
                            layer.close(editdepartmentnum);
                            thisPage.InitDepartment();
                        } else {
                            layer.msg(res.msg);
                        }
                    }
                });
            }
        },
        DelDepartment: function (id, index) {
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
                        layer.msg(res.msg);
                    }
                }
            });
        }
    }

    thisPage.Init();
})