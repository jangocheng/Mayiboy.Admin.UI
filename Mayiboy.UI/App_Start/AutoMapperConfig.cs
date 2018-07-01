using Mayiboy.Logic.Mapper;

namespace Mayiboy.UI
{
	/// <summary>
	/// 映射配置
	/// </summary>
	public class AutoMapperConfig
	{
		/// <summary>
		/// 初始化
		/// </summary>
		public static void Initialize()
		{
			AutoMapper.Mapper.Initialize(e =>
			{
				e.AddProfile(new MapperProfile());
			});

		}
	}
}