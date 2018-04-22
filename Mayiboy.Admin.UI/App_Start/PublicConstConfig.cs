namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 常用常量
    /// </summary>
    public class PublicConstConfig
    {
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
    }
}