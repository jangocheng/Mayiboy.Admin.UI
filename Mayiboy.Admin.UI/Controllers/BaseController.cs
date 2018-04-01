using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;

namespace Mayiboy.Admin.UI.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult test()
        {
            IUserInfoService iuserinfoservice = new UserInfoService();

            iuserinfoservice.Insert(new InsertRequest());

            return Content("ok");
        }
    }
}