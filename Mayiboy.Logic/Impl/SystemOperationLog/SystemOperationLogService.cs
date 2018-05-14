using System;
using Framework.Mayiboy.Utility;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using Mayiboy.Utils;
using System.Linq;

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

        /// <summary>
        /// 查询系统日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public QueryOperSysLogResponse QueryOperSysLog(QueryOperSysLogRequest request)
        {
            var response = new QueryOperSysLogResponse();
            try
            {
                int total = 0;
                var list = _systemOperationLogRepository.FindPage<SystemOperationLogPo>(e => e.IsValid == 1, o => o.Id,
                    request.PageIndex, request.PageSize, ref total,SqlSugar.OrderByType.Desc);

                if (list != null && list.Count > 0)
                {
                    response.EntityList = list.Select(e => e.As<SystemOperationLogDto>()).ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.MessageText = ex.Message;
                response.MessageCode = "-1";
                LogManager.LogicLogger.ErrorFormat("查询系统日志出错：{0}", new { request, err = ex.ToString() }.ToJson());
            }
            return response;
        }
    }
}