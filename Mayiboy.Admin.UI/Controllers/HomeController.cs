using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ISystemNavbarService _systemNavbarService;
        private readonly ISystemMenuService _systemMenuService;

        public HomeController(ISystemNavbarService systemNavbarService, ISystemMenuService systemMenuService)
        {
            _systemNavbarService = systemNavbarService;
            _systemMenuService = systemMenuService;
        }

        // GET: Home
        [LoginAuth]
        public ActionResult Index()
        {
            ViewBag.LogAccount = AccountConfig.LoginInfo;

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
                var list = new List<SystemMenuModel>();

                var res = _systemMenuService.QueryAllMenu(new QueryAllMenuRequest() { NavbarId = int.Parse(id) });

                if (res.IsSuccess && res.SystemMenuList != null)
                {
                    list = ToTree(res.SystemMenuList);
                }

                return ToJsonResult(new { status = 0, data = list });
            }
            catch (Exception ex)
            {
                return ToJsonFatalResult("系统出错", ex.ToString());
            }
        }


        private List<SystemMenuModel> ToTree(List<SystemMenuDto> list, int id = 0)
        {
            var treelist = new List<SystemMenuModel>();

            if (list == null || list.Count == 0) return null;

            if (list.Any(e => e.Pid == id))
            {
                treelist.AddRange(list.Where(e => e.Pid == id).Select(e => e.As<SystemMenuModel>()).OrderBy(e => e.Sort));
            }
            else
            {
                return null;
            }

            foreach (var item in treelist)
            {
                if (list.Any(o => o.Pid == item.Id))
                {
                    item.ChildNodes = ToTree(list, item.Id);
                }
            }

            return treelist;
        }

        //首页
        public ActionResult Page()
        {
            return View();
        }

        //介绍
        public ActionResult About()
        {
            return View();
        }
    }
}