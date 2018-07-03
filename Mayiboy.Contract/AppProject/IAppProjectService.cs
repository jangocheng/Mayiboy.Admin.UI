using Framework.Mayiboy.Ioc;

namespace Mayiboy.Contract
{
	public interface IAppProjectService : IBaseService, IDependency
	{

		/// <summary>
		/// 分页查询应用项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		QueryAppProjectResponse QueryAppProject(QueryAppProjectRequest request);

		/// <summary>
		/// 保存分页项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		SaveAppProjectResponse SaveAppProject(SaveAppProjectRequest request);

		/// <summary>
		/// 删除应用项目
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		DelAppProjectResponse DelAppProject(DelAppProjectRequest request);
	}
}