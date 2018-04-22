using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysMenuController : BaseController
    {

        // GET: SystemManage/SysMenu
        public ActionResult Index()
        {
            ViewBag.SystemNavbar = QueryAllSystemNavbar();

            return View();
        }
    }
}