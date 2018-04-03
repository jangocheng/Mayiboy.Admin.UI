using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserInfoService _iuserinfoservice;

        public AccountController(IUserInfoService iuserinfoservice)
        {
            _iuserinfoservice = iuserinfoservice;
        }

        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        #region 登录
        public ActionResult Login(LoginUserInfoModel model)
        {
            try
            {
                #region 验证验证码
                var vcode = SessionHelper.Get<string>("vcode");
                if (vcode.IsNullOrEmpty() || vcode != model.Code)
                {
                    return Json(new { status = 1, msg = "验证码错误" });
                }
                #endregion

                var request = new LoginQueryRequest
                {
                    LoginName = model.UserName,
                    Password = model.PassWord
                };

                var loginqueryresponse = _iuserinfoservice.LoginQuery(request);

                if (loginqueryresponse.UserInfoEntity == null)
                {
                    return Json(new { status = 2, msg = "密码错误" }, JsonRequestBehavior.AllowGet);
                }

                #region 保存用户登录状态
                string identityValue = Guid.NewGuid().ToString("N");

                CookieHelper.Set(PublicConst.IdentityCookieKey, identityValue, true);

                var entity = loginqueryresponse.UserInfoEntity.As<AccountModel>();

                entity.Fingerprint = RequestHelper.Fingerprint;

                CacheManager.RedisDefault.Set(identityValue.AddCachePrefix(PublicConst.IdentityCookieKey), entity, PublicConst.Time.Hour1);
                #endregion

                return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogManager.DefaultLogger.ErrorFormat("登录出错：{0}", new { model, err = ex.ToString() }.ToJson());
                return Json(new { status = -1, msg = "系统出错！" }, JsonRequestBehavior.AllowGet);
            }
        }
        #endregion

        #region 注销登录
        public ActionResult Logout()
        {

            return Redirect("Index");
        }
        #endregion

        #region 验证码
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public ActionResult VerifyCode()
        {
            string vcode = CaptchaHelper.CreateRandomCode(4);

            SessionHelper.Set("vcode", vcode);

            return File(CaptchaHelper.DrawImage(vcode), @"image/jpeg");
        }
        #endregion

    }
}