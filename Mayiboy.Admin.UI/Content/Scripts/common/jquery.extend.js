(function () {
    Date.prototype.format = function (format) {
        var o = {
            "M+": this.getMonth() + 1, //month 
            "d+": this.getDate(), //day 
            "h+": this.getHours(), //hour 
            "m+": this.getMinutes(), //minute 
            "s+": this.getSeconds(), //second 
            "q+": Math.floor((this.getMonth() + 3) / 3), //quarter 
            "S": this.getMilliseconds() //millisecond 
        }
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1,(this.getFullYear() + "").substr(4 - RegExp.$1.length));
        }

        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k] : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    };

    $.extend({
        Extend_formatDateTime: function (value) {
            if (value == null || value == '') {
                return '';
            }
            var dt;
            if (value instanceof Date) {
                dt = value;
            }
            else {
                dt = new Date(value);
                if (isNaN(dt)) {
                    value = value.replace(/\/Date\((-?\d+)\)\//, '$1');
                    dt = new Date();
                    dt.setTime(value);
                }
            }
            return dt.format("yyyy-MM-dd hh:mm");
        },
        Extend_formatDate: function (value) {
            if (value == null || value == '') {
                return '';
            }
            var dt;
            if (value instanceof Date) {
                dt = value;
            }
            else {
                dt = new Date(value);
                if (isNaN(dt)) {
                    value = value.replace(/\/Date\((-?\d+)\)\//, '$1');
                    dt = new Date();
                    dt.setTime(value);
                }
            }
            return dt.format("yyyy-MM-dd");
        },
        //加密
        Encrypt: function (value) {
            var rsa = new RSAKey();

            /*调用页面设置
                exponent = "指数";
                modulus = "系数";
            */

            var rsaexponent = $.cookie('exponent');
            var modulus = $.cookie('modulus');
            rsa.setPublic(modulus, rsaexponent);
            var res = rsa.encrypt(value);
            if (res) {
                return linebrk(hex2b64(res), 64);
            }
            else {
                return "";
            }
        },
        //获取参数
        GetQueryString: function(name) {
            var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
            var r = window.location.search.substr(1).match(reg);
            if (r != null) {
                return unescape(r[2]);
            }
            return null;
        }
    });
})(jQuery)