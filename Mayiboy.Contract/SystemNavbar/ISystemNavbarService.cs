using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 系统导航栏
    /// </summary>
    public interface ISystemNavbarService : IBaseService, IDependency
    {
        /// <summary>
        /// 查询所有导航栏
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryAllNavbarResponse QueryAll(QueryAllNavbarRequest request);

        /// <summary>
        /// 根据用户Id查询系统导航栏列表
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryNavbarResponse QueryNavbar(QueryNavbarRequest request);
    }
}