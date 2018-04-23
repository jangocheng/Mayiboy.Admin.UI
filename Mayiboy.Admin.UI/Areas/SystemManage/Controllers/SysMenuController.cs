using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysMenuController : BaseController
    {
        private readonly ISystemMenuService _systemMenuService;

        public SysMenuController(ISystemMenuService systemMenuService)
        {
            _systemMenuService = systemMenuService;
        }

        // GET: SystemManage/SysMenu
        public ActionResult Index()
        {
            ViewBag.SystemNavbar = QueryAllSystemNavbar();

            return View();
        }

        //查询系统菜单
        public ActionResult Query()
        {
            try
            {

                return ToJsonResult(new { });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }

        //保存系统菜单
        public ActionResult Save()
        {
            try
            {
                return ToJsonResult(new { });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }

        //删除系统菜单
        public ActionResult Del(string id)
        {
            try
            {

                return ToJsonResult(new { });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }


    }
}