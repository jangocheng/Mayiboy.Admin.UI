using System.Web.Mvc;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI
{
    /// <summary>
    /// 操作日志记录
    /// </summary>
    public class OperLogAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// 内容
        /// </summary>
        private string LogContent { get; set; }

        private ISystemOperationLogService SystemOperationLogService { get; set; }


        public OperLogAttribute(string logcontent)
        {
            LogContent = logcontent;
            SystemOperationLogService = ServiceLocater.GetService<ISystemOperationLogService>();
        }

        /// <summary>
        /// 在Action执行之前由 MVC 框架调用。
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string loginname = "未知:";
            string name = "未知:";

            if (LoginAccount.UserInfo != null)
            {
                loginname = LoginAccount.UserInfo.LoginName;
                name = LoginAccount.UserInfo.Name;
            }
            else
            {
                loginname += RequestHelper.Ip;
                name += RequestHelper.Ip;
            }

            string logcontent = string.Format("[LoginName:{0}]-[Name:{1}]-[Content:{2}]", loginname, name, LogContent);

            var response = SystemOperationLogService.AddOperationLog(new AddOperationLogRequest
            {
                Content = logcontent
            });

            if (!response.IsSuccess)
            {
                LogManager.DefaultLogger.ErrorFormat("记录系统日志出错");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}