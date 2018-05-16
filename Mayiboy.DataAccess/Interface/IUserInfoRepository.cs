using System.Collections.Generic;
using Framework.Mayiboy.Ioc;
using Mayiboy.Contract;

namespace Mayiboy.DataAccess.Interface
{
    public interface IUserInfoRepository : IBaseRepository, IDependency
    {
        /// <summary>
        /// 查询用户信息
        /// </summary>
        /// <param name="account">用户名</param>
        /// <param name="sex">性别（-1：全部；0:女；1：男）</param>
        /// <param name="pagesize">页面大小</param>
        /// <param name="total">合计</param>
        /// <param name="departmentid">部门Id</param>
        /// <param name="pageindex">页面索引</param>
        /// <returns></returns>
        List<UserInfoDto> QueryUserInfo(string account, int? sex, int departmentid, int pageindex, int pagesize, ref int total);
    }
}