using System.ComponentModel.DataAnnotations;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Models
{
    public class PermissionsModel : BaseModel
    {
        public int Id { get; set; }

        public int MenuId { get; set; }

        [Required(ErrorMessage = "权限名必填")]
        public string Name { get; set; }

        public string Action { get; set; }

        public string Code { get; set; }

        public int Type { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}