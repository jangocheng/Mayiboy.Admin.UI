using System;
using System.Linq;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using SqlSugar;

namespace Mayiboy.Logic.Impl
{
	public class AppProjectService : BaseService, IAppProjectService
	{
		private readonly IAppProjectRepository _appProjectRepository;

		public AppProjectService(IAppProjectRepository appProjectRepository)
		{
			_appProjectRepository = appProjectRepository;
		}

		/// <summary>
		/// 分页查询应用项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public QueryAppProjectResponse QueryAppProject(QueryAppProjectRequest request)
		{
			var response = new QueryAppProjectResponse();

			try
			{
				int total = 0;

				var list = _appProjectRepository.FindPage<AppProjectPo>(
					e => e.IsValid == 1 && (SqlFunc.IsNullOrEmpty(request) || e.ProjectName.Contains(request.ProjectName)),
					o => o.Id, request.PageIndex, request.PageSize, ref total, OrderByType.Desc);

				if (list != null)
				{
					response.EntityList = list.Select(e => e.As<AppProjectDto>()).ToList();
				}

			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = ex.Message;
				LogManager.LogicLogger.ErrorFormat("");
			}

			return response;
		}

		/// <summary>
		/// 保存应用项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public SaveAppProjectResponse SaveAppProject(SaveAppProjectRequest request)
		{
			var response = new SaveAppProjectResponse();

			if (request.Entity == null)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = "参数不能为空";
				return response;
			}
			try
			{
				var entity = request.Entity.As<AppProjectPo>();

				if (entity.Id == 0)
				{
					if (_appProjectRepository.Any<AppProjectPo>(e => e.IsValid == 1 && e.ApplicationId == entity.ApplicationId))
					{
						response.IsSuccess = false;
						response.MessageCode = "2";
						response.MessageText = "应用Id已存在";
						return response;
					}

					EntityLogger.CreateEntity(entity);
					response.Id = _appProjectRepository.InsertReturnIdentity(entity);
				}
				else
				{
					var entitytemp = _appProjectRepository.Find<AppProjectPo>(e => e.IsValid == 1 && e.Id == entity.Id);

					if (entitytemp == null)
					{
						throw new Exception("更新项目不存在");
					}

					EntityLogger.UpdateEntity(entity);
					_appProjectRepository.UpdateIgnoreColumns(entity, e => new
					{
						e.IsValid,
						e.CreateUserId,
						e.UpdateTime
					});
				}
			}
			catch (Exception ex)
			{
				response.IsSuccess = false;
				response.MessageCode = "-1";
				response.MessageText = ex.Message;
				LogManager.LogicLogger.ErrorFormat("");
			}

			return response;
		}

		/// <summary>
		/// 删除应用项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public DelAppProjectResponse DelAppProject(DelAppProjectRequest request)
		{
			var response = new DelAppProjectResponse();

			try
			{
				var entity = _appProjectRepository.Find<AppProjectPo>(e => e.IsValid == 1 && e.Id == request.Id);

				if (entity == null)
				{
					throw new Exception("删除应用项目不存在");
				}
				EntityLogger.UpdateEntity(entity);
				entity.IsValid = 0;

				_appProjectRepository.UpdateColumns(entity, e => new
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
				LogManager.LogicLogger.ErrorFormat("");
			}

			return response;
		}
	}
}