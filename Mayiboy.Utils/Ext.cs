using System;
using System.Text;

namespace Mayiboy.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class Ext
    {
        #region 转换数据类型
        /// <summary>
        /// 转换数据类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T As<T>(this object value)
        {
            return AutoMapper.Mapper.Map<T>(value);
        } 
        #endregion

        #region 添加缓存前缀
        /// <summary>
        /// 添加缓存前缀
        /// </summary>
        /// <param name="value">key</param>
        /// <param name="businessstr">业务字符串</param>
        /// <param name="block"></param>
        /// <returns></returns>
        public static string AddCachePrefix(this string value, string businessstr = "", bool block = true)
        {
            if (block)
            {
                return AppConfig.CacheKeyPrefix + ":" + businessstr + ":" + value;

            }
            else
            {
                return AppConfig.CacheKeyPrefix + businessstr + value;
            }
        } 
        #endregion
    }
}