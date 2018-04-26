using System.Collections.Generic;

namespace Mayiboy.Admin.UI.Areas.SystemManage.Models
{
    /// <summary>
    /// 系统菜单选择tree
    /// </summary>
    public class SelectSysMenuModel
    {
        public int id { get; set; }

        public string text { get; set; }

        public bool closed { get; set; }

        public List<SelectSysMenuModel> children { get; set; }
    }
}