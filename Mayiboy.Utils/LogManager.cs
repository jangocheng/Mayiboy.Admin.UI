using Framework.Mayiboy.Logging;

namespace Mayiboy.Utils
{
    /// <summary>
    /// 日志管理
    /// </summary>
    public class LogManager
    {
        /// <summary>
        /// 默认日记录器
        /// </summary>
        public static readonly Logger DefaultLogger;

        /// <summary>
        /// 业务服务日志记录器
        /// </summary>
        public static readonly Logger LogicLogger;

        static LogManager()
        {
            DefaultLogger = LoggerFactory.GetLogger("DefaultLogger");
            LogicLogger = LoggerFactory.GetLogger("LogicLogger");
        }
    }
}