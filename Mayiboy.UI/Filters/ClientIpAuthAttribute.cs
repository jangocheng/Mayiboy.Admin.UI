using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Utils;

namespace Mayiboy.UI
{
    /// <summary>
    /// 客户端Ip认证
    /// </summary>
    public class ClientIpAuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 配置key
        /// </summary>
        private string ConfigKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configkey">配置key</param>
        public ClientIpAuthAttribute(string configkey)
        {
            ConfigKey = configkey;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="checkIpEnum">枚举配置key</param>
        public ClientIpAuthAttribute(Enum checkIpEnum)
        {
            ConfigKey = EnumHelper.GetDescription(checkIpEnum);
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //从本地缓存中获取Configkey的ip列表
            var key = "appsettings." + ConfigKey;

            var iplist = CacheManager.RunTimeCache.Get<List<string>>(key);

            var clientip = RequestHelper.Ip;

            if (iplist == null)
            {
                var value = ConfigHelper.GetString(ConfigKey);

                if (string.IsNullOrEmpty(value)) return true;

                if (clientip == "::1" || clientip == "127.0.0.1") return true;

                iplist = value.Split(new[] { ";", "；" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                CacheManager.RunTimeCache.Set(key, iplist, PublicConst.Time.Minute1);

                return iplist.Any(e => e == clientip);
            }
            else
            {
                return iplist.Any(e => e == clientip);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ContentResult { Content = "非法访问" };
        }


    }

    /// <summary>
    /// 配置key
    /// </summary>
    public enum ConfigKeyEnum
    {
        /// <summary>
        /// 内部服务器
        /// </summary>
        [Description("InternalServer")]
        InternalServer
    }
}