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
        public static string XmlPrivateKey = "PFJTQUtleVZhbHVlPjxNb2R1bHVzPjBuRXFxVFdmQ01BL0JPVThETmRxSnZnaUUxUmxtV1lmNytPL1ZjSkZoZ093ZTJTYlB3UzFXSG54SytnVkNZYld1cGVRZmtmN3NmZm1zM1hkdW5leU03Q0ViRk1ZcWZpQ01KVXYza3dESDVmSmpEQktGUklxTUNmM2FQMFhIY3BXbmhxaG9KOXdGRG0yUHl6VkVVTnpFcnJQZHJ0eEZNVkx4WXIzWTE3SXJ2RT08L01vZHVsdXM+PEV4cG9uZW50PkFRQUI8L0V4cG9uZW50PjxQPjdzeTBvVk9rcEkzOWZOcFVZWUloLzJHbklWRHlmaisyQ1I0UVkvbkNDRTVUVnZJcEFsMHZXQ25CcDRyRWdlU3crT0xlMWMzL0MzVGQxSm5tRUdvOHZ3PT08L1A+PFE+NFptUlV4bTlIVHgra29uUDQ3Qm1TWTZqMWg1K0tpM21ZZU1EWTVCbE5PaGkrdWsrM1M1KysvRjVzVlJXTUlXRkh0MHhGNnpBTHNab204UEJQc3dRVHc9PTwvUT48RFA+TklnaHdFK1hLMy8wNEk5aStxUlcrRWd5TFBrU3IwVXl0V1RBdEUyUUtxV1lYY3NkekdCVmR1NlFwRnU1aU4yWE5Oa3JyaHNIM1N4VFZGNmFwc2ZJSHc9PTwvRFA+PERRPmN3aEQ4clNZR25WRE1PTTJicW4rcmhrZWIwcWpHRXpKUVRaby92YWN0R3FlTmNTQytuTC82dVdKSUtPWlF5cUI0MmZ6NVZkL3N4b1dNdjZRNWIxVnZ3PT08L0RRPjxJbnZlcnNlUT42TElhai9UYU50T3QvZ29RdmJ3dXNOTGw4aWlOWVhybXNuc2ViY3Y0SitxSHdpYnVqaUVLSjBOZklGTURYZnNpNEJFVldqdVlRcmVyMlFXeldOSFBqQT09PC9JbnZlcnNlUT48RD5zUXB4eUg4SzkxSzg4Y0prdmF6d2FpNXdoSENKTlA5OHVOV1NYM1NjQnIzTjJGRndaaGp3ODZsTEl4UVJ0cjBRbzFQbFJNZ01VTkhzN1dlb2pUb3IyTmZpcFljWmRCeE1lbnZoUS9SOXdmMmN1c2s0Ny83Z25pM24weEhGTUpyVGo2MFNUaVhwNFd2RCtQbDJRT1FOWDRseTZKczN0UW5VYkFvVjhlZnJ6NTA9PC9EPjwvUlNBS2V5VmFsdWU+";

        /// <summary>
        /// 数据验证不通过
        /// </summary>
        public const string DataModelValidNotThrough = "";

		/// <summary>
		/// Url鉴权参数
		/// </summary>
	    public const string UrlAuth = "ticket";

        /// <summary>
        /// 用于身份Cookie对应的Key常量
        /// </summary>
        public const string IdentityCookieKey = "mayiboy.Identity";

		/// <summary>
		/// 当前登录Id Key
		/// </summary>
		public const string LoginUserIdKey = "mayiboy.loginuserid";

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