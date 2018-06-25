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
            /// 主页地址
            /// </summary>
            public const string Home = "~/Home/Index";

			/// <summary>
			/// 数据验证不通过
			/// </summary>
			public const string DataModelValidNotThrough = "";

			/// <summary>
			/// 登录Url
			/// </summary>
			public const string OnLogin = "~/Account/Index";

            /// <summary>
            /// 登出Url
            /// </summary>
            public const string OutLogin = "~/Account/Out";

            /// <summary>
            /// 无权限页面地址
            /// </summary>
            public const string NoPermission = "~/Main/Page403";

            /// <summary>
            /// 系统异常地址
            /// </summary>
            public const string SystemException = "~/Main/Page500";
        }
    }
}