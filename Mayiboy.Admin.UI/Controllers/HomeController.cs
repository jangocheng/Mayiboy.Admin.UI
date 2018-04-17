using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Contract;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISystemNavbarService _systemNavbarService;

        public HomeController(ISystemNavbarService systemNavbarService)
        {
            _systemNavbarService = systemNavbarService;
        }

        // GET: Home
        [LoginAuth]
        public ActionResult Index()
        {
            return View();
        }

        //获取系统导航栏
        public ActionResult SystemNavbar()
        {
            //获取系统栏目
            var list = new List<SystemNavbarModel>();

            var res = _systemNavbarService.QueryAll(new QueryAllNavbarRequest());

            if (res.IsSuccess && res.SystemNavbarList != null)
            {
                list = res.SystemNavbarList.Select(e => e.As<SystemNavbarModel>()).ToList();
            }

            ViewBag.SysNavbarList = list;

            return View();
        }

        //获取系统菜单
        public ActionResult GetSystemMenu(string id)
        {
            try
            {
                return ToJsonResult();
            }
            catch (Exception ex)
            {
                return ToFatalJsonResult("系统出错", ex.ToString());
            }
        }
    }
}