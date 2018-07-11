using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.Mayiboy.Ioc;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.Logic.Impl;

namespace Mayiboy.Host.Controllers
{
	/// <summary>
	/// 获取用户信息接口
	/// </summary>
	public class UserInfoController : ApiController
	{
		private readonly IUserInfoService _userinfoService;

		public UserInfoController(IUserInfoService userInfoService)
		{
			_userinfoService = userInfoService;
		}

		/// <summary>
		/// 查询用户基本信息
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public LoginQueryResponse LoginQuery(LoginQueryRequest request)
		{
			var response = new LoginQueryResponse();

			try
			{
				if (request == null || request.LoginName.IsNullOrEmpty() || request.Password.IsNullOrEmpty())
				{
					response.IsSuccess = false;
					response.MessageCode = "1";
					response.MessageText = "参数不能为空";
					return response;
				}

				request.Password = request.Password.GetMd5();


				response = _userinfoService.LoginQuery(request);
			}
			catch (System.Exception ex)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = "系统出错";
			}

			return response;
		}

		[HttpPost]
		public DelUserInfoResponse DelUserInfo(DelUserInfoRequest request)
		{
			var response = new DelUserInfoResponse();
			return response;
		}
	}
}
