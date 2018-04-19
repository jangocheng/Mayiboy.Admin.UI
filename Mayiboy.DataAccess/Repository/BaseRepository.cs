using System;
using System.Collections.Generic;
using Mayiboy.DataAccess.Interface;
using Mayiboy.Utils;
using System.Configuration;
using System.Data;
using System.Linq.Expressions;
using SqlSugar;

namespace Mayiboy.DataAccess.Repository
{
    /// <summary>
    /// 基础仓储
    /// </summary>
    public class BaseRepository : IBaseRepository
    {
        protected SqlServerDbContext CurrentDbContext { get; set; }

        protected BaseRepository() : this(AppConfig.DefatultSqlConnection)
        {

        }

        /// <summary>
        /// 指定连接字符串
        /// </summary>
        /// <param name="connString"></param>
        protected BaseRepository(string connString)
        {
            CurrentDbContext = new SqlServerDbContext(connString);
        }

        #region Insert

        /// <summary>
        /// 插入并返回受影响行数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert<T>(T entity) where T : class, new()
        {
            return (int)CurrentDbContext.Insertable(entity).ExecuteCommand();
        }

        /// <summary>
        /// 插入并返回自增列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertReturnIdentity<T>(T entity) where T : class, new()
        {
            return (int)CurrentDbContext.Insertable<T>(entity).ExecuteReturnIdentity();
        }

        /// <summary>
        /// 插入并返回实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <remarks>
        /// 只是自identity 添加到 参数的实体里面并返回，没有查2次库，所以有些默认值什么的变动是取不到的你们需要手动进行2次查询获取
        /// </remarks>
        /// <returns></returns>
        public T InsertReturnEntity<T>(T entity) where T : class, new()
        {
            return CurrentDbContext.Insertable(entity).ExecuteReturnEntity();
        }

        /// <summary>
        /// 插入并返回bool, 并将identity赋值到实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertIdentityIntoEntity<T>(T entity) where T : class, new()
        {
            return CurrentDbContext.Insertable<T>(entity).ExecuteCommandIdentityIntoEntity();
        }

        #endregion

        #region Query

        /// <summary>
        /// 查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询条件</param>
        /// <returns></returns>
        public T Find<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).First(expression);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> FindAll<T>() where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).ToList();
        }

        /// <summary>
        /// 取前n条
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询条件</param>
        /// <param name="topnum">n条，默认1条</param>
        /// <returns></returns>
        public List<T> FindTopNum<T>(Expression<Func<T, bool>> expression, int topnum = 1) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Where(expression).Take(topnum).ToList();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pkValue">主键</param>
        /// <returns></returns>
        public T FindSingle<T>(object pkValue) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).InSingle(pkValue);
        }

        /// <summary>
        /// 条件查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询条件</param>
        /// <returns></returns>
        public List<T> FindWhere<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Where(expression).ToList();
        }

        /// <summary>
        /// 条件查询排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderfileds"></param>
        /// <returns></returns>
        public List<T> FindWhere<T>(Expression<Func<T, bool>> expression, string orderfileds) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Where(expression).OrderBy(orderfileds).ToList();
        }

        /// <summary>
        /// 条件查询指定排序条件
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <param name="orderbyexpression"></param>
        /// <param name="orderbytype"></param>
        /// <returns></returns>
        public List<T> FindWhere<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderbyexpression, OrderByType orderbytype = OrderByType.Asc) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>()
                    .With(SqlWith.NoLock)
                    .Where(expression)
                    .OrderBy(orderbyexpression, orderbytype)
                    .ToList();
        }

        /// <summary>
        /// 指定字段求和
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TR Sum<T, TR>(Expression<Func<T, TR>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Sum(expression);
        }

        /// <summary>
        /// 求平均
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TR Avg<T, TR>(Expression<Func<T, TR>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Avg(expression);
        }

        /// <summary>
        /// 最小
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TR Min<T, TR>(Expression<Func<T, TR>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Min(expression);
        }

        /// <summary>
        /// 最大
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TR"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public TR Max<T, TR>(Expression<Func<T, TR>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Max(expression);
        }

        /// <summary>
        /// 是否存在这条记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public bool Any<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Any(expression);
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">查询条件</param>
        /// <param name="orderexpression">排序条件</param>
        /// <param name="pageIndex">页面索引</param>
        /// <param name="pageSize">页面大小</param>
        /// <param name="totalNumber">合计</param>
        /// <returns></returns>
        public List<T> FindPage<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderexpression, int pageIndex, int pageSize, ref int totalNumber) where T : class, new()
        {
            return CurrentDbContext.Queryable<T>().With(SqlWith.NoLock).Where(expression).OrderBy(orderexpression).ToPageList(pageIndex, pageSize, ref totalNumber);
        }

        #endregion

        #region Update

        /// <summary>
        /// 根据实体更新（主键要有值，主键是更新条件）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public int Update<T>(T entity) where T : class, new()
        {
            return CurrentDbContext.Updateable<T>(entity).ExecuteCommand();
        }

        /// <summary>
        /// 根据查询条件更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="expression">更新条件</param>
        /// <returns></returns>
        public int UpdateWhereColumns<T>(T entity, Expression<Func<T, object>> expression) where T : class, new()
        {
            return CurrentDbContext.Updateable(entity).WhereColumns(expression).ExecuteCommand();
        }

        /// <summary>
        /// 更新指定列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expression">更新指定列</param>
        /// <returns></returns>
        public int UpdateColumns<T>(T entity, Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Updateable(entity).UpdateColumns(expression).ExecuteCommand();
        }

        /// <summary>
        /// 更新指定列除外的所有列
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="expression">不更新的列</param>
        /// <returns></returns>
        public int UpdateIgnoreColumns<T>(T entity, Expression<Func<T, object>> expression) where T : class, new()
        {
            return CurrentDbContext.Updateable(entity).IgnoreColumns(expression).ExecuteCommand();
        }

        /// <summary>
        /// 批量更新(主键要有值，主键是更新条件)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public int Update<T>(List<T> list) where T : class, new()
        {
            return CurrentDbContext.Updateable(list).ExecuteCommand();
        }

        /// <summary>
        /// 更新实体，更新条件是根据表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="expression">条件</param>
        /// <returns></returns>
        public int Update<T>(T entity, Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Updateable(entity).Where(expression).ExecuteCommand();
        }

        /// <summary>
        /// 根据表达式中的列更新   ，指定列并赋值的更新，比较常用
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">实体</param>
        /// <param name="expression">更新字段</param>
        /// <param name="whereexpression">更新条件</param>
        /// <returns></returns>
        public int UpdateColumns<T>(T entity, Expression<Func<T, bool>> expression, Expression<Func<T, bool>> whereexpression) where T : class, new()
        {
            return CurrentDbContext.Updateable(entity).UpdateColumns(expression).Where(whereexpression).ExecuteCommand();
        }




        #endregion

        #region Delete

        /// <summary>
        /// 根据实体删除（实体内主键一定要有值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">删除条件</param>
        /// <returns></returns>
        public int Delete<T>(Expression<Func<T, bool>> expression) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>(expression).ExecuteCommand();
        }

        /// <summary>
        /// 根据主键删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValue">主键</param>
        /// <returns></returns>
        public int Delete<T>(dynamic primaryKeyValue) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>(primaryKeyValue);
        }

        /// <summary>
        /// 根据主键集合删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="primaryKeyValues">主键</param>
        /// <returns></returns>
        public int Delete<T>(dynamic[] primaryKeyValues) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>(primaryKeyValues).ExecuteCommand();
        }

        /// <summary>
        /// 根据主键集合删除
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="pkValues">主键集合</param>
        /// <returns></returns>
        public int Delete<T>(List<dynamic> pkValues) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>(pkValues).ExecuteCommand();
        }


        /// <summary>
        /// 根据实体删除（实体内主键一定要有值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deleteObj">实体</param>
        /// <returns></returns>
        public int Delete<T>(T deleteObj) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>().Where(deleteObj).ExecuteCommand();
        }

        /// <summary>
        /// 根据实体集合删除（实体集合中的每一个实体主键一定要有值）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="deleteObjs"></param>
        /// <returns></returns>
        public int Delete<T>(List<T> deleteObjs) where T : class, new()
        {
            return CurrentDbContext.Deleteable<T>().Where(deleteObjs).ExecuteCommand();
        }

        #endregion

        #region UseStoredProcedure
        /// <summary>
        /// 使用存储过程
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <returns></returns>
        public List<T> UseStoredProcedure<T>(string procedureName)
        {
            return CurrentDbContext.Ado.UseStoredProcedure().SqlQuery<T>(procedureName);
        }

        /// <summary>
        /// 使用存储过程
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> UseStoredProcedure<T>(string procedureName, params SugarParameter[] parameters)
        {
            return CurrentDbContext.Ado.UseStoredProcedure().SqlQuery<T>(procedureName);
        }

        /// <summary>
        /// 使用存储过程
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public List<T> UseStoredProcedure<T>(string procedureName, object parameters)
        {
            return CurrentDbContext.Ado.UseStoredProcedure().SqlQuery<T>(procedureName, parameters);
        }

        /// <summary>
        /// 调用存储过程
        /// </summary>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public void UseStoredProcedure(string procedureName, params SugarParameter[] parameters)
        {
            CurrentDbContext.Ado.UseStoredProcedure().ExecuteCommand(procedureName, parameters);
        }
        #endregion
    }
}