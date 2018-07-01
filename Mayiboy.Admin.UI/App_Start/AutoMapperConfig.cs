using System;
using AutoMapper;
using Mayiboy.Contract;
using Mayiboy.Logic.Mapper;

namespace Mayiboy.Admin.UI
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
			Mapper.Initialize(e =>
			{
				AddMapper(e);
				e.AddProfile(new MapperProfile());
			});
		}

		/// <summary>
		/// 添加映射映射关系
		/// </summary>
		/// <param name="config"></param>
		private static void AddMapper(IMapperConfigurationExpression config)
		{
			config.CreateMap<AccountModel, UserInfoDto>();
			config.CreateMap<UserInfoDto, AccountModel>();


			config.CreateMap<SystemMenuDto, SystemMenuModel>();
			config.CreateMap<SystemMenuModel, SystemMenuDto>();

			config.CreateMap<SystemNavbarDto, SystemNavbarModel>();
			config.CreateMap<SystemNavbarModel, SystemNavbarDto>();
		}

	}
}