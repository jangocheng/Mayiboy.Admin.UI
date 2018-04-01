using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Mayiboy.UI.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        //登录
        public ActionResult Login()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }

        //退出登录
        public ActionResult Logout()
        {
            return Json(new { }, JsonRequestBehavior.AllowGet);
        }
    }
}