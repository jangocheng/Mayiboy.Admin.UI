using Mayiboy.Contract;
using Mayiboy.Logic.Mapper;

namespace Mayiboy.Host
{
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