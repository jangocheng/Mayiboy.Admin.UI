using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 
    /// </summary>
    public interface IUserInfoService : IBaseService, IDependency
    {
        /// <summary>
        /// 插入用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveUserInfoResponse SaveUserInfo(SaveUserInfoRequest request);

        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryUserInfoResponse QueryUserInfo(QueryUserInfoRequest request);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelUserInfoResponse Del(DelUserInfoRequest request);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LoginQueryResponse LoginQuery(LoginQueryRequest request);
    }
}