using System;
using SqlSugar;

namespace Mayiboy.Model.Po
{
	[SugarTable("AppProject")]
	public class AppProjectPo
	{
		/// <summary>
		/// 主键Id
		/// </summary>
		[SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
		public int Id { get; set; }

		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// AppId
		/// </summary>
		public string ApplicationId { get; set; }

		/// <summary>
		/// 描述
		/// </summary>
		public string Remark { get; set; }

		/// <summary>
		/// 创建用户Id
		/// </summary>
		public int CreateUserId { get; set; }

		/// <summary>
		/// 创建时间
		/// </summary>
		public DateTime CreateTime { get; set; }

		/// <summary>
		/// 修改用户Id
		/// </summary>
		public int? UpdateUserId { get; set; }

		/// <summary>
		/// 修改时间
		/// </summary>
		public DateTime? UpdateTime { get; set; }

		/// <summary>
		/// 是否有效（0：无效；1：有效）
		/// </summary>
		public int IsValid { get; set; }

	}
}