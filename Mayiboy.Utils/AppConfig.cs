using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.Mayiboy.Utility;

namespace Mayiboy.Utils
{
    public class AppConfig
    {

        /// <summary>
        /// 默认数据库连接字符串
        /// </summary>
        public static string DefatultSqlConnection
        {
            get { return ConfigurationManager.ConnectionStrings["DefatultSqlConnection"].ConnectionString; }
        }

        /// <summary>
        /// 缓存Key前缀
        /// </summary>
        public static string CacheKeyPrefix
        {
            get
            {
                return ConfigHelper.GetString("CacheKeyPrefix");
            }
        }
    }
}
