using System;
using System.Linq;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using SqlSugar;

namespace Mayiboy.Logic.Impl
{
	public class UserApIdAuthService : BaseService, IUserAppIdAuthService
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
		public QueryByUserAppIdResponse QueryByUserAppId(QueryByUserAppIdRequest request)
		{
			var response = new QueryByUserAppIdResponse();

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

		/// <summary>
		/// 查询用户授权Id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public QueryUserAppIdResponse QueryUserAppId(QueryUserAppIdRequest request)
		{
			var response = new QueryUserAppIdResponse();

			try
			{
				int total = 0;

				var list = _userApIdAuthRepository.QueryUserAppId(request.UserName, request.UserAppId, request.PageIndex, request.PageSize, ref total);

				if (list != null)
				{
					response.EntityList = list;
				}

				response.TotalCount = total;
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

		/// <summary>
		/// 保存用户授权Id
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public SaveUserAppIdResponse SaveUserAppId(SaveUserAppIdRequest request)
		{
			var response = new SaveUserAppIdResponse();

			if (request.Entity == null)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = "参数不能为空";
				return response;
			}

			try
			{
				var entity = request.Entity.As<UserAppIdAuthPo>();

				if (entity.Id == 0)
				{
					if (_userApIdAuthRepository.Any<UserAppIdAuthPo>(e => e.IsValid == 1 && e.UserId == entity.UserId && e.AppId == entity.AppId))
					{
						response.IsSuccess = false;
						response.MessageCode = "-1";
						response.MessageText = "不能重复授权";
						return response;
					}

					EntityLogger.CreateEntity(entity);

					_userApIdAuthRepository.Insert(entity);
				}
				else
				{
					var entitytemp = _userApIdAuthRepository.Find<UserAppIdAuthPo>(e => e.IsValid == 1 && e.Id == entity.Id);;

					if (entitytemp == null)
					{
						response.IsSuccess = false;
						response.MessageCode = "-1";
						response.MessageText = "更新不存在";
						return response;
					}

					EntityLogger.UpdateEntity(entity);

					_userApIdAuthRepository.UpdateIgnoreColumns(entity, e => new
					{
						e.IsValid,
						e.CreateTime,
						e.CreateUserId,
					});
				}

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

		/// <summary>
		/// 删除用户授权AppId
		/// </summary>
		/// <param name="request">参数</param>
		/// <returns></returns>
		public DelUserAppIdResponse DelUserAppId(DelUserAppIdRequest request)
		{
			var response = new DelUserAppIdResponse();

			try
			{
				var entity = _userApIdAuthRepository.Find<UserAppIdAuthPo>(e => e.IsValid == 1 && e.Id == request.Id);

				if (entity == null)
				{
					throw new Exception("删除授权不存在");
				}

				EntityLogger.UpdateEntity(entity);
				entity.IsValid = 0;

				_userApIdAuthRepository.UpdateColumns(entity, e => new
				{
					e.IsValid,
					e.UpdateTime,
					e.UpdateUserId
				});
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