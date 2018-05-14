using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Areas.SystemManage.Models;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysMenuController : BaseController
    {
        private readonly ISystemNavbarService _systemNavbarService;//系统栏目
        private readonly ISystemMenuService _systemMenuService;//系统菜单
        private readonly IPermissionsService _permissionsService;//权限


        public SysMenuController(ISystemMenuService systemMenuService, IPermissionsService permissionsService, ISystemNavbarService systemNavbarService)
        {
            _systemMenuService = systemMenuService;
            _permissionsService = permissionsService;
            _systemNavbarService = systemNavbarService;
        }

        // GET: SystemManage/SysMenu
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

                return ToJsonResult(new { status = 0, total = response.SystemMenuList.Count, rows = response.SystemMenuList });
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
        [ActionAuth]
        [OperLog("保存系统菜单")]
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
        [ActionAuth]
        [OperLog("删除系统菜单")]
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

        //菜单权限
        public ActionResult QueryMenuPermissions(string id)
        {
            try
            {
                var response = _permissionsService.QueryPermissions(new QueryPermissionsRequest
                {
                    MenuId = int.Parse(id.IsNullOrEmpty() ? "0" : id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "查询菜单权限出错");
                }

                return ToJsonResult(new { code = 0, data = response.EntityList });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询菜单出错{0}", new { id, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("查询菜单权限出错");
            }
        }

        //保存菜单权限
        [ActionAuth]
        [OperLog("保存系统菜单操作权限")]
        public ActionResult SaveMenuPermissions(PermissionsModel model)
        {
            try
            {
                var entity = new PermissionsDto
                {
                    Id = model.Id,
                    MenuId = model.MenuId,
                    Name = model.Name,
                    Action = model.Action,
                    Code = model.Code,
                    Type = model.Type,
                    Remark = model.Remark
                };

                var response = _permissionsService.SavePermissions(new SavePermissionsRequest
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
                LogManager.DefaultLogger.ErrorFormat("保存菜单权限出错：{0}", new { model, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("保存菜单权限出错");
            }
        }

        //删除权限
        [ActionAuth]
        [OperLog("删除系统菜单操作权限")]
        public ActionResult DelPermissions(string id)
        {
            try
            {
                var response = _permissionsService.DelPermissions(new DelPermissionsRequest()
                {
                    Id = int.Parse(id.IsNullOrEmpty() ? "0" : id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除权限出错：{0}", new { id, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("删除权限出错！", ex.Message);
            }
        }

        //查询系统权限
        public ActionResult QuerySystemPermissions(string id)
        {
            try
            {
                var list = new List<object>();

                #region 系统菜单
                var resallsysmenu = _systemMenuService.QueryAllMenu(new QueryAllMenuRequest
                {
                    NavbarId = int.Parse(id)
                });

                if (resallsysmenu.IsSuccess)
                {
                    list.AddRange(resallsysmenu.SystemMenuList.Select(e => new
                    {
                        Id = e.Id,
                        _parentId = e.Pid,
                        Name = e.Name,
                        Remark = e.Remark
                    }));
                }
                #endregion

                return ToJsonResult(new { status = 0, total = 100, rows = list });

            }
            catch (Exception ex)
            {
                return ToJsonFatalResult(ex.Message);
            }
        }

        //获取权限Code
        public ActionResult GetPermissionsCode()
        {
            try
            {
                string code = DateTime.Now.ToString("PyyMMddmmss");

                return ToJsonResult(new { status = 0, code = code });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("获取权限Code出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("获取权限Code出错");
            }
        }
    }
}