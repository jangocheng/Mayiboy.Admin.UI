layui.use(['form', 'layer', 'jquery'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer;
    $ = layui.jquery;

    //登录按钮
    form.on("submit(login)", function (data) {
        $(this).text("登录中...").attr("disabled", "disabled").addClass("layui-disabled");

        var loginbtn = $(this);

        //登录
        $.ajax({
            type: "post",
            url: $(this).data("url"),
            data: {
                username: $.Encrypt($("#userName").val()),
                password: $.Encrypt($("#password").val()),
                code: $("#code").val()
            },
            success: function (res) {
                if (res.status == 0) {
                    location.href = $("#loginbtn").data("tourl");
                } else if (res.status == 1) {
                    loginbtn.text("登录").removeAttr("disabled").removeClass("layui-disabled");
                    layer.msg(res.msg);
                    $("#vcode").click();
                } else {
                    loginbtn.text("登录").removeAttr("disabled").removeClass("layui-disabled");
                    layer.msg(res.msg);
                }
               
            }
        });

        return false;
    });

    //表单输入效果
    $(".loginBody .input-item").click(function (e) {
        e.stopPropagation();
        $(this).addClass("layui-input-focus").find(".layui-input").focus();
    });
    $(".loginBody .layui-form-item .layui-input").focus(function () {
        $(this).parent().addClass("layui-input-focus");
    });
    $(".loginBody .layui-form-item .layui-input").blur(function () {
        $(this).parent().removeClass("layui-input-focus");
        if ($(this).val() != '') {
            $(this).parent().addClass("layui-input-active");
        } else {
            $(this).parent().removeClass("layui-input-active");
        }
    });
})
