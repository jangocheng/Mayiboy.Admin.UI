using System;

namespace Mayiboy.Admin.UI
{
	public class SystemNavbarModel
	{
		/// <summary>
		/// 主键id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 导航名
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 导航地址
		/// </summary>
		public string Url { get; set; }

		/// <summary>
		/// 备注说明
		/// </summary>
		public string Remark { get; set; }

		/// <summary>
		/// 排序字段
		/// </summary>
		public int Sort { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 创建用户Id
		/// </summary>
		public int? CreateUserId { get; set; }

		/// <summary>
		/// 更新时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 更新用户Id
		/// </summary>
		public int? UpdateUserId { get; set; }

		/// <summary>
		/// 是否有效（0：无效；1：有效）
		/// </summary>
		public int IsValid { get; set; }
	}
}