﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.Utils;

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
				LogManager.DefaultLogger.ErrorFormat("登录用户名出错：{0}", new { request, err = ex.ToString() }.ToJson());
			}

			return response;
		}

		/// <summary>
		/// 根据用户Id查询用户信息
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost]
		public QueryUserInfoByIdResponse QueryUserInfoById(QueryUserInfoByIdRequest request)
		{
			return _userinfoService.QueryUserInfoById(request);
		}
	}
}
