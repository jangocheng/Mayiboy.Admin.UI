using System.Web.Mvc;
using Mayiboy.ConstDefine;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 数据验证异常
    /// </summary>
    public class ViewModelException : BaseException
    {

        /// <summary>
        /// 构造数据验证异常
        /// </summary>
        /// <param name="code">异常代码</param>
        /// <param name="message">异常消息</param>
        public ViewModelException(int code, string message)
            : base(code, message)
        { }

        /// <summary>
        /// 构造数据验证异常
        /// </summary>
        /// <param name="message">异常消息</param>
        public ViewModelException(string message)
            : base(message)
        { }

        public ViewModelException(object data)
            : base(data)
        {

        }

        /// <summary>
        /// 视图处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void ViewHandler(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(PublicConst.DataModelValidNotThrough);
        }
    }
}