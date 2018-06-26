using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
	public class QueryUserAppIdRequest : Request
	{
		public int UserId { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public string UserAppId { get; set; }
	}

	public class QueryUserAppIdResponse : Response
	{
		/// <summary>
		/// 用户授权AppId实体
		/// </summary>
		public UserAppIdAuthDto Entity { get; set; }
	}
}