using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using System.Linq;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 操作授权
    /// </summary>
    public class ActionAuthAttribute : AuthorizeAttribute
    {
        private string ActionCode { get; set; }

        public ActionAuthAttribute()
        {

        }

        public ActionAuthAttribute(string actioncode)
        {
            ActionCode = actioncode;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return Unauthorized(httpContext);
        }


        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (!filterContext.RequestContext.HttpContext.Request.IsAjaxRequest() &&
                (filterContext.RequestContext.HttpContext.Request.Url != null))
            {
                filterContext.Result = new RedirectResult(PublicConstConfig.Url.OnLogin);
            }
            else
            {
                filterContext.Result = new ContentResult { Content = "没有权限" };
            }
        }

        private bool Unauthorized(HttpContextBase httpContext)
        {
            var entity = LoginAccount.UserInfo;

            if (entity == null || !RequestHelper.CheckFingerprint(entity.Fingerprint))
            {
                return false;
            }

            string controllername = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString();
            string actionname = httpContext.Request.RequestContext.RouteData.Values["action"].ToString();

            //获取用户权限

            if (string.IsNullOrEmpty(ActionCode))
            {
                //判断请求地址权限
                if (LoginAccount.UserPermissions == null)
                {
                    return false;
                }
                else
                {
                    return LoginAccount.UserPermissions.Any(e => e.Action.ToLower().Contains(string.Format("{0}/{1}", controllername.ToLower(), actionname.ToLower())));
                }
            }
            else
            {
                //判断请求权限码
                return LoginAccount.UserPermissions.Any(e => e.Code == ActionCode);
            }
        }
    }
}