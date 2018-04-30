using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Admin.UI.Controllers;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysDictController : BaseController
    {


        // GET: SystemManage/SysDict
        [LoginAuth]
        public ActionResult Index()
        {
            return View();
        }
    }
}