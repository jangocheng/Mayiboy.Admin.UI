using System.Web.Mvc;
using Mayiboy.ConstDefine;

namespace Mayiboy.UI
{
    /// <summary>
    /// 登录异常
    /// </summary>
    public class LoginException : BaseException
    {


        /// <summary>
        /// 登录异常
        /// </summary>
        /// <param name="code">异常代码</param>
        /// <param name="message">异常消息</param>
        public LoginException(int code, string message)
            : base(code, message)
        { }

        /// <summary>
        /// 登录异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public LoginException(string message)
            : base(message)
        { }


        /// <summary>
        /// 登录异常
        /// </summary>
        /// <param name="data"></param>
        public LoginException(object data)
            : base(data)
        {

        }

        /// <summary>
        /// 视图处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void ViewHandler(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(PublicConst.Url.OnLogin);
        }
    }
}