using System;

namespace Mayiboy.Contract
{
	public class UserAppIdAuthDto
	{
		/// <summary>
		/// 主键Id
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 应用Id
		/// </summary>
		public string AppId { get; set; }

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
		/// 是否有效标识（1：有效；0：无效）
		/// </summary>
		public int IsValid { get; set; }
	}
}