using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
	public interface IUserApIdAuthService : IBaseService, IDependency
	{
		/// <summary>
		/// 查询用户授权AppId
		/// </summary>
		/// <param name="request">参数</param>
		/// <returns></returns>
		QueryUserAppIdResponse QueryUserAppId(QueryUserAppIdRequest request);
	}
}