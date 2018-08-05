using System.Web.Mvc;
using Framework.Mayiboy.Utility;

namespace Mayiboy.UI
{
	/// <summary>
	/// 兼容跨域请求（jsonp数据格式）
	/// </summary>
	public class JsonpAttribute: ActionFilterAttribute
	{
		public override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			var callbackfunction = filterContext.HttpContext.Request["callback"];

			if (!string.IsNullOrEmpty(callbackfunction))
			{
				var jsonResult = filterContext.Result as JsonResult;
				var contentResult = filterContext.Result as ContentResult;

				if (jsonResult != null)
				{
					var content = string.Format("{0}({1})", callbackfunction, jsonResult.Data.ToJson());
					filterContext.Result = new ContentResult { Content = content };

				}
				else if (contentResult != null)
				{
					var content = string.Format("{0}({1})", callbackfunction, contentResult.Content);
					filterContext.Result = new ContentResult { Content = content };
				}
			}

			base.OnActionExecuted(filterContext);
		}
	}
}