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
    public class SysLogController : BaseController
    {
        private readonly ISystemOperationLogService _systemOperationLogService;

        public SysLogController(ISystemOperationLogService systemOperationLogService)
        {
            _systemOperationLogService = systemOperationLogService;
        }

        [ActionAuth]
        public ActionResult Index()
        {
            return View();
        }

        //查询
        public ActionResult Query(int page = 1, int limit = 20)
        {
            try
            {
                var response = _systemOperationLogService.QueryOperSysLog(new QueryOperSysLogRequest
                {
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
                LogManager.DefaultLogger.ErrorFormat("查询系统日志出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("查询系统字典出错！");
            }
        }
    }
}