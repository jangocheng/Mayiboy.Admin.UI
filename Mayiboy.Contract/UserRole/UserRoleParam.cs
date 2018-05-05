using System.Collections.Generic;
using Mayiboy.Model.Dto;
using Mayiboy.Model.Po;

namespace Mayiboy.Contract
{
    public class SaveUserRoleRequest : BaseRequest
    {
        /// <summary>
        /// 角色实体
        /// </summary>
        public UserRolePo Entity { get; set; }
    }

    public class SaveUserRoleResponse : BaseResponse
    {
        public int Id { get; set; }
    }


    public class DelUserRoleRequest : BaseRequest
    {
        public int Id { get; set; }
    }

    public class DelUserRoleResponse : BaseResponse
    {
        public int Id { get; set; }
    }

    public class QueryUserRoleRequest : BasePageRequest
    {
        public string Name { get; set; }
    }

    public class QueryUserRoleResponse : BasePageResponse
    {
        public List<UserRoleDto> List { get; set; }
    }

    public class SaveUserRoleJoinRequest : BaseRequest
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

    public class SaveUserRoleJoinResponse : BaseResponse
    {
        public int Id { get; set; }
    }

    public class DelUserRoleJoinRequest : BaseRequest
    {
        public int Id { get; set; }
    }

    public class DelUserRoleJoinResponse : BaseResponse
    {

    }

    public class QueryUserRoleJoinRequest : BaseRequest
    {
        public int UserId { get; set; }
    }

    public class QueryUserRoleJoinResponse : BaseResponse
    {
        public List<UserRoleJoinDto> EntityList { get; set; }
    }

    public class SaveRolePermissionsRequest : BaseRequest
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

    public class SaveRolePermissionsResponse : BaseResponse
    {

    }

    public class QueryRoleMenuPermissionsRequest : BaseRequest
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

    public class QueryRoleMenuPermissionsResponse : BaseResponse
    {
        public List<RolePermissionsJoinDto> EntityList { get; set; }
    }
}