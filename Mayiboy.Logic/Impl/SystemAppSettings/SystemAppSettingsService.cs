using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Logic.Impl
{
    public class SystemAppSettingsService : BaseService, ISystemAppSettingsService
    {
        private readonly ISystemAppSettingsRepository _systemAppSettingsRepository;

        public SystemAppSettingsService(ISystemAppSettingsRepository systemAppSettingsRepository)
        {
            _systemAppSettingsRepository = systemAppSettingsRepository;
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetSysAppSettingResponse GetSysAppSetting(GetSysAppSettingRequest request)
        {
            var response = new GetSysAppSettingResponse();
            try
            {
                var entity = _systemAppSettingsRepository.Find<SystemAppSettingsPo>(e => e.KeyWord == request.Key);

                if (entity != null)
                {
                    response.KeyValue = entity.KeyValue;
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("获取系统配置出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 查询系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QuerySysAppSettingResponse QuerySysAppSetting(QuerySysAppSettingRequest request)
        {
            var response = new QuerySysAppSettingResponse();
            try
            {

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("查询系统配置出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public SaveSysAppSettingResponse SaveSysAppSetting(SaveSysAppSettingReqeust request)
        {
            var response = new SaveSysAppSettingResponse();
            try
            {

            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("保存系统配置出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }

        /// <summary>
        /// 删除系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public DelSysAppSettingResponse DelSysAppSetting(DelSysAppSettingRequest request)
        {
            var response = new DelSysAppSettingResponse();
            try
            {
                var entity = _systemAppSettingsRepository.FindSingle<SystemAppSettingsPo>(request.Id);
                if (entity == null)
                {
                    throw new Exception("删除系统配置不存在");
                }

                entity.IsValid = 0;

                EntityLogger.UpdateEntity(entity);

                _systemAppSettingsRepository.UpdateIgnoreColumns<SystemAppSettingsPo>(entity, e => new { e.CreateUserId, e.CreateTime });
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.ToString();

                LogManager.LogicLogger.ErrorFormat("删除系统配置出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}