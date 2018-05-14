using System;

namespace Mayiboy.Contract
{
    public class SystemOperationLogDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 操作内容说明
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 类型（1:登录；2：退出；3：其他操作）
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}