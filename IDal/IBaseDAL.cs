/** 
* IBaseDAL.cs
*
* 功 能： 数据层通用接口
* 类 名： IBaseDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/8/29 17:04:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IDAL
{
    /// <summary>
    ///数据层父接口
    /// </summary>
    public interface IBaseDAL<T>
    {
        #region 根据主键获取模型

        T GetModel (int id);

        Task<T> GetModelAsync(int id);

        #endregion 根据主键获取模型

        #region 添加数据

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        int Add(T model);


        Task<int> AddAsync(T model);

        #endregion

        #region 根据ID删除数据

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        int Del(T model);

        Task<int> DelAsync(T model);
        
        #endregion

        #region 根据条件删除数据

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="delWhere">条件Lambda表达式</param>
        /// <returns></returns>
        int DelBy(Expression<Func<T, bool>> delWhere);

        Task<int> DelByAsync(Expression<Func<T, bool>> delWhere);
        
        #endregion

        #region 修改数据

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        int Modify(T model, params string[] proNames);

        /// <summary>
        /// 异步修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        Task<int> ModifyAsync(T model, params string[] proNames);

        #endregion

        #region 批量修改数据

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="modifyWhere">条件Lambda表达式</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        int ModifyBy(T model, Expression<Func<T, bool>> modifyWhere, params string[] proNames);

        Task<int> ModifyByAsync(T model, Expression<Func<T, bool>> modifyWhere, params string[] proNames);

        #endregion

        #region 获取总数

        /// <summary>
        /// 获取总数
        /// </summary>
        /// <param name="queryWhere">查询条件</param>
        /// <returns></returns>
        int GetTotal(Expression<Func<T, bool>> queryWhere);

        /// <summary>
        /// 异步获取总数
        /// </summary>
        /// <param name="queryWhere">查询条件</param>
        /// <returns></returns>
        Task<int> GetTotalAsync(Expression<Func<T, bool>> queryWhere);

        #endregion 获取总数

        #region 根据条件查询数据

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
        List<T> GetListBy(Expression<Func<T, bool>> queryWhere);

        Task<List<T>> GetListByAsync(Expression<Func<T, bool>> queryWhere);

        #endregion

        #region 根据条件查询数据并排序

        /// <summary>
        /// 根据条件查询数据
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        List<T> GetOrderListBy<TKey>(Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy,  bool isDesc = false);

        Task<List<T>> GetOrderListByAsync<TKey>(Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false);

        #endregion

        #region 根据条件分页查询数据并排序

        /// <summary>
        /// 根据条件分页查询数据并排序
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        List<T> GetPageListBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false);

        Task<PageData<T>> GetPageDataAsync<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false);

        #endregion

        #region 根据条件分页查询数据并输出总行数

        /// <summary>
        /// 根据条件分页查询数据并并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="orderBy">排序Lambda表达式</param>
        /// <param name="totalCount">数据总数</param>
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        List<T> GetPageListBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, out int totalCount, bool isDesc = false);

        Task<List<T>> GetPageListByAsync<TKey>(int pageIndex, int pagesize, Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false);

        #endregion

        #region 根据条件分页查询数据并输出总行数(多条件排序)

        /// <summary>
        /// 根据条件分页查询数据并并输出总行数
        /// </summary> 
        /// <typeparam name="TKey">排序字段类型</typeparam>
        ///  <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <param name="strOrderBy">排序条件字符串，如如Id desc,ParentId asc,其中asc可以省略</param>
        /// <param name="totalCount">数据总数</param>
        /// <returns></returns>
        List<T> GetPageListBy(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, string strOrderBy, out int totalCount);

        Task<PageData<T>> GetPageListByAsync(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, string strOrderBy);        

        #endregion
    }
}
