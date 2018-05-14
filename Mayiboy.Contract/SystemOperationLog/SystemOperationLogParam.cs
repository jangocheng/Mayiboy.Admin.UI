using System.Collections.Generic;

namespace Mayiboy.Contract
{

    public class AddOperationLogRequest : BaseRequest
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

    public class AddOperationLogResponse : BaseResponse
    {
        /// <summary>
        /// 新增主键Id
        /// </summary>
        public int Id { get; set; }
    }

    public class QueryOperSysLogRequest : BasePageRequest
    {

    }

    public class QueryOperSysLogResponse : BasePageResponse
    {
        public List<SystemOperationLogDto> EntityList { get; set; }
    }

}