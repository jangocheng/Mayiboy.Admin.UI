using System.Web;
using System.Web.Mvc;

namespace Mayiboy.UI
{
    public class LoginAuthAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Unauthorized(httpContext);
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            //if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
            //    (filterContext.RequestContext.HttpContext.Request.Url != null))
            //{
            //    filterContext.Result = new RedirectResult(PublicConst.Url.OnLogin);
            //}
            //else
            //{
            //    filterContext.Result = new ContentResult { Content = "未登录" };
            //}
        }

        private bool Unauthorized(HttpContextBase httpContext)
        {
            //var identityvalue = CookieHelper.Get(PublicConst.IdentityCookieKey);

            //if (identityvalue.IsNullOrEmpty())
            //{
            //    return false;
            //}

            //var entity = CacheManager.RedisDefault.Get<UserInfoDto>(identityvalue.AddCachePrefix(PublicConst.IdentityCookieKey));

            //return entity.IsNotNull();

            return false;
        }
    }
}