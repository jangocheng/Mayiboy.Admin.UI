using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    public interface IUserRoleService : IBaseService, IDependency
    {

        /// <summary>
        /// 查询用户角色
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryUserRoleResponse QueryUserRole(QueryUserRoleRequest request);

        /// <summary>
        /// 保存角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveUserRoleResponse SaveUserRole(SaveUserRoleRequest request);

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelUserRoleResponse DelUserRole(DelUserRoleRequest request);
    }
}