using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
	public class QueryByUserAppIdRequest : Request
	{
		public int UserId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string UserAppId { get; set; }
	}

	public class QueryByUserAppIdResponse : Response
	{
		/// <summary>
		/// 用户授权AppId实体
		/// </summary>
		public UserAppIdAuthDto Entity { get; set; }
	}

	public class QueryUserAppIdRequest : PageRequest
	{
		/// <summary>
		/// 用户名
		/// </summary>
		public string UserName { get; set; }

		/// <summary>
		/// 应用标识
		/// </summary>
		public string UserAppId { get; set; }
	}

	public class QueryUserAppIdResponse : PageResponse
	{
		/// <summary>
		/// 
		/// </summary>
		public List<UserAppIdAuthDto> EntityList { get; set; }
	}

	public class SaveUserAppIdRequest : Request
	{
		public UserAppIdAuthDto Entity { get; set; }
	}

	public class SaveUserAppIdResponse : Response
	{
		public int Id { get; set; }
	}

	public class DelUserAppIdRequest : Request
	{
		public int Id { get; set; }
	}

	public class DelUserAppIdResponse : Response
	{

	}
}