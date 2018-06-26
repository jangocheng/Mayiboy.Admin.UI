using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.Admin.UI.Controllers
{
	/// <summary>
	/// 单点登录
	/// </summary>
	public class LoginController : Controller
	{
		//统一登陆页面
		public ActionResult Index(string fromurl, string fromappid)
		{
			if (string.IsNullOrEmpty(fromurl) || string.IsNullOrEmpty(fromappid))
			{
				return Content("参数有误");
			}

			ViewBag.FormUrl = fromurl;

			return View();
		}

		//登陆
		public ActionResult Submit(string fromappid, string username, string password, string code)
		{
			//如果重定向回调地址

			return Content("");
		}

		//验证凭证是否正确
		public ActionResult ValidateClientTicket()
		{
			return Content("");
		}
	}
}