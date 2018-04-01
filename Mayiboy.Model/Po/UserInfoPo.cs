using System;
using SqlSugar;

namespace Mayiboy.Model.Po
{
    [SugarTable("UserInfo")]
    public class UserInfoPo
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        [SugarColumn(IsIdentity = true)]
        public int Id { get; set; }


        public string LoginName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Remark { get; set; }

        public int IsValid { get; set; }
    }
}