using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
    public class SaveDepartmentRequest : Request
    {
        public DepartmentDto Entity { get; set; }
    }

    public class SaveDepartmentResponse : Response
    {
        public int Id { get; set; }
    }

    public class DelDepartamentRequest : Request
    {
        public int Id { get; set; }
    }

    public class DelDepartmentResponse : Response
    {

    }

    public class QueryAllDepartmentRequest :Request
    {

    }

    public class QueryAllDepartmentResponse : Response
    {
        public List<DepartmentDto> List { get; set; }
    }

    public class SaveUserDepartmentRequest : Request
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public int DepartmentId { get; set; }
    }

    public class SaveUserDepartmentResponse : Response
    {
        
    }

	/// <summary>
	/// 查询组织架构参数
	/// </summary>
	public class QueryOrganizationRequest : Request
	{
		/// <summary>
		/// 部门Id
		/// </summary>
		public int DepartmentId { get; set; }
	}

	public class QueryOrganizationResponse : Response
	{
		
	}
}