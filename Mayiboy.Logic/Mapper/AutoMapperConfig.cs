namespace Mayiboy.Logic.Mapper
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// 初始化AutoMapper
        /// </summary>
        public static void Configure()
        {
            AutoMapper.Mapper.Initialize(e =>
            {
                e.AddProfile(new MapperProfile());
            });
        }
    }
}