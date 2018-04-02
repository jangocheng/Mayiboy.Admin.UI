using System;
using System.Web.Mvc;
using Mayiboy.ConstDefine;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 自定义异常基类
    /// </summary>
    public class BaseException : ApplicationException
    {
        protected readonly int ExceptionCode = -1;
        protected readonly string ExceptionMessage;
        protected readonly object ResultData;

        public BaseException(int code, string message)
        {
            this.ExceptionCode = code;
            this.ExceptionMessage = message;
        }

        public BaseException(string message)
        {
            this.ExceptionMessage = message;
        }

        public BaseException(object data)
        {
            ResultData = data;
        }

        /// <summary>
        /// Ajax处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void AjaxHandler(ExceptionContext filterContext)
        {
            var result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet };

            if (ResultData == null)
            {
                result.Data = new
                {
                    status = ExceptionCode,
                    msg = ExceptionMessage
                };
            }
            else
            {
                result.Data = ResultData;
            }

            filterContext.Result = result;
        }

        /// <summary>
        /// 视图处理函数
        /// </summary>
        /// <param name="filterContext"></param>
        protected virtual void ViewHandler(ExceptionContext filterContext)
        {
            filterContext.HttpContext.Response.Redirect(PublicConst.Url.SystemException);
        }

        /// <summary>
        /// 异常结果处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected void ExceptionResultHandler(ExceptionContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                AjaxHandler(filterContext);
            }
            else
            {
                ViewHandler(filterContext);
            }
        }

        /// <summary>
        /// 注册自定义定义异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        public static void RegisterExceptionHandler(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as BaseException;

            if (exception != null)
            {
                exception.ExceptionResultHandler(filterContext);

                filterContext.ExceptionHandled = true;
            }
        }
    }
}