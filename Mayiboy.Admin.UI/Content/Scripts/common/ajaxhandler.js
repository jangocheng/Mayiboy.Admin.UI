(function () {
    //全局ajax loading
    //$(document).ready(function () {
    //    var _loading = $("#_loading");
    //    var count = 0;
    //    $(document).ajaxStart(function () {
    //        var _dateStr = new Date();
    //        _loading.data("__time__", _dateStr.getTime());
    //        count++;
    //        _loading.mask();
    //    }).ajaxStop(function () {
    //        count--;
    //        var _nowDate = new Date(),
    //            _time = null;
    //        if (_nowDate.getTime() - _loading.data("__time__") > 800) {
    //            _loading.unmask();
    //        } else {
    //            if (count == 0) {
    //                _time = setTimeout(function () {
    //                    _loading.unmask();
    //                }, 100);
    //            }
    //        }
    //    });
    //});

    $(document).ready(function () {
        //$(document).ajaxStart(function () {
        //    $.ajax({
        //        type: "GET",
        //        url: baseUrl + "Base/GetPagePermission",
        //        async: false,
        //        datatype: "json",
        //        success: function (data) {
        //            if (data.result != 0) {
        //                window.parent.location.href = data.url;
        //            }
        //        }
        //    });
        //});
    });
})();