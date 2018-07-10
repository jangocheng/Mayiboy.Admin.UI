using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Framework.Mayiboy.Utility;
using Framework.Mayiboy.Utility.EncryptionHelper;
using Mayiboy.ConstDefine;
using Mayiboy.Contract;
using Mayiboy.Utils;

namespace Mayiboy.Admin.UI.Controllers
{
	/// <summary>
	/// 单点登录
	/// </summary>
	public class LoginController : Controller
	{
		private readonly IUserInfoService _iuserinfoservice;
		private readonly IUserAppIdAuthService _userApIdAuthService;

		public LoginController(IUserInfoService iuserinfoservice, IUserAppIdAuthService userApIdAuthService)
		{
			_iuserinfoservice = iuserinfoservice;
			_userApIdAuthService = userApIdAuthService;
		}

		//统一登陆页面
		public ActionResult Index(string fromurl, string fromappid)
		{
			if (string.IsNullOrEmpty(fromurl) || string.IsNullOrEmpty(fromappid))
			{
				return Content("参数有误");
			}

			SessionHelper.Set<string>("fromappid", fromappid);
			SessionHelper.Set<string>("fromurl", fromurl);

			return View();
		}

		//登陆
		public ActionResult Submit(string username, string password)
		{
			try
			{
				#region 验证参数

				if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
				{
					return Json(new { status = 1, msg = "请正确输入用户名、密码！" }, JsonRequestBehavior.AllowGet);
				}

				var fromappid = SessionHelper.Get<string>("fromappid");
				var fromurl = SessionHelper.Get<string>("fromurl");

				if (string.IsNullOrEmpty(fromurl) || string.IsNullOrEmpty(fromappid))
				{
					return Json(new { status = 3, msg = "参数有误" }, JsonRequestBehavior.AllowGet);
				}
				#endregion

				username = RsaCryption.Decrypt(PublicConst.XmlPrivateKey, username);
				password = RsaCryption.Decrypt(PublicConst.XmlPrivateKey, password);

				#region 验证码登陆错误次数


				var loginkey = username + RequestHelper.Ip;
				var loginnum = int.Parse(CacheManager.RunTimeCache.Get(loginkey) ?? "0");

				if (loginnum >= ConfigHelper.GetConfigInt("MaxNumberErrorLogin"))
				{
					return Json(new { status = 5, msg = "错误登陆次数超过上限" }, JsonRequestBehavior.AllowGet);
				}
				#endregion

				#region 验证用户名密码


				var response = _iuserinfoservice.LoginQuery(new LoginQueryRequest
				{
					LoginName = username,
					Password = password.GetMd5()
				});

				if (!response.IsSuccess)
				{
					return Json(new { status = 2, msg = "登录失败" }, JsonRequestBehavior.AllowGet);
				}

				if (response.UserInfoEntity == null)
				{
					CacheManager.RunTimeCache.Set(loginkey, (loginnum + 1).ToString(), PublicConst.Time.Day1);
					return Json(new { status = 3, msg = "密码错误！" }, JsonRequestBehavior.AllowGet);
				}
				CacheManager.RunTimeCache.Remove(loginkey);
				#endregion

				#region 验证fromId
				var userappidauthresponse = _userApIdAuthService.QueryByUserAppId(new QueryByUserAppIdRequest
				{
					UserId = response.UserInfoEntity.Id,
					UserAppId = fromappid,
				});

				if (!userappidauthresponse.IsSuccess)
				{
					return Json(new { status = 6, msg = "登录失败" }, JsonRequestBehavior.AllowGet);
				}

				if (userappidauthresponse.Entity == null)
				{
					return Json(new { status = 7, msg = "您没有权限!请联系管理员" }, JsonRequestBehavior.AllowGet);
				}
				#endregion

				var ticket = Guid.NewGuid().ToString("N");

				CacheManager.RunTimeCache.Set(fromappid, ticket, 60 * 5);

				var entity = response.UserInfoEntity.As<AccountModel>();

				entity.Fingerprint = RequestHelper.Fingerprint;

				CacheManager.RunTimeCache.Set(ticket, entity, 60 * 5);

				fromurl = string.Concat(fromurl, (fromurl.Contains("?") ? "&" : "?"), PublicConst.UrlAuth, "=", ticket);

				return Json(new { status = 0, fromurl = fromurl }, JsonRequestBehavior.AllowGet);
			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.Error(new { status = -1, msg = "登录失败" }.ToJson());
				return Json(new { username, password, err = ex.ToString() }, JsonRequestBehavior.AllowGet);
			}
		}

		//验证凭证
		public ActionResult ValidateClientTicket(string appid, string ticket, string encrypt)
		{
			#region 验证参数的有效性
			if (string.IsNullOrEmpty(ticket) || string.IsNullOrEmpty(appid) || encrypt.IsNullOrEmpty())
			{
				return Json(new { status = 1, data = "参数有误" }, JsonRequestBehavior.AllowGet);
			}

			var cacheticket = CacheManager.RunTimeCache.Get(appid);

			if (cacheticket.IsNullOrEmpty())
			{
				return Json(new { status = 2, data = "参数有误" }, JsonRequestBehavior.AllowGet);
			}

			if (cacheticket != ticket)
			{
				return Json(new { status = 3, data = "参数有误" }, JsonRequestBehavior.AllowGet);
			}

			if (!ValidateEncrypt(ticket, encrypt))
			{
				return Json(new { status = 4, data = "非法访问" });
			}

			#endregion

			var entity = CacheManager.RunTimeCache.Get<AccountModel>(ticket);

			return Json(new { status = 0, data=entity }, JsonRequestBehavior.AllowGet);
		}

		/// <summary>
		/// 验证Encrypt
		/// </summary>
		/// <param name="encrypt"></param>
		/// <returns></returns>
		public bool ValidateEncrypt(string ticket, string encrypt)
		{
			var list = Request.QueryString.AllKeys.Where(e => e != "encrypt").Select(e => string.Format("{0}={1}", e, Request.QueryString.Get(e))).ToList();

			var secretKey = ConfigHelper.GetString("ValidateTicket.SecretKey");

			list.Sort();

			StringBuilder stringBuilder = new StringBuilder();
			foreach (string current in list)
			{
				stringBuilder.Append(current);
			}

			string password = stringBuilder.ToString() + secretKey;

			return password.GetMd5() == encrypt;
		}
	}
}