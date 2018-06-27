﻿using System;
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

				CacheManager.RunTimeCache.Set(ticket, response.UserInfoEntity, 60 * 5);

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
		[HttpPost]
		public ActionResult ValidateClientTicket(string ticket)
		{
			if (string.IsNullOrEmpty(ticket))
			{
				return Json(new { status = 1, msg = "参数有误" }, JsonRequestBehavior.AllowGet);
			}

			var entity = CacheManager.RunTimeCache.Get<UserInfoDto>(ticket);

			if (ticket.IsNullOrEmpty())
			{
				return Json(new { status = 2, msg = "参数无效" }, JsonRequestBehavior.AllowGet);
			}

			CacheManager.RunTimeCache.Remove(ticket);

			return Json(new { status = 0, entity }, JsonRequestBehavior.AllowGet);
		}
	}
}