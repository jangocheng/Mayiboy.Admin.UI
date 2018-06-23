using System;

namespace Mayiboy.Contract
{
    public class SystemMenuDto
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 父级Id
        /// </summary>
        public int Pid { get; set; }

        /// <summary>
        /// 因为前段需要该字段
        /// </summary>
        public int _parentId { get { return Pid; } }

        /// <summary>
        /// 导航id
        /// </summary>
        public int NavbarId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string UrlAddress { get; set; }

		/// <summary>
		/// 地址是否鉴权（1：鉴权）
		/// </summary>
		public int? AddressAuth { get; set; }

		/// <summary>
		/// 菜单类型
		/// </summary>
		public int MenuType { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public int CreateUserId { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改人id
        /// </summary>
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 是否有效（0：无效；1：有效）
        /// </summary>
        public int IsValid { get; set; }
    }
}