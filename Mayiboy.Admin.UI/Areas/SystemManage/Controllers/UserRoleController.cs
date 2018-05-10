using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;
        private readonly IPermissionsService _permissionsService;//权限

        public UserRoleController(IUserRoleService userRoleService, IPermissionsService permissionsService)
        {
            _userRoleService = userRoleService;
            _permissionsService = permissionsService;
        }

        // GET: SystemManage/UserRole
        public ActionResult Index()
        {
            ViewBag.SystemNavbar = QueryAllSystemNavbar();
            return View();
        }

        //查询用户角色
        public ActionResult Query(string name, int page = 1, int limit = 20)
        {
            try
            {
                var response = _userRoleService.QueryUserRole(new QueryUserRoleRequest
                {
                    Name = name,
                    PageIndex = page,
                    PageSize = limit
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "查询出错", response.MessageText);
                }
                return Json(new { code = 0, data = response.List, count = response.TotalCount }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询用户角色信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存用户角色信息
        public ActionResult Save(string id, string name, string remark)
        {
            try
            {
                if (name.IsNullOrEmpty())
                {
                    return ToJsonErrorResult(1, "用户角色名不能为空");
                }

                var response = _userRoleService.SaveUserRole(new SaveUserRoleRequest
                {
                    Entity = new UserRolePo
                    {
                        Id = int.Parse(id),
                        Name = name,
                        Remark = remark
                    }
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存用户角色信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //查询角色权限
        public ActionResult QueryRolePermissions(string id, string navbarid, string menuid, string roleid)
        {
            try
            {
                var rolelist = new List<RolePermissionsJoinDto>();

                var response = _permissionsService.QueryPermissions(new QueryPermissionsRequest
                {
                    MenuId = int.Parse(id.IsNullOrEmpty() ? "0" : id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                var rolemenuresponse = _userRoleService.QueryRoleMenuPermissions(new QueryRoleMenuPermissionsRequest()
                {
                    NavbarId = int.Parse(navbarid),
                    MenuId = int.Parse(menuid),
                    RoleId = int.Parse(roleid)
                });

                if (rolemenuresponse.IsSuccess)
                {
                    rolelist = rolemenuresponse.EntityList;
                }

                var list = response.EntityList.Select(e => new
                {
                    LAY_CHECKED = rolelist.Any(o => o.PermissionsId == e.Id),
                    e.Id,
                    e.Name,
                    e.Remark
                });


                return ToJsonResult(new { code = 0, data = list });
            }
            catch (Exception ex)
            {
                return ToJsonFatalResult("系统出错");
            }
        }

        //删除用户角色
        public ActionResult Del(string id)
        {
            try
            {
                var response = _userRoleService.DelUserRole(new DelUserRoleRequest
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
                LogManager.DefaultLogger.ErrorFormat("删除用户角色出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //保存角色权限
        public ActionResult SaveRolePermissions(string nvabarid, string menuid, string roleid, List<int> pid)
        {
            try
            {
                var response = _userRoleService.SaveRolePermissions(new SaveRolePermissionsRequest
                {
                    NavbarId = int.Parse(nvabarid),
                    MenuId = int.Parse(menuid),
                    RoleId = int.Parse(roleid),
                    PermissionsIds = pid
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存角色权限出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错");
            }
        }
    }
}