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

}