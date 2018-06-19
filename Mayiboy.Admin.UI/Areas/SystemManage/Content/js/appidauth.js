$(function () {
    var editappidauthnul;
    var editsecretkeynum;
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

            $("#refauthsecretKey").click(function() {
                if ($("#txtsecretKey").val().length > 0) {
                    layer.confirm("确认要更新秘钥？", function (index) {
                        $.get($("#refauthsecretKey").data("url"), { id: $("#txtid2").val() }, function (res) {
                            if (res.status == 0) {
                                $("#txtsecretKey").val(res.SecretKey);
                            } else {
                                layer.msg(res.msg);
                            }
                        });
                        layer.close(index);
                    });
                } else {
                    $.get($("#refauthsecretKey").data("url"), { id: $("#txtid2").val() }, function (res) {
                        if (res.status == 0) {
                            $("#txtsecretKey").val(res.SecretKey);
                        } else {
                            layer.msg(res.msg);
                        }
                    });
                }
            });

            $("#refauthpublicKey").click(function() {
                if ($("#txtpublicKey").val().length > 0) {
                    layer.confirm("确认要更新秘钥？", function (index) {
                        $.get($("#refauthpublicKey").data("url"), { id: $("#txtid2").val() }, function (res) {
                            if (res.status == 0) {
                                $("#txtpublicKey").val(res.PublicKey);
                                $("#txtprivateKey").val(res.PrivateKey);
                            } else {
                                layer.msg(res.msg);
                            }
                        });
                        layer.close(index);
                    });
                } else {
                    $.get($("#refauthpublicKey").data("url"), { id: $("#txtid2").val() }, function (res) {
                        if (res.status == 0) {
                            $("#txtpublicKey").val(res.PublicKey);
                            $("#txtprivateKey").val(res.PrivateKey);
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
                            { field: 'AppId', title: '应用标识', width: 100 },
                            { field: 'AuthToken', title: 'Token', width: 300 },
                            {
                                field: 'EncryptionType', title: '数据加密类型', width: 115, templet: function (row) {
                                    switch (row.EncryptionType) {
                                        case 0:
                                            return "不加密(0)";
                                        case 1:
                                            return "DES(1)";
                                        case 2:
                                            return "AES(2)";
                                        case 3:
                                            return "RSA(3)";
                                        default:
                                    }
                                }
                            },
                            {
                                field: 'Status',
                                title: '状态',
                                width: 80,
                                templet: function (row) {
                                    var html = "";
                                    html += '<input type="checkbox" name="open" value=' + row.Id + ' style="height:20px" title="启用" lay-skin="switch" lay-text="启用|禁用"  lay-filter="appidstatus" ' + (row.Status == 1 ? "checked" : "") + ' >';
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
                                    if (row.EncryptionType != 0) {
                                        html += '<a href="javascript:;" class="layui-btn layui-btn-normal layui-btn-xs" lay-event="editsecretkey">配置秘钥</a>';
                                    }
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
                        case "editsecretkey"://配置秘钥
                            thisPage.ShowEditSecretkeyPage(data);
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
                    layui.form.on('switch(appidstatus)', function (obj) {
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
                $("#encryptiontype").val("0");
            } else {
                title = "修改应用授权配置";
                $("#txtid").val(data.Id);
                $("#txtappid").val(data.AppId);
                $("#txtauthtoken").val(data.AuthToken);
                $("#status").prop("checked", (data.Status == 1));
                $("#encryptiontype").val(data.EncryptionType);
                $("#txtremark").val(data.Remark);
            }

            //手动渲染checked
            form.render("select");
            form.render("checkbox");


            editappidauthnul = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: ['450px', '470px'],
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
        ShowEditSecretkeyPage: function (data) {
            var title = "";
            var heigth = 100;
            var width = 450;
            $("#secretKey,#publicKey,#privateKey").hide();

            switch (data.EncryptionType) {
                case 1:
                {
                    title = "配置DSE秘钥";
                    $("#secretKey").show();
                    heigth = 210;
                    break;
                }
                case 2:
                {
                    title = "配置ASE秘钥";
                    $("#secretKey").show();
                    heigth = 210;
                    break;
                }
                case 3:
                {
                    title = "配置RSA秘钥";
                    $("#publicKey,#privateKey").show();
                    heigth = 600;
                    width = 1000;
                    break;
                }
                default:
            }

            $("#txtid2").val(data.Id);
            $("#txtsecretKey").val(data.SecretKey);
            $("#txtpublicKey").val(data.PublicKey);
            $("#txtprivateKey").val(data.PrivateKey);

            editsecretkeynum = layer.open({
                title: title,
                resize: false,
                type: 1,
                area: [width+'px', heigth+'px'],
                offset: '20px',
                content: $('#editsecretkey'), //这里content是一个DOM，注意：最好该元素要存放在body最外层，否则可能被其它的相对元素所影响
                btn: ["保存", "取消"],
                btn1: function () {
                    thisPage.SaveSecretKey();
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
                        EncryptionType: $("#encryptiontype").val(),
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
        SaveSecretKey: function () {
            $.ajax({
                type: "post",
                url: $("#editsecretkey").data("url"),
                data: {
                    id: $("#txtid2").val(),
                    secretKey: $("#txtsecretKey").val(),
                    privateKey: $("#txtprivateKey").val(),
                    publicKey: $("#txtpublicKey").val()
                },
                success:function(res) {
                    if (res.status == 0) {
                        layer.msg("保存成功");
                        thisPage.Buttons.AppIdAuthTable.reload();
                        layer.close(editsecretkeynum);
                    } else {
                        layer.msg(res.msg);
                    }
                }
            });
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