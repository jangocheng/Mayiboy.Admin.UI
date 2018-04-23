using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;

namespace Mayiboy.Logic.Impl
{
    public class SystemOperationLogService : BaseService, ISystemOperationLogService
    {
        private readonly ISystemOperationLogRepository _systemOperationLogRepository;

        public SystemOperationLogService(ISystemOperationLogRepository systemOperationLogRepository)
        {
            _systemOperationLogRepository = systemOperationLogRepository;
        }

        /// <summary>
        /// 新增系统操作日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public AddOperationLogResponse AddOperationLog(AddOperationLogRequest request)
        {
            var response = new AddOperationLogResponse();
            try
            {
                var entity = new SystemOperationLogPo()
                {
                    Content = request.Content,
                    Type = request.Type,
                };

                EntityLogger.CreateEntity(entity);

                var id = _systemOperationLogRepository.InsertReturnIdentity(entity);

                response.Id = id;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageCode = "-1";
                response.MessageText = ex.Message;

                LogManager.LogicLogger.ErrorFormat("添加系统操作日志出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}