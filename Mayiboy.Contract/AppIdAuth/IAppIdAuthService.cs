using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 应用授权token服务管理
    /// </summary>
    public interface IAppIdAuthService : IBaseService, IDependency
    {
        /// <summary>
        /// 保存应用授权Token
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveAppIdAuthResponse SaveAppIdAuthToken(SaveAppIdAuthRequest request);

        /// <summary>
        /// 查询应用授权列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryAppIdAuthResponse QueryAppIdAuth(QueryAppIdAuthRequest request);

        /// <summary>
        /// 删除应用授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelAppIdAuthResponse DelAppIdAuth(DelAppIdAuthRequest request);

        /// <summary>
        /// 更新启用状态
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        UpdateStatusResponse UpdateStatus(UpdateStatusRequest request);

        /// <summary>
        /// 更加AppId查询应用授权配置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryByAppIdResponse QueryByAppId(QueryByAppIdRequest request);
    }
}