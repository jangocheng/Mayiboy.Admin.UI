using System.Collections.Generic;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 登录账号信息
    /// </summary>
    public class LoginAccount
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
        public static AccountModel UserInfo
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

        /// <summary>
        /// 用户权限
        /// </summary>
        public static List<PermissionsDto> UserPermissions
        {
            get
            {
                if (UserInfo == null)
                {
                    return null;
                }

                var key = Identity.AddCachePrefix("userpermission");

                var entitylist = CacheManager.Get<List<PermissionsDto>>(key, 2);

                if (entitylist == null || entitylist.Count == 0)
                {
                    var res = ServiceLocater.GetService<IPermissionsService>().QueryPermissionsByUserId(new QueryPermissionsByUserIdRequest
                    {
                        UserId = UserInfo.Id
                    });

                    if (res.IsSuccess)
                    {
                        entitylist = res.EntityList;
                    }
                    else
                    {
                        LogManager.DefaultLogger.ErrorFormat("查询用户权限出错：{0}", new { UserInfo, res }.ToJson());
                    }
                }

                return entitylist;
            }
        }
    }
}