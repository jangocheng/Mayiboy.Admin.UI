using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.Admin.UI.Controllers
{
    public class MainController : Controller
    {
        /// <summary>
        /// 无权限403
        /// </summary>
        /// <returns></returns>
        public ActionResult Page403()
        {
            return View();
        }

        /// <summary>
        /// 内部服务器错误500
        /// </summary>
        /// <returns></returns>
        public ActionResult Page500()
        {
            return View();
        }

    }
}