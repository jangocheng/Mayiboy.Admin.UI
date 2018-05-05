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

        /// <summary>
        /// 查询用户角色关联列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryUserRoleJoinResponse QueryUserRoleJoin(QueryUserRoleJoinRequest request);

        /// <summary>
        /// 保存用户角色关联
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveUserRoleJoinResponse SaveUserRoleJoin(SaveUserRoleJoinRequest request);

        /// <summary>
        /// 删除用户角色关联
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelUserRoleJoinResponse DelUserRoleJoin(DelUserRoleJoinRequest request);

        /// <summary>
        /// 查询角色菜单权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryRoleMenuPermissionsResponse QueryRoleMenuPermissions(QueryRoleMenuPermissionsRequest request);

        /// <summary>
        /// 保存角色权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveRolePermissionsResponse SaveRolePermissions(SaveRolePermissionsRequest request);
    }
}