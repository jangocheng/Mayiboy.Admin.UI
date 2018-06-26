using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Logic.Impl
{
	public class UserApIdAuthService : BaseService, IUserApIdAuthService
	{

		private readonly IUserAppIdAuthRepository _userApIdAuthRepository;

		public UserApIdAuthService(IUserAppIdAuthRepository userApIdAuthRepository)
		{
			_userApIdAuthRepository = userApIdAuthRepository;
		}

		/// <summary>
		/// 查询用户授权AppId
		/// </summary>
		/// <param name="request">参数</param>
		/// <returns></returns>
		public QueryUserAppIdResponse QueryUserAppId(QueryUserAppIdRequest request)
		{
			var response = new QueryUserAppIdResponse();

			try
			{
				var entity = _userApIdAuthRepository.Find<UserAppIdAuthPo>(
					e => e.IsValid == 1 && e.UserId == request.UserId && e.AppId == request.UserAppId);

				response.Entity = entity.As<UserAppIdAuthDto>();
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = ex.Message;
				LogManager.LogicLogger.ErrorFormat(new { request, err = ex.ToString() }.ToJson());
			}

			return response;
		}
	}
}