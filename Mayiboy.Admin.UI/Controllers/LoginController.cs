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
		public ActionResult Index(string frommurl, string fromappid)
		{
			if (string.IsNullOrEmpty(frommurl) || string.IsNullOrEmpty(fromappid))
			{
				return Content("参数有误");
			}

			ViewBag.FormUrl = frommurl;

			return View();
		}

		public ActionResult Submit(string fromappid, string username, string password, string code)
		{
			return Content("");
		}
	}
}