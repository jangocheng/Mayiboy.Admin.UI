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
    public class AppIdAuthController : BaseController
    {
        private readonly IAppIdAuthService _appIdAuthService;

        public AppIdAuthController(IAppIdAuthService appIdAuthService)
        {
            _appIdAuthService = appIdAuthService;
        }

        // GET: SystemManage/AppIdAuth
        public ActionResult Index()
        {
            return View();
        }

        //分页查询应用授权配置列表
        public ActionResult Query(string appid, string authtoken, int page = 1, int limit = 20)
        {
            try
            {
                var response = _appIdAuthService.QueryAppIdAuth(new QueryAppIdAuthRequest
                {
                    ServiceAppId = appid,
                    AuthToken = authtoken,
                    PageIndex = page,
                    PageSize = limit
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, "查询出错", response.MessageText);
                }
                return ToJsonResult(new { code = 0, data = response.EntityList, count = response.TotalCount });
            }
            catch (Exception)
            {
                return ToJsonFatalResult("查询应用授权出错");
            }
        }

        //保存应用授权配置
        [ActionAuth]
        [OperLog("保存应用授权配置")]
        public ActionResult Save(string id, string appid, string authtoken, string status, string remark)
        {
            try
            {
                if (appid.IsNullOrEmpty())
                {
                    return ToJsonErrorResult(1, "应用标识不能为空");
                }

                if (authtoken.IsNullOrEmpty())
                {
                    return ToJsonErrorResult(2, "授权Token不能为空");
                }

                var response = _appIdAuthService.SaveAppIdAuthToken(new SaveAppIdAuthRequest
                {
                    Entity = new AppIdAuthDto
                    {
                        Id = int.Parse(id),
                        AppId = appid,
                        AuthToken = authtoken,
                        Status = int.Parse(status),
                        Remark = remark
                    }
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(3, response.MessageText);
                }
                return ToJsonResult(new { status = 0 });
            }
            catch (Exception)
            {
                return ToJsonFatalResult("保存应用授权出错");
            }
        }

        //删除应用授权
        [ActionAuth]
        [OperLog("删除应用授权配置")]
        public ActionResult Del(string id)
        {
            try
            {
                var response = _appIdAuthService.DelAppIdAuth(new DelAppIdAuthRequest
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
                LogManager.DefaultLogger.ErrorFormat("删除应用标识出错：{0}", new { id, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("删除应用标识出错");
            }
        }

        //删除应用授权
        [ActionAuth]
        [OperLog("更新应用标识状态")]
        public ActionResult UpdateStatus(string id, string status)
        {
            try
            {
                var response = _appIdAuthService.UpdateStatus(new UpdateStatusRequest
                {
                    Id = int.Parse(id),
                    Status = int.Parse(status)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(1, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("更新应用授权配置出错：{0}", new { id, status, err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("更新应用授权配置状态出错");
            }
        }


        //获取授权Token
        public ActionResult GetAuthToken()
        {
            try
            {
                string authtoken = Guid.NewGuid().ToString("N");

                return ToJsonResult(new { status = 0, content = authtoken });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("获取授权Token出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("获取授权Token");
            }
        }
    }
}