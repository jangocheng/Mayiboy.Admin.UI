using System.Collections.Generic;
using Framework.Mayiboy.Soa.Agent;

namespace Mayiboy.Contract
{
	public class QueryAppProjectRequest : PageRequest
	{
		/// <summary>
		/// 项目名称
		/// </summary>
		public string ProjectName { get; set; }

		/// <summary>
		/// 应用Id
		/// </summary>
		public string ApplicationId { get; set; }

	}

	public class QueryAppProjectResponse : PageResponse
	{
		public List<AppProjectDto> EntityList { get; set; }
	}

	public class SaveAppProjectRequest : Request
	{
		public AppProjectDto Entity { get; set; }
	}

	public class SaveAppProjectResponse : Response
	{
		public int Id { get; set; }
	}

	public class DelAppProjectRequest : Request
	{
		public int Id { get; set; }
	}

	public class DelAppProjectResponse : Response
	{
		public int Id { get; set; }
	}

}