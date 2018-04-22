using System;

namespace Mayiboy.Contract
{
    /// <summary>
    /// 分页查询基类
    /// </summary>
    [Serializable]
    public class BasePageResponse : BaseResponse
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 每页显示数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPage
        {
            get
            {
                return (int)Math.Ceiling(((decimal)TotalCount / (decimal)PageSize));
            }

        }
    }
}