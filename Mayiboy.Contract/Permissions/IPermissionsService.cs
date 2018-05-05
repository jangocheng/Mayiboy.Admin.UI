using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    public interface IPermissionsService : IBaseService, IDependency
    {
        /// <summary>
        /// 查询所有权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryAllPermissionsResponse QueryAllPermissions(QueryAllPermissionsRequest request);

        /// <summary>
        /// 查询权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryPermissionsResponse QueryPermissions(QueryPermissionsRequest request);

        /// <summary>
        /// 根据用户Id查询用户权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryPermissionsByUserIdResponse QueryPermissionsByUserId(QueryPermissionsByUserIdRequest request);

        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SavePermissionsResponse SavePermissions(SavePermissionsRequest request);

        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelPermissionsResponse DelPermissions(DelPermissionsRequest request);
    }
}