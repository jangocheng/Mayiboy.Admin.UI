using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.ConstDefine;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI
{
    public class LoginAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Unauthorized(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
                (filterContext.RequestContext.HttpContext.Request.Url != null))
            {
                filterContext.Result = new RedirectResult(PublicConst.Url.OnLogin);
            }
            else
            {
                filterContext.Result = new ContentResult { Content = "未登录" };
            }
        }

        private bool Unauthorized(HttpContextBase httpContext)
        {
            bool islogin = true;

            var identityvalue = CookieHelper.Get(PublicConst.IdentityCookieKey);

            if (identityvalue.IsNullOrEmpty())
            {
                islogin = false;
            }

            var entity = CacheManager.RedisDefault.Get<AccountModel>(identityvalue.AddCachePrefix(PublicConst.IdentityCookieKey));


            if (entity == null || !RequestHelper.CheckFingerprint(entity.Fingerprint))
            {
                islogin = false;
            }

            return islogin;
        }
    }
}