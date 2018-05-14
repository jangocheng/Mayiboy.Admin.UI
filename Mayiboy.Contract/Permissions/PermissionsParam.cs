using System.Collections.Generic;
using Mayiboy.Model.Po;

namespace Mayiboy.Contract
{

    public class QueryAllPermissionsRequest : BaseRequest
    {
        
    }

    public class QueryAllPermissionsResponse : BaseResponse
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class QueryPermissionsRequest : BaseRequest
    {
        public int MenuId { get; set; }
    }

    public class QueryPermissionsResponse : BaseResponse
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class QueryPermissionsByUserIdRequest : BaseRequest
    {
        public int UserId { get; set; }
    }

    public class QueryPermissionsByUserIdResponse : BaseResponse
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class SavePermissionsRequest : BaseRequest
    {
        public PermissionsDto Entity { get; set; }
    }

    public class SavePermissionsResponse : BaseResponse
    {
        public int Id { get; set; }
    }


    public class DelPermissionsRequest : BaseRequest
    {
        public int Id { get; set; }
    }

    public class DelPermissionsResponse : BaseResponse
    {

    }
}