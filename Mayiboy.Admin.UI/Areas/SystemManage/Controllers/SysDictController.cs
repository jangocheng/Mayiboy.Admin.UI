using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Mayiboy.Admin.UI.Controllers;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Controllers
{
    public class SysDictController : BaseController
    {
        private readonly ISystemAppSettingsService _systemAppSettingsService;

        public SysDictController(ISystemAppSettingsService systemAppSettingsService)
        {
            _systemAppSettingsService = systemAppSettingsService;
        }

        // GET: SystemManage/SysDict
        [LoginAuth]
        public ActionResult Index()
        {
            return View();
        }

        //查询字典
        public ActionResult Query(string name, string key, string keyvalue, string remark, int page = 1, int limit = 20)
        {
            try
            {
                var response = _systemAppSettingsService.QuerySysAppSetting(new QuerySysAppSettingRequest
                {
                    Name = name,
                    Key = key,
                    Remark = remark,
                    KeyValue = keyvalue,
                    PageIndex = page,
                    PageSize = limit
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "查询出错");
                }

                return ToJsonResult(new { code = 0, data = response.EntityList, count = response.TotalCount });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("查询系统字典出错：{0}", new { name, key, remark, page = 1, limit, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("查询系统字典出错！");
            }
        }

        //保存字典
        public ActionResult Save(string id, string name, string key, string keyvalue, string remark)
        {
            try
            {
                var entity = new SystemAppSettingsDto
                {
                    Id = int.Parse(id.IsNullOrEmpty() ? "0" : id),
                    Name = name,
                    KeyWord = key,
                    KeyValue = keyvalue,
                    Remark = remark
                };

                var response = _systemAppSettingsService.SaveSysAppSetting(new SaveSysAppSettingReqeust
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
                LogManager.DefaultLogger.ErrorFormat("保存系统字典信息出错：{0}", new { err = ex.ToString() }.ToJson());
                return Json(new { status = 0, msg = "系统出错" }, JsonRequestBehavior.AllowGet);
            }
        }

        //删除字典
        public ActionResult Del(string id)
        {
            try
            {
                var response = _systemAppSettingsService.DelSysAppSetting(new DelSysAppSettingRequest
                {
                    Id = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "删除出错");
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("删除字典出错！{0}", new { id, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("删除出错");
            }
        }
    }
}