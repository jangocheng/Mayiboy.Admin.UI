using System.Web.Mvc;
using Mayiboy.ConstDefine;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 权限异常
    /// </summary>
    public class PermissionException : BaseException
    {
        /// <summary>
        /// 权限异常
        /// </summary>
        /// <param name="code">异常代码</param>
        /// <param name="message">异常消息</param>
        public PermissionException(int code, string message)
            : base(code, message)
        {
        }

        /// <summary>
        /// 权限异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public PermissionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// 权限异常
        /// </summary>
        /// <param name="data"></param>
        public PermissionException(object data)
            : base(data)
        {

        }

        /// <summary>
        /// 处理页面
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void ViewHandler(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(PublicConst.Url.NoPermission);
        }
    }
}