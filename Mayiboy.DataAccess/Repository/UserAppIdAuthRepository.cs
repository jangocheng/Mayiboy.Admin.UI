using System.Collections.Generic;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using SqlSugar;

namespace Mayiboy.DataAccess.Repository
{
	public class UserAppIdAuthRepository : BaseRepository, IUserAppIdAuthRepository
	{

		/// <summary>
		/// 查询用户Id授权
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="appid">应用Id</param>
		/// <param name="pageindex"></param>
		/// <param name="pagesize"></param>
		/// <param name="total"></param>
		/// <returns></returns>
		public List<UserAppIdAuthDto> QueryUserAppId(string username, string appid, int pageindex, int pagesize, ref int total)
		{

			var list = CurrentDbContext.Queryable<UserAppIdAuthPo, UserInfoPo>((t1, t2) => new object[]
			{
				JoinType.Left, t1.UserId == t2.Id
			})
			.Where((t1, t2) =>
						t1.IsValid == 1 && t2.IsValid == 1
						&& (SqlFunc.IsNullOrEmpty(username) || t2.LoginName.Contains(username) || t2.Name.Contains(username))
						&& (SqlFunc.IsNullOrEmpty(appid) || t1.AppId.Contains(appid)))
			.OrderBy((t1, t2) => t1.Id, OrderByType.Desc)
			.Select((t1, t2) => new UserAppIdAuthDto
			{
				Id = t1.Id,
				AppId = t1.AppId,
				CreateTime = t1.CreateTime,
				CreateUserId = t1.CreateUserId,
				IsValid = t1.IsValid,
				UpdateTime = t1.UpdateTime,
				UpdateUserId = t1.UpdateUserId,
				UserId = t1.UserId,
				LoginName = t2.LoginName,
				Name=t2.Name
			}).ToPageList(pageindex, pagesize, ref total);

			return list;
		}
	}
}