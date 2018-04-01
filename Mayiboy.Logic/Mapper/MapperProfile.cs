using System;
using System.Text.RegularExpressions;
using AutoMapper;

namespace Mayiboy.Logic.Mapper
{
    /// <summary>
    /// 映射配置关系
    /// </summary>
    public class MapperProfile : Profile
    {
        /// <summary>
        /// 映射配置关系
        /// </summary>
        public MapperProfile()
        {
            #region 默认映射关系

            //DateTime to String
            this.CreateMap<DateTime?, string>().ConvertUsing(e =>
            {
                if (!e.HasValue) return string.Empty;

                return e.Value.ToString("yyyy-MM-dd HH:mm:ss:fff");
            });


            //String To DateTime

            this.CreateMap<string, DateTime?>().ConstructUsing(e =>
            {
                if (string.IsNullOrEmpty(e)) return null;

                if (e.Length == 8)
                {
                    if (Regex.IsMatch(e, "^[0-9]{4}[0-9]{2}[0-9]{2}$", RegexOptions.IgnoreCase))
                    {
                        return new DateTime(int.Parse(e.Substring(0, 4)), int.Parse(e.Substring(4, 2)), int.Parse(e.Substring(6, 2)));
                    }
                }

                return DateTime.Parse(e);
            });
            #endregion


        }
    }
}