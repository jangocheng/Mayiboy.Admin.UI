using SqlSugar;

namespace Mayiboy.DataAccess
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SqlServerDbContext : SqlSugarClient
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public SqlServerDbContext(string connectionString) : base(
            new ConnectionConfig()
            {
                DbType = DbType.SqlServer,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true
            })
        {
            //SQL执行前事件
            this.Aop.OnLogExecuting = (sql, pars) =>
            {
                string executetsql = sql;
            };


            //SQL执行前事件
            this.Aop.OnLogExecuting = (sql, pars) =>
            {
                string executetsql = sql;
            };

            //SQL执行完事件
            this.Aop.OnLogExecuted = (sql, pars) =>
            {

            };

            //执行SQL 错误事件
            this.Aop.OnError = (exp) =>
            {

            };


        }
    }
}