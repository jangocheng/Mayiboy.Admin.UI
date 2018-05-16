using System.Collections.Generic;
using Mayiboy.Contract;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Model.Po;
using SqlSugar;

namespace Mayiboy.DataAccess.Repository
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoRepository : BaseRepository, IUserInfoRepository
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
        public List<UserInfoDto> QueryUserInfo(string account, int? sex, int departmentid, int pageindex, int pagesize, ref int total)
        {
            var list = CurrentDbContext.Queryable<UserInfoPo, UserDepartmentJoinPo, DepartmentPo>((ui, udj, d) => new object[]
             {
                JoinType.Left, (ui.Id == udj.UserId && udj.IsValid==1),
                JoinType.Left,(udj.DepartmentId==d.Id && d.IsValid==1)
             }).Where((ui, udj, d) =>
                 ui.IsValid == 1
                 && (SqlFunc.IsNullOrEmpty(account) || ui.LoginName.Contains(account) || ui.Email.Contains(account))
                 && (sex == -1 || ui.Sex == sex)
                 && (departmentid == 0 || udj.DepartmentId == departmentid)
                ).OrderBy((ui, udj) => ui.Id, OrderByType.Desc)
                .Select((ui, udj, d) => new UserInfoDto
                {
                    Id = ui.Id,
                    LoginName = ui.LoginName,
                    Email = ui.Email,
                    Password = ui.Password,
                    Name = ui.Name,
                    Sex = ui.Sex,
                    HeadimgUrl = ui.HeadimgUrl,
                    Mobile = ui.Mobile,
                    CreateUserId = ui.CreateUserId,
                    CreateTime = ui.CreateTime,
                    UpdateUserId = ui.UpdateUserId,
                    UpdateTime = ui.UpdateTime,
                    Remark = ui.Remark,
                    IsValid = ui.IsValid,
                    DepartmentId = udj.DepartmentId,
                    DepartementName = d.Name
                }).ToPageList(pageindex, pagesize, ref total);

            return list;
        }
    }
}