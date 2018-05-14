using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using System.Linq;

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
                var entity = _systemAppSettingsRepository.Find<SystemAppSettingsPo>(e => e.IsValid == 1 && e.KeyWord == request.Key);

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
                int total = 0;
                var list = _systemAppSettingsRepository.FindPage<SystemAppSettingsPo>(e => e.IsValid == 1, o => o.Id,
                    request.PageIndex, request.PageSize, ref total, SqlSugar.OrderByType.Asc);

                if (list != null && list.Count > 0)
                {
                    response.EntityList = list.Select(e => e.As<SystemAppSettingsDto>()).ToList();
                }
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

            if (request.Entity == null)
            {
                response.IsSuccess = false;
                response.MessageCode = "1";
                response.MessageText = "系统配置不能为空";
                return response;
            }

            try
            {
                var entity = request.Entity.As<SystemAppSettingsPo>();
                if (entity.Id == 0)
                {
                    if (!_systemAppSettingsRepository.Any<SystemAppSettingsPo>(e => e.IsValid == 1 && e.KeyWord == entity.KeyWord))
                    {
                        EntityLogger.CreateEntity(entity);

                        _systemAppSettingsRepository.InsertReturnIdentity(entity);
                    }
                    else
                    {
                        response.IsSuccess = false;
                        response.MessageCode = "2";
                        response.MessageText = "Key已经存在";

                        return response;
                    }
                }
                else
                {
                    EntityLogger.UpdateEntity(entity);

                    _systemAppSettingsRepository.UpdateIgnoreColumns(entity, e => new
                    {
                        e.IsValid,
                        e.CreateUserId,
                        e.CreateTime
                    });
                }

                //更新缓存
                var cachekey = entity.KeyWord.AddCachePrefix("systemappsetting");
                CacheManager.RedisDefault.Del(cachekey);
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
                var entity = _systemAppSettingsRepository.Find<SystemAppSettingsPo>(e => e.IsValid == 1 && e.Id == request.Id);

                if (entity == null)
                {
                    throw new Exception("删除系统配置不存在");
                }

                EntityLogger.UpdateEntity(entity);
                entity.IsValid = 0;

                _systemAppSettingsRepository.UpdateColumns(entity, e => new
                {
                    e.UpdateUserId,
                    e.UpdateTime,
                    e.IsValid
                });
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