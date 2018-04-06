/** 
* IArticleDAL.cs
*
* 功 能： Article数据层接口
* 类 名： IArticleDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Models;

namespace IDAL
{
    public partial interface IArticleDAL : IBaseDAL<Article>
    {
        Task<Article> GetModelAsync(int id);

        #region 获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        List<ArticleIncludeClassNameView> GetArticleIncludeClass(Expression<Func<Article, bool>> queryWhere);

        /// <summary>
        /// 异步获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        Task<List<ArticleIncludeClassNameView>> GetArticleIncludeClassAsync(Expression<Func<Article, bool>> queryWhere);

        #endregion

        #region 分页获取文章关联文章类别的数据并排序（延时加载）

        /// <summary>
        /// 分页获取文章关联文章类别的数据并排序（延时加载）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <param name="strOrderBy">排序lamdba表达式</param>
        /// <param name="totalCount">总数</param>
        /// x
        /// <returns></returns>
        List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy, out int totalCount);

        /// <summary>
        /// 异步分页获取文章关联文章类别的数据并排序（延时加载）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <param name="strOrderBy">排序lamdba表达式</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy);

        #endregion

        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(Article表用aco表示)</param>
        /// <returns></returns>
        List<ArticleView> GetArticleIncludeClass(string strWhere);

        /// <summary>
        /// 异步获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(Article表用aco表示)</param>
        /// <returns></returns>
        Task<List<ArticleView>> GetArticleIncludeClassAsync(string strWhere);

        #endregion

        #region 分页获取文章关联文章类别的数据并排序（直接执行查询语句）

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strWhere">查询条件(Article表用aco表示),必传</param>
        /// <param name="orderBy">排序(Article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, string strWhere, string orderBy, out int totalCount);

        /// <summary>
        /// 异步获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strWhere">查询条件(Article表用aco表示),必传</param>
        /// <param name="orderBy">排序(Article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, string strWhere, string orderBy);

        #endregion

        #region 点击量累加

        /// <summary>
        /// 根据文章ID点击量累加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int AccumulationClickCount(int id);

        /// <summary>
        /// 异步根据文章ID点击量累加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<int> AccumulationClickCountAsync(int id);

        #endregion 点击量累加
    }
}
