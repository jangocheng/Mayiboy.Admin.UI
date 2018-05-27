using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{

    public class AddOperationLogRequest : Request
    {
        /// <summary>
        /// 操作内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type { get; set; }
    }

    public class AddOperationLogResponse : Response
    {
        /// <summary>
        /// 新增主键Id
        /// </summary>
        public int Id { get; set; }
    }

    public class QueryOperSysLogRequest : PageRequest
    {

    }

    public class QueryOperSysLogResponse : PageResponse
    {
        public List<SystemOperationLogDto> EntityList { get; set; }
    }

}