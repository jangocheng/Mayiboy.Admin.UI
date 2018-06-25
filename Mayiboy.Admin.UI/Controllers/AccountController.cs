using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Model.Model;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserInfoService _iuserinfoservice;
		private readonly ISystemOperationLogService _systemOperationLogService;


		public AccountController(IUserInfoService iuserinfoservice, ISystemOperationLogService systemOperationLogService)
		{
			_iuserinfoservice = iuserinfoservice;
			_systemOperationLogService = systemOperationLogService;
		}

		// GET: Account
		public ActionResult Index()
		{
			return View();
		}

		#region 用户登录
		public ActionResult Login(LoginUserInfoModel model)
		{
			try
			{
				#region 验证验证码登陆错误次数
				var loginkey = model.UserName + RequestHelper.Ip;
				var loginnum = int.Parse(CacheManager.RunTimeCache.Get(loginkey) ?? "0");

				if (loginnum >= ConfigHelper.GetConfigInt("MaxNumberErrorLogin"))
				{
					return Json(new { status = 5, msg = "错误登陆次数超过上限" }, JsonRequestBehavior.AllowGet);
				}
				#endregion

				#region 验证验证码
				var vcode = SessionHelper.Get<string>("vcode");
				if (vcode.IsNullOrEmpty() || vcode != model.Code)
				{
					return Json(new { status = 1, msg = "验证码错误" });
				}
				SessionHelper.Remove("vcode");
				#endregion

				var request = new LoginQueryRequest
				{
					LoginName = model.UserName,
					Password = model.PassWord.GetMd5()
				};

				var loginqueryresponse = _iuserinfoservice.LoginQuery(request);

				if (loginqueryresponse.UserInfoEntity == null)
				{
					//记录ip地址、用户名登陆次数
					CacheManager.RunTimeCache.Set(loginkey, (loginnum + 1).ToString(), PublicConst.Time.Day1);
					return Json(new { status = 2, msg = "密码错误" }, JsonRequestBehavior.AllowGet);
				}

				#region 保存用户登录状态
				string identityValue = Guid.NewGuid().ToString("N");

				CookieHelper.Set(PublicConst.IdentityCookieKey, identityValue, true);

				var entity = loginqueryresponse.UserInfoEntity.As<AccountModel>();

				entity.Fingerprint = RequestHelper.Fingerprint;

				var key = identityValue.AddCachePrefix(PublicConst.IdentityCookieKey);

				CacheManager.RedisDefault.Set(key, entity, PublicConst.Time.Hour1);

				#region 只允许一个客户端登录
				//var k = entity.Id.ToString().AddCachePrefix("LoginBind");

				//var v = CacheManager.RedisDefault.Get(k);

				//if (!string.IsNullOrEmpty(v))
				//{
				//	CacheManager.RedisDefault.Remove(v.AddCachePrefix(PublicConst.IdentityCookieKey));
				//}

				//CacheManager.RedisDefault.Set(k, identityValue, PublicConst.Time.Hour1); 
				#endregion

				#region 记录用户操作日志
				_systemOperationLogService.AddOperationLog(new AddOperationLogRequest
				{
					Content = string.Format("[LoginName:{0}]-[Name:{1}]-[Content:{2}]", entity.LoginName, entity.Name, "用户登录")
				});
				#endregion

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
		[LoginAuth]
		[OperLog("注销登录")]
		public ActionResult Logout()
		{
			SessionHelper.Clear();

			//删除登录信息
			var keyl = CookieHelper.Get(PublicConst.IdentityCookieKey).AddCachePrefix(PublicConst.IdentityCookieKey);
			CacheManager.RedisDefault.Del(keyl);

			CookieHelper.Remove(PublicConst.IdentityCookieKey);

			//删除权限
			var keyp = LoginAccount.Identity.AddCachePrefix("userpermission");
			CacheManager.RedisDefault.Del(keyp);

			return Redirect("Index");
		}
		#endregion

		#region 修改用户密码
		/// <summary>
		/// 修改用户密码
		/// </summary>
		/// <returns></returns>
		[LoginAuth]
		[OperLog("修改用户密码")]
		public ActionResult ChangePassword(string oldpwd, string newpwd)
		{
			try
			{
				var response = _iuserinfoservice.ChangePassword(new ChangePasswordRequest
				{
					UserId = LoginAccount.UserInfo.Id,
					OldPassword = oldpwd,
					NewPassword = newpwd
				});

				if (!response.IsSuccess)
				{
					return Json(new { status = 1, msg = response.MessageText }, JsonRequestBehavior.AllowGet);
				}

				return Json(new { status = 0 }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.ErrorFormat("修改用户密码出错{0}", new { oldpwd, newpwd, err = ex.ToString() }.ToJson());
				return Json(new { status = -1, msg = "系统出错！" }, JsonRequestBehavior.AllowGet);
			}

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

		//用户信息页面
		public ActionResult UserInfoPage()
		{
			return View();
		}


		//保存用户基本信息
		public ActionResult SaveUserInfo()
		{
			return Json(new { }, JsonRequestBehavior.AllowGet);
		}

	}
}