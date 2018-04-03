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
        InsertResponse Insert(InsertRequest request);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        LoginQueryResponse LoginQuery(LoginQueryRequest request);
    }
}