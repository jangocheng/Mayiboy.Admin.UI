using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using SqlSugar;

namespace Mayiboy.DataAccess.Interface
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBaseRepository
	{
		#region Insert

		/// <summary>
		/// 插入数据
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="entity">实体</param>
		/// <returns></returns>
		int Insert<T>(T entity) where T : class, new();

		/// <summary>
		/// 插入并返回自增列
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		int InsertReturnIdentity<T>(T entity) where T : class, new();

		/// <summary>
		/// 插入并返回实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <remarks>
		/// 只是自identity 添加到 参数的实体里面并返回，没有查2次库，所以有些默认值什么的变动是取不到的你们需要手动进行2次查询获取
		/// </remarks>
		/// <returns></returns>
		T InsertReturnEntity<T>(T entity) where T : class, new();

		/// <summary>
		/// 插入并返回bool, 并将identity赋值到实体
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool InsertIdentityIntoEntity<T>(T entity) where T : class, new();

		#endregion

		#region Query

		/// <summary>
		/// 查询一条
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <returns></returns>
		T Find<T>(Expression<Func<T, bool>> expression) where T : class, new();

		/// <summary>
		/// 查询所有
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		List<T> FindAll<T>() where T : class, new();

		/// <summary>
		/// 取前n条
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <param name="topnum">n条，默认1条</param>
		/// <returns></returns>
		List<T> FindTopNum<T>(Expression<Func<T, bool>> expression, int topnum = 1) where T : class, new();

		/// <summary>
		/// 根据主键查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="pkValue">主键</param>
		/// <returns></returns>
		T FindSingle<T>(object pkValue) where T : class, new();

		/// <summary>
		/// 条件查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <returns></returns>
		List<T> FindWhere<T>(Expression<Func<T, bool>> expression) where T : class, new();

		/// <summary>
		/// 条件查询排序
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <param name="orderfileds">排序字段</param>
		/// <returns></returns>
		List<T> FindWhere<T>(Expression<Func<T, bool>> expression, string orderfileds) where T : class, new();

		/// <summary>
		/// 条件查询指定排序条件
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <param name="orderbyexpression">排序字段</param>
		/// <param name="orderbytype">排序类型</param>
		/// <returns></returns>
		List<T> FindWhere<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderbyexpression,
			OrderByType orderbytype = OrderByType.Asc) where T : class, new();

		/// <summary>
		/// 指定字段求和
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		TR Sum<T, TR>(Expression<Func<T, TR>> expression) where T : class, new();

		/// <summary>
		/// 求平均
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		TR Avg<T, TR>(Expression<Func<T, TR>> expression) where T : class, new();

		/// <summary>
		/// 最小
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		TR Min<T, TR>(Expression<Func<T, TR>> expression) where T : class, new();

		/// <summary>
		/// 最大
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TR"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		TR Max<T, TR>(Expression<Func<T, TR>> expression) where T : class, new();

		/// <summary>
		/// 是否存在这条记录
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		bool Any<T>(Expression<Func<T, bool>> expression) where T : class, new();

		/// <summary>
		/// 分页查询
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">查询条件</param>
		/// <param name="orderexpression">排序条件</param>
		/// <param name="pageIndex">页面索引</param>
		/// <param name="pageSize">页面大小</param>
		/// <param name="totalNumber">合计</param>
		/// <param name="orderbytype">排序类型</param>
		/// <returns></returns>
		List<T> FindPage<T>(Expression<Func<T, bool>> expression, Expression<Func<T, object>> orderexpression,
			int pageIndex, int pageSize, ref int totalNumber, OrderByType orderbytype = OrderByType.Asc) where T : class, new();

		#endregion

		#region Update

		/// <summary>
		/// 根据实体更新（主键要有值，主键是更新条件）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">实体</param>
		/// <returns></returns>
		int Update<T>(T entity) where T : class, new();

		/// <summary>
		/// 根据查询条件更新
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">实体</param>
		/// <param name="expression">更新条件</param>
		/// <returns></returns>
		int UpdateWhereColumns<T>(T entity, Expression<Func<T, object>> expression) where T : class, new();

		/// <summary>
		/// 更新指定列
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="columns">更新指定列</param>
		/// <returns></returns>
		int UpdateColumns<T>(T entity, Expression<Func<T, object>> columns) where T : class, new();

		/// <summary>
		/// 更新指定列除外的所有列
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity"></param>
		/// <param name="columns">不更新的列</param>
		/// <returns></returns>
		int UpdateIgnoreColumns<T>(T entity, Expression<Func<T, object>> columns) where T : class, new();

		/// <summary>
		/// 批量更新(主键要有值，主键是更新条件)
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		int Update<T>(List<T> list) where T : class, new();

		/// <summary>
		/// 更新实体，更新条件是根据表达式
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">实体</param>
		/// <param name="expression">条件</param>
		/// <returns></returns>
		int Update<T>(T entity, Expression<Func<T, bool>> expression) where T : class, new();

		/// <summary>
		/// 根据表达式中的列更新   ，指定列并赋值的更新，比较常用
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="entity">实体</param>
		/// <param name="expression">更新字段</param>
		/// <param name="whereexpression">更新条件</param>
		/// <returns></returns>
		int UpdateColumns<T>(T entity, Expression<Func<T, bool>> expression,
			Expression<Func<T, bool>> whereexpression) where T : class, new();




		#endregion

		#region Delete

		/// <summary>
		/// 根据实体删除（实体内主键一定要有值）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression">删除条件</param>
		/// <returns></returns>
		int Delete<T>(Expression<Func<T, bool>> expression) where T : class, new();

		/// <summary>
		/// 根据主键删除
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="primaryKeyValue">主键</param>
		/// <returns></returns>
		int Delete<T>(dynamic primaryKeyValue) where T : class, new();

		/// <summary>
		/// 根据主键集合删除
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="primaryKeyValues">主键</param>
		/// <returns></returns>
		int Delete<T>(dynamic[] primaryKeyValues) where T : class, new();

		/// <summary>
		/// 根据主键集合删除
		/// </summary>
		/// <typeparam name="T">类型</typeparam>
		/// <param name="pkValues">主键集合</param>
		/// <returns></returns>
		int Delete<T>(List<dynamic> pkValues) where T : class, new();


		/// <summary>
		/// 根据实体删除（实体内主键一定要有值）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="deleteObj">实体</param>
		/// <returns></returns>
		int Delete<T>(T deleteObj) where T : class, new();

		/// <summary>
		/// 根据实体集合删除（实体集合中的每一个实体主键一定要有值）
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="deleteObjs"></param>
		/// <returns></returns>
		int Delete<T>(List<T> deleteObjs) where T : class, new();

		#endregion

		#region UseStoredProcedure
		/// <summary>
		/// 使用存储过程
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="procedureName">存储过程名称</param>
		/// <returns></returns>
		List<T> UseStoredProcedure<T>(string procedureName);

		/// <summary>
		/// 使用存储过程
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		List<T> UseStoredProcedure<T>(string procedureName, params SugarParameter[] parameters);

		/// <summary>
		/// 使用存储过程
		/// </summary>
		/// <typeparam name="T">返回类型</typeparam>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="parameters">参数</param>
		/// <returns></returns>
		List<T> UseStoredProcedure<T>(string procedureName, object parameters);

		/// <summary>
		/// 调用存储过程
		/// </summary>
		/// <param name="procedureName">存储过程名称</param>
		/// <param name="parameters">参数</param>
		void UseStoredProcedure(string procedureName, params SugarParameter[] parameters);
		#endregion
	}
}