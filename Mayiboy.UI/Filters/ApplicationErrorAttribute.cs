using System;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Utils;

namespace Mayiboy.UI
{
    /// <summary>
    /// 应用程序异常处理
    /// </summary>
    public class ApplicationErrorAttribute : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            BaseException.RegisterExceptionHandler(filterContext);

            #region 记录未处理的异常信息
            if (!filterContext.ExceptionHandled)
            {
                var routeData = filterContext.RequestContext.RouteData;

                string controllerName = routeData.Values["controller"].IsNotNull() ? routeData.Values["controller"].ToString() : "";
                string actionName = routeData.Values["action"].IsNotNull() ? routeData.Values["action"].ToString() : "";

                string errormsg = string.Format("ExceptionTime:{0}\r\nIpAddress:{1}\r\nControllerName:{2}\r\nActionName{3}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), RequestHelper.Ip, controllerName, actionName);

                Exception ex = filterContext.Exception;
                ex.Source = errormsg + "\r\n" + ex.Source;

                LogManager.DefaultLogger.Fatal(ex);
            }
            #endregion
        }
    }
}