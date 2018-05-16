using System;
using System.ComponentModel.DataAnnotations;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Models
{
    public class UserInfoModel : BaseModel
    {
        /// <summary>
        /// 主键Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        [Required(ErrorMessage = "用户名必填")]
        public string LoginName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required(ErrorMessage = "邮箱地址必填")]
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [Required(ErrorMessage = "姓名必填")]
        public string Name { get; set; }

        /// <summary>
        /// 性别（-1：全部；0:女；1：男）
        /// </summary>
        public int? Sex { get; set; }

        /// <summary>
        /// 密文手机号
        /// </summary>
        [Required(ErrorMessage = "手机号必填")]
        public string Mobile { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartmentId { get; set; }

    }
}