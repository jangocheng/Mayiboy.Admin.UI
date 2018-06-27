using System.Collections.Generic;
using Framework.Mayiboy.Ioc;
using Mayiboy.Contract;
using Mayiboy.Model.Po;

namespace Mayiboy.DataAccess.Interface
{
	public interface IUserAppIdAuthRepository : IBaseRepository, IDependency
	{
		/// <summary>
		/// 查询用户AppId授权
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="appid">应用标识</param>
		/// <param name="pageindex">页面索引</param>
		/// <param name="pagesize">页面大小</param>
		/// <param name="total">合计</param>
		/// <returns></returns>
		List<UserAppIdAuthDto> QueryUserAppId(string username, string appid, int pageindex, int pagesize, ref int total);
	}
}