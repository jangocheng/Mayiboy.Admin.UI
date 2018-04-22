using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 登录账号信息
    /// </summary>
    public class AccountConfig
    {

        /// <summary>
        /// 客户端身份Id值
        /// </summary>
        public static string Identity
        {
            get
            {
                var identityvalue = CookieHelper.Get(PublicConst.IdentityCookieKey);

                return identityvalue;
            }
        }

        /// <summary>
        /// 登录用户缓存key
        /// </summary>
        public static string IdentityCacheKey
        {
            get
            {
                if (Identity.IsNullOrEmpty()) return null;

                return Identity.AddCachePrefix(PublicConst.IdentityCookieKey);
            }
        }

        /// <summary>
        /// 登录账号信息
        /// </summary>
        public static AccountModel LoginInfo
        {
            get
            {

                if (IdentityCacheKey.IsNullOrEmpty()) return null;

                var entity = CacheManager.Get<AccountModel>(IdentityCacheKey, 2);

                if (entity != null)
                {
                    CacheManager.RedisDefault.Expire(IdentityCacheKey, PublicConst.Time.Hour1);
                }
                return entity;
            }
        }
    }
}