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
    public class DepartmentController : BaseController
    {
        private readonly IDepartmentService _departmentService;

        public DepartmentController(IDepartmentService departmentService)
        {
            _departmentService = departmentService;
        }

        // GET: SystemManage/Department
        public ActionResult Index()
        {
            return View();
        }

        //查询部门
        public ActionResult Query()
        {
            try
            {
                var response = _departmentService.QueryAllDepartment(new QueryAllDepartmentRequest { });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "查询出错");
                }

                return ToJsonResult(new { status = 0, rows = response.List });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询部门出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("查询部门出错", ex.ToString());
            }
        }

        //保存部门信息
        [ActionAuth]
        [OperLog("保存部门信息")]
        public ActionResult Save(string id, string pid, string name, string remark)
        {
            try
            {
                var entity = new DepartmentDto
                {
                    Id = int.Parse(id.IsNullOrEmpty() ? "0" : id),
                    Pid = int.Parse(pid.IsNullOrEmpty() ? "0" : pid),
                    Name = name,
                    Remark = remark
                };

				if (entity.Id == entity.Pid)
				{
					return ToJsonErrorResult(1, "父级菜单不能设置成自己");
				}

				var response = _departmentService.SaveDeparment(new SaveDepartmentRequest
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
                LogManager.DefaultLogger.ErrorFormat("保存部门信息出错：{0}", new { id, pid, name, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("保存部门信息出错");
            }
        }

        //删除部门信息
        [ActionAuth]
        [OperLog("删除部门信息")]
        public ActionResult Del(string id)
        {
            try
            {
                var response = _departmentService.DelDeparment(new DelDepartamentRequest
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
                LogManager.DefaultLogger.ErrorFormat("删除部门信息出错：{0}", new { id, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("系统出错");
            }
        }
    }
}