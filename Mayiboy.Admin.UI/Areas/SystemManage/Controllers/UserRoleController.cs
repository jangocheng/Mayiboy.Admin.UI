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
    public class UserRoleController : BaseController
    {
        private readonly IUserRoleService _userRoleService;

        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }

        // GET: SystemManage/UserRole
        [LoginAuth]
        public ActionResult Index()
        {
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
        public ActionResult Save(string id, string name,string remark)
        {
            try
            {
                if (name.IsNullOrEmpty())
                {
                    return ToJsonErrorResult(1, "用户角色名不能为空");
                }

                var response = _userRoleService.SaveUserRole(new SaveUserRoleRequest
                {
                    Entity = new Model.Po.UserRolePo
                    {
                        Id = int.Parse(id),
                        Name = name,
                        Remark= remark
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
    }
}