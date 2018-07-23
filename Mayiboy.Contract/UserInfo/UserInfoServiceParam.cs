using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{

	#region 保存用户信息
	/// <summary>
	/// 保存用户信息参数
	/// </summary>
	public class SaveUserInfoRequest : Request
	{
		/// <summary>
		/// 用户信息
		/// </summary>
		public UserInfoDto UserInfoEntity { get; set; }
	}

	/// <summary>
	/// 插入用户信息响应
	/// </summary>
	public class SaveUserInfoResponse : Response
	{
		/// <summary>
		/// 用户信息
		/// </summary>
		public UserInfoDto UserInfoEntity { get; set; }
	}
	#endregion

	#region 登录用户查询
	/// <summary>
	/// 登录用户查询参数
	/// </summary>
	public class LoginQueryRequest : Request
	{
		/// <summary>
		/// 登录名
		/// </summary>
		public string LoginName { get; set; }

		/// <summary>
		/// 密码
		/// </summary>
		public string Password { get; set; }
	}

	/// <summary>
	/// 登录用户查询响应
	/// </summary>
	public class LoginQueryResponse : Response
	{
		/// <summary>
		/// 用户信息
		/// </summary>
		public UserInfoDto UserInfoEntity { get; set; }
	}
	#endregion

	#region 查询用户信息
	/// <summary>
	/// 查询用户信息参数
	/// </summary>
	public class QueryUserInfoRequest : PageRequest
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string Account { get; set; }

		/// <summary>
		/// 性别（-1：全部；0:女；1：男）
		/// </summary>
		public int? Sex { get; set; }

		/// <summary>
		/// 部门Id
		/// </summary>
		public int DepartmentId { get; set; }
	}

	/// <summary>
	/// 查询用户信息响应
	/// </summary>
	public class QueryUserInfoResponse : PageResponse
	{
		/// <summary>
		/// 用户列表
		/// </summary>
		public List<UserInfoDto> EntityList { get; set; }
	}
	#endregion

	#region 根据用户Id查询用户信息
	/// <summary>
	/// 根据用户Id查询用户信息参数
	/// </summary>
	public class QueryUserInfoByIdRequest : Request
	{
		public int Id { get; set; }
	}

	/// <summary>
	/// 根据用户Id查询用户信息响应
	/// </summary>
	public class QueryUserInfoByIdResponse : Response
	{
		public UserInfoDto Entity { get; set; }
	}
	#endregion

	#region 删除用户信息
	/// <summary>
	/// 删除用户信息参数
	/// </summary>
	public class DelUserInfoRequest : Request
	{
		/// <summary>
		/// 主键id
		/// </summary>
		public int Id { get; set; }
	}

	public class DelUserInfoResponse : Response
	{
		/// <summary>
		/// 删除的用户信息
		/// </summary>
		public UserInfoDto Entity { get; set; }
	}
	#endregion

	#region 重置用户密码
	/// <summary>
	/// 重置用户密码参数
	/// </summary>
	public class ResetPasswordRequest : Request
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 新密码（明文）
		/// </summary>
		public string NewPassword { get; set; }
	}

	/// <summary>
	/// 重置用户密码响应
	/// </summary>
	public class ResetPasswordResponse : Response
	{

	}
	#endregion

	#region 修改用户密码
	/// <summary>
	/// 修改用户密码参数
	/// </summary>
	public class ChangePasswordRequest : Request
	{
		/// <summary>
		/// 用户Id
		/// </summary>
		public int UserId { get; set; }

		/// <summary>
		/// 旧密码（明文）
		/// </summary>
		public string OldPassword { get; set; }

		/// <summary>
		/// 新密码（明文）
		/// </summary>
		public string NewPassword { get; set; }
	}

	/// <summary>
	/// 修改用户密码响应
	/// </summary>
	public class ChangePasswordResponse : Response
	{

	}
	#endregion

	#region 根据用户名查询用户列表
	/// <summary>
	/// 根据用户名查询用户列表参数
	/// </summary>
	public class QueryUserInfoByNameRequest : Request
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string Name { get; set; }
	}

	/// <summary>
	/// 根据用户名查询用户列表响应
	/// </summary>
	public class QueryUserInfoByNameResponse : Response
	{
		/// <summary>
		/// 用户列表
		/// </summary>
		public List<UserInfoDto> EntityList { get; set; }
	} 
	#endregion

}