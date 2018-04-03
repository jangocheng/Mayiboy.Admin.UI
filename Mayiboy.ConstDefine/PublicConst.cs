namespace Mayiboy.ConstDefine
{
    /// <summary>
    /// 公用常量
    /// </summary>
    public class PublicConst
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public static string XmlPrivateKey = "<RSAKeyValue><Modulus>x9wznYDtUIgG3hJ1rid7c7zhh2Eik6NwmUiTRHPhfYrtg6Bf7mCvgEMayHXfrWFRh7LKuLuY55S4hC30HVglOcqlXobR8+pfnGAvSvSQVTWbtDIT6bEU10IC6YoJ0JVRoftpcHKnitMj98LIUUqa7Ggo/aeo0FmrBSh6ivGBwX8=</Modulus><Exponent>AQAB</Exponent><P>/qAe84fm6aTtaaEDu6lCdNgmc5lwIR+83/hdcLQ+KzC4/fNNhTJcr7SsapqpZ29jQgo2vtyPWf24ynoAA1sQEw==</P><Q>yPBl3j+4Kz1wHJtoQCV16akRLeH5vF6BXruE1RHrMGuLcIhxuqnPkEtS9J4yFfmavSGQnW8SSiB/GEQJB6suZQ==</Q><DP>YVEhg0J15ua6NpzrqFXQqIfUampCiOZwccmjLOg2upssmSLchgPxmNYc78Gc7YONFDiDI/94apSmg/yM9LthMQ==</DP><DQ>Twb1GX64ARGNuUKJssjI4hfjMMdyP9pvSQG5EU+VzxpM4fpXuFE22Ao32wsoqancaMv9o11etRaoxbNkVcbGXQ==</DQ><InverseQ>0tpQRWOIIBOXpLbcu/OFCG34VNKLUnVHNT+oRTM8PRYrwuSHLgh+Dq7eSwM2+MoBmE32gMhwqSnj+OfXwso0nA==</InverseQ><D>sd0SQlV+3XBxTCj5eefBQhsSSrzzXJjTmFayWPUX8/YzsjSDq014YplVuJjOWyqEuFMxfn3VciM0os2Stpq/ZL25TJaFf0IPmGO2dSDEjaQQ+lJkpT3UhJAhQMJLwwLkmUDWOCCQRLaGCIPyJFBc42OkFTG01+DSUcyCGsjXmqE=</D></RSAKeyValue>";

        /// <summary>
        /// 数据验证不通过
        /// </summary>
        public const string DataModelValidNotThrough = "";

        /// <summary>
        /// 用于身份Cookie对应的Key常量
        /// </summary>
        public const string IdentityCookieKey = "mayiboy.Identity";

        /// <summary>
        /// 常用地址
        /// </summary>
        public class Url
        {
            /// <summary>
            /// 系统异常地址
            /// </summary>
            public const string SystemException = "";

            /// <summary>
            /// 登录Url
            /// </summary>
            public const string OnLogin = "~/Account/Index";

            /// <summary>
            /// 登出Url
            /// </summary>
            public const string OutLogin = "~/Account/Out";

            /// <summary>
            /// 主页地址
            /// </summary>
            public const string Home = "~/Main/Index";

            /// <summary>
            /// 无权限
            /// </summary>
            public const string NoPermission = "~/Main/permission";
        }

        /// <summary>
        /// 时间
        /// </summary>
        public class Time
        {
            /// <summary>
            /// 1分钟（60秒）
            /// </summary>
            public const int Minute1 = 60;

            /// <summary>
            /// 5分钟（300秒）
            /// </summary>
            public const int Minute5 = 300;

            /// <summary>
            /// 10分钟（600秒）
            /// </summary>
            public const int Minute10 = 600;

            /// <summary>
            /// 30分钟（1800秒）
            /// </summary>
            public const int Minute30 = 1800;

            /// <summary>
            /// 1小时（3600秒）
            /// </summary>
            public const int Hour1 = 3600;

            /// <summary>
            /// 2小时（7200秒）
            /// </summary>
            public const int Hour2 = 14400;

            /// <summary>
            /// 4小时（7200秒）
            /// </summary>
            public const int Hour4 = 7200;

            /// <summary>
            /// 1天（43200秒）
            /// </summary>
            public const int Day1 = 43200;

            /// <summary>
            /// 7天（604800秒）
            /// </summary>
            public const int Day7 = 604800;//7天
        }
    }
}