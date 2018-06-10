using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.Admin.UI.Areas.SystemManage.Models;
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
        public ActionResult Save(AppIdAuthModel model)
        {
            try
            {
                var response = _appIdAuthService.SaveAppIdAuthToken(new SaveAppIdAuthRequest
                {
                    Entity = new AppIdAuthDto
                    {
                        Id = model.Id,
                        AppId = model.Appid,
                        AuthToken = model.Authtoken,
                        EncryptionType = model.EncryptionType,
                        Status = model.Status,
                        Remark = model.Remark
                    }
                });

                if (!response.IsSuccess)
                {
                    return ToJsonErrorResult(3, response.MessageText);
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存应用授权出错：{0}", new { model, err = ex.ToString() }.ToJson());
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

        //保存秘钥
        [ActionAuth]
        [OperLog("保存秘钥")]
        public ActionResult SaveSecretKey(string id, string secretKey, string privateKey, string publicKey)
        {
            try
            {
                var entity = new AppIdAuthDto
                {
                    Id = int.Parse(id),
                    SecretKey = secretKey,
                    PrivateKey = privateKey,
                    PublicKey = publicKey
                };

                var response = _appIdAuthService.SaveSecretKey(new SaveSecretKeyRequest
                {
                    Entity = entity
                });

                if (!response.IsSuccess)
                {
                    return ToJsonResult(new { status = 1, msg = "保存秘钥出错" });
                }

                return ToJsonResult(new { status = 0 });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("保存秘钥出错：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("保存秘钥出错");
            }
        }

        //获取秘钥
        public ActionResult GetSecretKey(string id)
        {
            try
            {
                var secretKey = "";
                var privateKey = "";
                var publicKey = "";

                var response = _appIdAuthService.GetAppIdAuth(new GetAppIdAuthRequest
                {
                    Id = int.Parse(id)
                });

                if (!response.IsSuccess)
                {
                    return ToJsonResult(new { status = 1, msg = "应用不存在" });
                }

                #region 根据加密类型获取加密字符串
                switch (response.Entity.EncryptionType)
                {
                    case 0:
                        break;
                    case 1://对称加密（DES）
                        secretKey = Guid.NewGuid().ToString("N").Substring(0, 32);
                        break;
                    case 2://对称加密（AES）
                        secretKey = Guid.NewGuid().ToString("N").Substring(0, 32);
                        break;
                    case 3://非对称加密
                        RsaCryption.RsaKey(out privateKey, out publicKey);
                        break;
                }
                #endregion

                return ToJsonResult(new
                {
                    status = 0,
                    SecretKey = secretKey,
                    PrivateKey = privateKey,
                    PublicKey = publicKey
                });
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("获取秘钥不存在：{0}", new { err = ex.ToString() }.ToJson());
                return ToJsonFatalResult("获取秘钥出错");
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