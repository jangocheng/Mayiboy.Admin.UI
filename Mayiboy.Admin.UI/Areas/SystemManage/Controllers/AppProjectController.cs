using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Admin.UI.Controllers;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class AppProjectController : BaseController
    {
        // GET: SystemManage/AppProject
        public ActionResult Index()
        {
            return View();
        }
    }
}