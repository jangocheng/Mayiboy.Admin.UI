using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Areas.SystemManage.Models;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
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
        [LoginAuth]
        public ActionResult Index()
        {
            ViewBag.SystemNavbar = QueryAllSystemNavbar();

            return View();
        }

        //查询系统菜单
        public ActionResult Query(string id)
        {
            try
            {

                var response = _systemMenuService.QueryAllMenu(new QueryAllMenuRequest
                {
                    NavbarId = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status=0, total = response.SystemMenuList.Count, rows = response.SystemMenuList });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }

        //查询树形系统菜单
        public ActionResult QueryTreeSysMenu(string id)
        {

            try
            {
                var list = new List<SelectSysMenuModel>();

                var response = _systemMenuService.QueryAllMenu(new QueryAllMenuRequest
                {
                    NavbarId = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                //list.Add(new SelectSysMenuModel
                //{
                //    id = 1,
                //    text = "系统"
                //});

                return ToJsonResult(response.SystemMenuList);
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询树形系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }

        //保存系统菜单
        public ActionResult Save(SysMenuModel model)
        {
            try
            {
                #region 赋值实体
                var entity = new SystemMenuDto
                {
                    Id = model.Id,
                    Pid = model.Pid,
                    Name = model.Name,
                    UrlAddress = model.UrlAddress,
                    NavbarId = model.NavbarId,
                    MenuType = model.MenuType,
                    Icon = "fa fa-navicon",//model.Icon,
                    Sort = model.Sort,
                    Remark = model.Remark
                };
                #endregion

                var response = _systemMenuService.SaveSystemMenu(new SaveSystemMenuRequest
                {
                    Entity = entity
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
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
                var response = _systemMenuService.DelSystemMenu(new DelSystemMenuRequest
                {
                    Id = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除系统菜单出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错！");
            }
        }


    }
}