using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;
using Mayiboy.Model.Po;

namespace Mayiboy.Contract
{

    public class QueryAllPermissionsRequest : Request
    {
        
    }

    public class QueryAllPermissionsResponse : Response
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class QueryPermissionsRequest : Request
    {
        public int MenuId { get; set; }
    }

    public class QueryPermissionsResponse : Response
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class QueryPermissionsByUserIdRequest : Request
    {
        public int UserId { get; set; }
    }

    public class QueryPermissionsByUserIdResponse : Response
    {
        public List<PermissionsDto> EntityList { get; set; }
    }

    public class SavePermissionsRequest : Request
    {
        public PermissionsDto Entity { get; set; }
    }

    public class SavePermissionsResponse : Response
    {
        public int Id { get; set; }
    }


    public class DelPermissionsRequest : Request
    {
        public int Id { get; set; }
    }

    public class DelPermissionsResponse : Response
    {

    }
}