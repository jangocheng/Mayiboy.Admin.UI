using System;

namespace Mayiboy.Contract
{
    public class AppIdAuthDto
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 授权Token
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// 是否启用状态（0：未启用；1：已启用）
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改用户
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}