using System.Collections.Generic;

namespace Mayiboy.Contract
{
    public class SaveDepartmentRequest : BaseRequest
    {
        public DepartmentDto Entity { get; set; }
    }

    public class SaveDepartmentResponse : BaseResponse
    {
        public int Id { get; set; }
    }

    public class DelDepartamentRequest : BaseRequest
    {
        public int Id { get; set; }
    }

    public class DelDepartmentResponse : BaseResponse
    {

    }

    public class QueryAllDepartmentRequest :BaseRequest
    {

    }

    public class QueryAllDepartmentResponse : BaseResponse
    {
        public List<DepartmentDto> List { get; set; }
    }
}