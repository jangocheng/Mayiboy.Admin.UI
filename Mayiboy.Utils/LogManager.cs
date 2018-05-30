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

        /// <summary>
        /// 任务组件日志记录器
        /// </summary>
        public static readonly Logger TaskLoger;

        /// <summary>
        /// 执行Sql脚本出错
        /// </summary>
        public static readonly Logger DataAccessLogger;

        static LogManager()
        {
            DefaultLogger = LoggerFactory.GetLogger("DefaultLogger");
            LogicLogger = LoggerFactory.GetLogger("LogicLogger");
            DataAccessLogger = LoggerFactory.GetLogger("DataAccessLogger");
            TaskLoger= LoggerFactory.GetLogger("TaskLogger");
        }
    }
}