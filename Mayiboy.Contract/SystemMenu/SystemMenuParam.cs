using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{

    public class QueryAllSystemMenuRequest : Request
    {

    }

    public class QueryAllSystemMenuResponse : Response
    {
        public List<SystemMenuDto> SystemMenuList { get; set; }
    }

    /// <summary>
    /// 查询所有导航栏下的菜单参数
    /// </summary>
    public class QueryAllMenuRequest : Request
    {
        /// <summary>
        /// 导航栏Id
        /// </summary>
        public int NavbarId { get; set; }
    }

    /// <summary>
    /// 查询所有导航栏下的菜单参数
    /// </summary>
    public class QueryAllMenuResponse : Response
    {
        public List<SystemMenuDto> SystemMenuList { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询导航栏下的所有菜单参数
    /// </summary>
    public class QueryMenuByUserIdRequest : Request
    {
        /// <summary>
        /// 导航栏Id
        /// </summary>
        public int NavbarId { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }
    }

    /// <summary>
    /// 根据用户名Id查询导航栏下的所有菜单响应
    /// </summary>
    public class QueryMenuByUserIdResponse : Response
    {
        /// <summary>
        /// 系统菜单列表
        /// </summary>
        public List<SystemMenuDto> EntityList { get; set; }
    }

    public class SaveSystemMenuRequest : Request
    {
        /// <summary>
        /// 系统菜单
        /// </summary>
        public SystemMenuDto Entity { get; set; }
    }

    public class SaveSystemMenuResponse : Response
    {

        public int Id { get; set; }
    }



    public class DelSystemMenuRequest : Request
    {
        /// <summary>
        /// 主键id
        /// </summary>
        public int Id { get; set; }
    }


    public class DelSystemMenuResponse : Response
    {

    }

}