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
    public class AppIdAuthService : BaseService, IAppIdAuthService
    {
        private readonly IAppIdAuthRepository _appIdAuthTokenRepository;

        public AppIdAuthService(IAppIdAuthRepository appIdAuthTokenRepository)
        {
            _appIdAuthTokenRepository = appIdAuthTokenRepository;
        }

        /// <summary>
        /// 保存应用授权Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveAppIdAuthResponse SaveAppIdAuthToken(SaveAppIdAuthRequest request)
        {
            var response = new SaveAppIdAuthResponse();

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "应用授权信息不存在";
                return response;
            }
            try
            {
                var entity = request.Entity.As<AppIdAuthPo>();
                if (entity.Id == 0)
                {
                    if (_appIdAuthTokenRepository.Any<AppIdAuthPo>(e => e.IsValid == 1 && e.AppId == entity.AppId))
                    {
                        response.IsSuccess = false;
                        response.MessageCode = "-1";
                        response.MessageText = "应用标识不能重复";
                        return response;
                    }
                    EntityLogger.CreateEntity(entity);

                    entity.Id = _appIdAuthTokenRepository.InsertReturnIdentity(entity);
                }
                else
                {
                    var entitytemp = _appIdAuthTokenRepository.FindSingle<AppIdAuthPo>(entity.Id);

                    if (entitytemp == null)
                    {
                        throw new Exception("更新应用授权信息不存在");
                    }
                    EntityLogger.UpdateEntity(entity);
                    _appIdAuthTokenRepository.UpdateIgnoreColumns(entity, e => new
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
                LogManager.LogicLogger.ErrorFormat("保存应用授权信息出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 查询应用授权列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryAppIdAuthResponse QueryAppIdAuth(QueryAppIdAuthRequest request)
        {
            var response = new QueryAppIdAuthResponse();
            try
            {
                int total = 0;

                var list = _appIdAuthTokenRepository.FindPage<AppIdAuthPo>(
                        e => e.IsValid == 1
                            && (SqlFunc.IsNullOrEmpty(request.ServiceAppId) || e.AppId.Contains(request.ServiceAppId))
                            && (SqlFunc.IsNullOrEmpty(request.AuthToken) || e.AuthToken.Contains(request.AuthToken)),
                        o => o.Id,
                        request.PageIndex, request.PageSize, ref total, OrderByType.Desc);

                response.EntityList = list.Select(e => e.As<AppIdAuthDto>()).ToList();

                response.TotalCount = total;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("查询应用授权列表出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除应用授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelAppIdAuthResponse DelAppIdAuth(DelAppIdAuthRequest request)
        {
            var response = new DelAppIdAuthResponse();
            try
            {
                var entity = _appIdAuthTokenRepository.Find<AppIdAuthPo>(e => e.IsValid == 1 && e.Id == request.Id);

                if (entity == null)
                {
                    throw new Exception("删除应用授权不存在");
                }

                entity.IsValid = 0;
                EntityLogger.UpdateEntity(entity);

                _appIdAuthTokenRepository.UpdateColumns(entity, (e) => new { e.IsValid, e.UpdateTime, e.UpdateUserId });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("删除应用授权出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public UpdateStatusResponse UpdateStatus(UpdateStatusRequest request)
        {
            var response = new UpdateStatusResponse();
            try
            {
                var entity = _appIdAuthTokenRepository.Find<AppIdAuthPo>(e => e.IsValid == 1 && e.Id == request.Id);

                if (entity == null)
                {
                    throw new Exception("修改应用授权不存在");
                }

                entity.Status = request.Status;
                EntityLogger.UpdateEntity(entity);

                _appIdAuthTokenRepository.UpdateColumns(entity, (e) => new { e.Status, e.UpdateTime, e.UpdateUserId });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("更新启用状态出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 根据应用标识AppId查询授权配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryByAppIdResponse QueryByAppId(QueryByAppIdRequest request)
        {
            var response = new QueryByAppIdResponse();
            try
            {
                var entity = _appIdAuthTokenRepository.Find<AppIdAuthPo>(e => e.IsValid == 1 && e.AppId == request.ServiceAppId);

                if (entity != null)
                {
                    response.Entity = entity.As<AppIdAuthDto>();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;
                LogManager.LogicLogger.ErrorFormat("根据应用标识AppId查询授权配置出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}