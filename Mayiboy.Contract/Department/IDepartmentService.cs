using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 部门服务
    /// </summary>
    public interface IDepartmentService : IBaseService, IDependency
    {
        /// <summary>
        /// 查询所有部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        QueryAllDepartmentResponse QueryAllDepartment(QueryAllDepartmentRequest request);

        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        SaveDepartmentResponse SaveDeparment(SaveDepartmentRequest request);

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        DelDepartmentResponse DelDeparment(DelDepartamentRequest request);
    }
}