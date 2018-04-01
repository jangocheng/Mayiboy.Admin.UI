using Framework.Mayiboy.Caching;

namespace Mayiboy.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheManager
    {
        /// <summary>
        /// 运行时缓存
        /// </summary>
        public static readonly RunTimeCache RunTimeCache;

        /// <summary>
        /// Redis
        /// </summary>
        public static readonly RedisCache RedisDefault;

        static CacheManager()
        {
            RunTimeCache = (RunTimeCache)CacheFactory.GetCache("RunTime");
            RedisDefault = (RedisCache)CacheFactory.GetCache("RedisDefault");
        }

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="runtimecaheexpire">本地过期时间（秒）</param>
        /// <remarks>
        /// 先本地，在Redis
        /// </remarks>
        /// <returns></returns>
        public static T Get<T>(string key, int runtimecaheexpire = 10)
        {
            var value = RunTimeCache.Get<T>(key);

            if (value == null)
            {
                value = RedisDefault.Get<T>(key);

                if (value != null)
                {
                    RunTimeCache.Set<T>(key, value, runtimecaheexpire);
                }
            }

            return value;
        }


        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="runtimecaheexpire">本地过期时间（秒）</param>
        /// <remarks>
        /// 先本地，在Redis
        /// </remarks>
        /// <returns></returns>
        public static string Get(string key, int runtimecaheexpire = 10)
        {
            var value = RunTimeCache.Get(key);

            if (value == null)
            {
                value = RedisDefault.Get(key);

                if (value != null)
                {
                    RunTimeCache.Set(key, value, runtimecaheexpire);
                }
            }

            return value;
        }
    }
}