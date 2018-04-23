using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    public interface ISystemAppSettingsService : IBaseService, IDependency
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetSysAppSettingResponse GetSysAppSetting(GetSysAppSettingRequest request);

        /// <summary>
        /// 查询系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QuerySysAppSettingResponse QuerySysAppSetting(QuerySysAppSettingRequest request);

        /// <summary>
        /// 保存系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveSysAppSettingResponse SaveSysAppSetting(SaveSysAppSettingReqeust request);

        /// <summary>
        /// 删除系统配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelSysAppSettingResponse DelSysAppSetting(DelSysAppSettingRequest request);
    }
}