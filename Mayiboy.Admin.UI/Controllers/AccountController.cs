using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
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
                var res = _iuserinfoservice.LoginQuery(new LoginQueryRequest
                {
                    LoginName = model.UserName,
                    Password = model.PassWord
                });

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

        #region 解密 js加密
        /// <summary>
        /// 解密 js加密
        /// </summary>
        /// <param name="ciphertext">密文</param>
        private void RsaDecrypt(ref string ciphertext)
        {
            if (ciphertext.IsNotNullOrEmpty())
            {
                ciphertext = RsaCryption.RsaDecrypt(PublicConst.XmlPrivateKey, ciphertext);
            }
        }
        #endregion
    }
}