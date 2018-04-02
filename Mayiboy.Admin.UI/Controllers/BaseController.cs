using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Ioc;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;

namespace Mayiboy.Admin.UI.Controllers
{
    public class BaseController : Controller
    {
        private readonly IUserInfoService _iuserinfoservice;

        public BaseController(IUserInfoService iuserinfoservice)
        {
            _iuserinfoservice = iuserinfoservice;
        }

        public BaseController()
        {

        }

        public ActionResult test()
        {

            _iuserinfoservice.Insert(new InsertRequest());

            return Content("ok");
        }
    }
}