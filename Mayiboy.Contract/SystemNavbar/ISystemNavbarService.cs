using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 系统导航栏
    /// </summary>
    public interface ISystemNavbarService : IBaseService, IDependency
    {
        /// <summary>
        /// 获取栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        GetResponse Get(GetRequest request);

        /// <summary>
        /// 查询所有栏目
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryAllNavbarResponse QueryAll(QueryAllNavbarRequest request);

        /// <summary>
        /// 查询栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryResponse Query(QueryRequest request);

        /// <summary>
        /// 保存栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveResponse Save(SaveRequest request);

        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelResponse Del(DelReqeust request);

        /// <summary>
        /// 根据用户Id查询系统导航栏列表
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryNavbarResponse QueryNavbar(QueryNavbarRequest request);
    }
}