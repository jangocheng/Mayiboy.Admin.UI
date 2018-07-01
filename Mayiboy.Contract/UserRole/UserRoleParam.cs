using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
    public class SaveUserRoleRequest : Request
    {
        /// <summary>
        /// 角色实体
        /// </summary>
        public UserRoleDto Entity { get; set; }
    }

    public class SaveUserRoleResponse : Response
    {
        public int Id { get; set; }
    }


    public class DelUserRoleRequest : Request
    {
        public int Id { get; set; }
    }

    public class DelUserRoleResponse : Response
    {
        public int Id { get; set; }
    }

    public class QueryUserRoleRequest : PageRequest
    {
        public string Name { get; set; }
    }

    public class QueryUserRoleResponse : PageResponse
    {
        public List<UserRoleDto> List { get; set; }
    }

    public class SaveUserRoleJoinRequest : Request
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 角色Id列表
        /// </summary>
        public List<int> RoleIdList { get; set; }
    }

    public class SaveUserRoleJoinResponse : Response
    {
        public int Id { get; set; }
    }

    public class DelUserRoleJoinRequest : Request
    {
        public int Id { get; set; }
    }

    public class DelUserRoleJoinResponse : Response
    {

    }

    public class QueryUserRoleJoinRequest : Request
    {
        public int UserId { get; set; }
    }

    public class QueryUserRoleJoinResponse : Response
    {
        public List<UserRoleJoinDto> EntityList { get; set; }
    }

    public class SaveRolePermissionsRequest : Request
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public int NavbarId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 权限Id集合
        /// </summary>
        public List<int> PermissionsIds { get; set; }
    }

    public class SaveRolePermissionsResponse : Response
    {

    }

    public class QueryRoleMenuPermissionsRequest : Request
    {
        /// <summary>
        /// 栏目id
        /// </summary>
        public int NavbarId { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public int MenuId { get; set; }

        /// <summary>
        /// 角色id
        /// </summary>
        public int RoleId { get; set; }
    }

    public class QueryRoleMenuPermissionsResponse : Response
    {
        public List<RolePermissionsJoinDto> EntityList { get; set; }
    }
}