using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 系统菜单
    /// </summary>
    public interface ISystemMenuService : IBaseService, IDependency
    {
        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryAllMenuResponse QueryAllMenu(QueryAllMenuRequest request);

        /// <summary>
        /// 根据用户id查询菜单列表
        /// </summary>
        /// <param name="request">参数</param>
        /// <returns></returns>
        QueryMenuByUserIdResponse QueryMenuByUserId(QueryMenuByUserIdRequest request);
    }
}