$(function () {
    var thisPage = {
        Buttons: {
            SysLogTable: null
        },
        Init: function () {
            thisPage.InitSysLogTable();
        },
        InitSysLogTable: function () {
            layui.use("table", function () {
                var table = layui.table;
                thisPage.Buttons.SysLogTable = table.render({
                    elem: '#dttable',
                    url: $("#dttable").data("url"), //数据接口
                    limit: 20,
                    page: true, //开启分页
                    even: true, //各行变色
                    cols: [
                            [
                                { field: 'id', title: '序号', type: "numbers", width: 60 },
                                { field: 'Content', title: '日志内容' },
                                {
                                    field: 'CreateTime',
                                    title: '创建时间',
                                    width: 180,
                                    templet: function (row) {
                                        return $.Extend_formatDateTime(row.CreateTime);
                                    }
                                }
                            ]
                    ],
                    done: function (res, curr, count) {

                    }
                });

            });
        }
    }

    thisPage.Init();
})