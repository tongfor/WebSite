/** 
* BaseBLL.cs
*
* 功 能： 逻辑层通用类
* 类 名： BaseBLL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/8/31 11:40:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using Common;
using IDAL;

namespace BLL
{
    public abstract class BaseService<T> where T : class, new()
    {
        protected IBaseDAL<T> IBaseDal;

        public abstract void SetIBaseDal();

        #region 根据主键获取模型

        public T GetModel(int id)
        {
            return IBaseDal.GetModel(id);
        }

        #endregion 根据主键获取模型

        #region 添加数据

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public int Add(T model)
        {
            return IBaseDal.Add(model);
        }
        #endregion

        #region 根据ID删除数据

        /// <summary>
        /// 根据ID删除数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <returns></returns>
        public int Del(T model)
        {
            return IBaseDal.Del(model);
        }
        #endregion

        #region 根据条件删除数据

        /// <summary>
        /// 根据条件删除数据
        /// </summary>
        /// <param name="delWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {
            return IBaseDal.DelBy(delWhere);
        }

        #endregion        

        #region 修改数据

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
            return IBaseDal.Modify(model, proNames);
        }
        #endregion

        #region 批量修改数据

        /// <summary>
        /// 批量修改数据
        /// </summary>
        /// <param name="model">模型</param>
        /// <param name="modifyWhere">条件Lambda表达式</param>
        /// <param name="proNames">要修改的字段</param>
        /// <returns></returns>
        public int ModifyBy(T model, Expression<Func<T, bool>> modifyWhere, params string[] proNames)
        {
            return IBaseDal.ModifyBy(model, modifyWhere, proNames);
        }

        #endregion
        
        #region 根据条件查询数据

        /// <summary>
        /// 根据条件查询数据
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> queryWhere)
        {
            return IBaseDal.GetListBy(queryWhere);
        }

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
        public List<T> GetOrderListBy<TKey>(Expression<Func<T, bool>> queryWhere, Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            return IBaseDal.GetOrderListBy(queryWhere, orderBy, isDesc);
        }

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
        public List<T> GetPageListBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere,
            Expression<Func<T, TKey>> orderBy, bool isDesc = false)
        {
            return IBaseDal.GetPageListBy(pageIndex, pageSize, queryWhere, orderBy, isDesc);
        }

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
        public List<T> GetPageListBy<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere,
            Expression<Func<T, TKey>> orderBy, out int totalCount, bool isDesc = false)
        {
            return IBaseDal.GetPageListBy(pageIndex, pageSize, queryWhere, orderBy, out totalCount, isDesc);
        }

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
        /// <param name="isDesc">是否降序</param>
        /// <returns></returns>
        public List<T> GetPageListBy(int pageIndex, int pageSize, Expression<Func<T, bool>> queryWhere, string strOrderBy, out int totalCount)
        {
            return IBaseDal.GetPageListBy(pageIndex, pageSize, queryWhere, strOrderBy, out totalCount);
        }

        #endregion 根据条件分页查询数据并输出总行数(多条件排序)

        #region 排序语句是否可以正常排序

        /// <summary>
        /// 排序语句是否可以正常排序
        /// </summary>
        /// <param name="strOrderBy">排序条件字符串，如如Id desc,ParentId asc,其中asc可以省略</param>
        /// <returns></returns>
        public bool CanOrdered(string strOrderBy)
        {
            bool bresult = false;
            if (string.IsNullOrEmpty(strOrderBy) || !Utils.IsSafeSqlString(strOrderBy))
            {
                return bresult;
            }
            Type type = typeof(T);
            string[] orderByArray = strOrderBy.Split(',');
            foreach (string s in orderByArray)
            {
                string[] orderBy = s.TrimStart().TrimEnd().Split(' ');
                //排序属性名
                string orderProperty = orderBy[0];
                PropertyInfo property = type.GetProperty(orderProperty); //获取指定名称的属性
                if (property == null)
                {
                    continue;
                }
                bresult = true;
            }
            return bresult;
        }

        #endregion 排序语句是否可以正常排序
    }
}
