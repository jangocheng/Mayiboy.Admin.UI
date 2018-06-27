using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
	public interface IUserAppIdAuthService : IBaseService, IDependency
	{
		/// <summary>
		/// 查询用户授权AppId
		/// </summary>
		/// <param name="request">参数</param>
		/// <returns></returns>
		QueryByUserAppIdResponse QueryByUserAppId(QueryByUserAppIdRequest request);

		/// <summary>
		/// 分页查询用户授权AppId
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		QueryUserAppIdResponse QueryUserAppId(QueryUserAppIdRequest request);

		/// <summary>
		/// 保存用户授权AppId
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		SaveUserAppIdResponse SaveUserAppId(SaveUserAppIdRequest request);

		/// <summary>
		/// 删除用户授权AppId
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		DelUserAppIdResponse DelUserAppId(DelUserAppIdRequest request);
	}
}