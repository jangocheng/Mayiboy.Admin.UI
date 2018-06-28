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
	public class UserAppIdAuthController : BaseController
	{
		private readonly IUserAppIdAuthService _userAppIdAuthService;
		private readonly IUserInfoService _userInfoService;

		public UserAppIdAuthController(IUserAppIdAuthService userRoleService, IUserInfoService userInfoService)
		{
			_userAppIdAuthService = userRoleService;
			_userInfoService = userInfoService;
		}


		public ActionResult Index()
		{
			var list = new List<UserInfoDto>();

			var response = _userInfoService.QueryUserInfoByName(new QueryUserInfoByNameRequest { });

			if (response.IsSuccess && response.EntityList != null)
			{
				list = response.EntityList;
			}

			ViewBag.UserInfoList = list;

			return View();
		}

		//查询
		public ActionResult Query(string username, string appid, int page = 1, int limit = 20)
		{
			try
			{
				var response = _userAppIdAuthService.QueryUserAppId(new QueryUserAppIdRequest
				{
					UserName = username,
					UserAppId = appid,
					PageIndex = page,
					PageSize = limit
				});

				if (!response.IsSuccess)
				{
					return ToJsonErrorResult(1, "查询出错", response.MessageText);
				}
				return Json(new { code = 0, data = response.EntityList, count = response.TotalCount }, JsonRequestBehavior.AllowGet);

			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.FatalFormat(new { username, appid, err = ex.ToString() }.ToJson());
				return ToJsonFatalResult("系统出错");
			}
		}


		[ActionAuth]
		[OperLog("保存用户授权AppId")]
		public ActionResult Save(string id, string userId, string appid, string remark)
		{
			try
			{
				if (userId.IsNullOrEmpty() || appid.IsNullOrEmpty())
				{
					return ToJsonErrorResult(1, "参数不能为空");
				}

				var response = _userAppIdAuthService.SaveUserAppId(new SaveUserAppIdRequest
				{
					Entity = new UserAppIdAuthDto
					{
						Id = int.Parse(id),
						UserId = int.Parse(userId),
						AppId = appid,
						Remark = remark
					}
				});

				if (!response.IsSuccess)
				{
					return ToJsonErrorResult(2, response.MessageText);
				}

				return ToJsonResult(new { status = 0 });
			}
			catch (Exception ex)
			{
				LogManager.DefaultLogger.FatalFormat(new { id, userId, appid, err = ex.ToString() }.ToJson());
				return ToJsonFatalResult("系统出错");
			}
		}

		//删除
		[ActionAuth]
		[OperLog("删除用户授权AppId")]
		public ActionResult Del(string id)
		{
			try
			{
				var response = _userAppIdAuthService.DelUserAppId(new DelUserAppIdRequest
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
				LogManager.DefaultLogger.ErrorFormat(new { id, err = ex.ToString() }.ToJson());
				return ToJsonErrorResult(-1, "删除出错");
			}
		}

	}
}