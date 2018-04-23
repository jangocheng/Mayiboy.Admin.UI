using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 系统操作日志
    /// </summary>
    public interface ISystemOperationLogService : IBaseService, IDependency
    {
        /// <summary>
        /// 添加系统操作日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        AddOperationLogResponse AddOperationLog(AddOperationLogRequest request);
    }
}