using System;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    [Serializable]
    public class BasePageRequest : BaseRequest
    {
        /// <summary>
        /// 页面索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }
    }
}