using System;
using System.ComponentModel.DataAnnotations;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Models
{
    public class SysMenuModel : BaseModel
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
        /// 菜单名称
        /// </summary>
        [Required(ErrorMessage = "菜单名必填")]
        public string Name { get; set; }

        /// <summary>
        /// Url地址
        /// </summary>
        public string UrlAddress { get; set; }

		/// <summary>
		/// 地址是否鉴权（1：鉴权）
		/// </summary>
		public int AddressAuth { get; set; }

		/// <summary>
		/// 导航id
		/// </summary>
		public int NavbarId { get; set; }

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
        /// 备注说明
        /// </summary>
        public string Remark { get; set; }

    }
}