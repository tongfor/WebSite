/** 
* ArticleService.cs
*
* 功 能： Article逻辑层
* 类 名： ArticleService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Common;
using Models;
using IDAL;
using Common.Html;

namespace BLL
{
    public partial class ArticleService
    {
        protected IArticleDAL ArticleDAL = new DALSession().IArticleDAL;

        #region 获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public List<ArticleIncludeClassNameView> GetArticleIncludeClass(Expression<Func<Article, bool>> queryWhere)
        {
            return ArticleDAL.GetArticleIncludeClass(queryWhere);
        }

        #endregion

        #region 根据条件获取文章关联文章类别的数据（延时加载）

        public ArticleView GetArticleViewBy(Expression<Func<Article, bool>> queryWhere)
        {
            return ArticleDAL.GetArticleViewBy(queryWhere);
        }

        #endregion 根据条件获取文章关联文章类别的数据（延时加载）

        #region 根据ID获取文章关联文章类别的数据列表（延时加载，包含前一条及后一条数据）

        public ArticleNextAndPrevView GetArticleViewNearId(int id)
        {
            ArticleNextAndPrevView result = new ArticleNextAndPrevView();
            result.CurrentArticle = GetArticleViewBy(f => f.Id == id);
            if (result.CurrentArticle == null)
            {
                return result;
            }
            result.NextArticle = GetArticleViewBy(f => f.Id > id && f.ClassId == result.CurrentArticle.ClassId);
            result.PrevArticle = GetArticleViewBy(f => f.Id < id && f.ClassId == result.CurrentArticle.ClassId);
            return result;
        }

        #endregion 根据ID获取文章关联文章类别的数据（延时加载）

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
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy, out int totalCount)
        {
            return ArticleDAL.GetOrderArticleIncludeClassByPage(pageIndex, pageSize, queryWhere, strOrderBy, out totalCount);
        }

        #endregion

        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(Article表用aco表示)</param>
        /// <returns></returns>
        public List<ArticleView> GetArticleIncludeClass(string strWhere)
        {
            return ArticleDAL.GetArticleIncludeClass(strWhere);
        }

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
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, string strWhere,
            string orderBy, out int totalCount)
        {
            return ArticleDAL.GetOrderArticleIncludeClassByPage(pageIndex, pageSize, strWhere, orderBy, out totalCount);
        }

        #endregion

        #region 根据请求条件获取文章关联文章类别数据列表(分页)

        public List<ArticleView> GetArticleList(out int totalCount, ArticleRequest request = null)
        {
            request = request ?? new ArticleRequest();
            Expression<Func<Article, bool>> queryWhere = null;

            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                queryWhere = f => f.Title.Contains(request.Title);
            }
            else
            {
                queryWhere = f => f.IsDel == 0;
            }

            if (request.ClassId > 0)
            {
                if (!string.IsNullOrEmpty(request.ClassPath) && Utils.IsSafeSqlString(request.ClassPath))
                {
                    queryWhere =
                        queryWhere.AndAlso(f => f.ClassId == request.ClassId)
                            .OrAlso(f => f.ClassId.ToString().Contains(request.ClassPath));
                }
                else
                {
                    queryWhere = queryWhere.AndAlso(f => f.ClassId == request.ClassId);
                }
            }
            if (string.IsNullOrEmpty(request.OrderBy) || !CanOrdered(request.OrderBy))
            {
                request.OrderBy = "AddTime desc";
            }
            var articles = GetOrderArticleIncludeClassByPage(request.PageIndex, request.PageSize, queryWhere, request.OrderBy, out totalCount);

            return articles;
        }

        #endregion 根据请求条件获取文章关联文章类别数据列表(分页)

        #region 根据请求条件获取IPageList格式数据
        public IEnumerable<ArticleView> GetArticleList(ArticleRequest request = null)
        {
            request = request ?? new ArticleRequest();
            List<ArticleView> articles = new List<ArticleView>();
            string strWhere = " 1=1 ";

            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                //$" and Title like '%{request.Title}%'";
                strWhere += string.Format(" and Title like '%{0}%' ", request.Title);
            }

            if (request.ClassId > 0)
            {
                if (!string.IsNullOrEmpty(request.ClassPath) && Utils.IsSafeSqlString(request.ClassPath))
                {
                    //$" and (aco.ClassId = {request.Pid} or aco.ClassId in ({request.Pid}))";
                    strWhere += string.Format(" and (aco.ClassId = {0} or aco.ClassId in ({0}))", request.ClassId);
                }
                else
                {
                    //$" and aco.ClassId = {request.Pid}";
                    strWhere += string.Format(" and aco.ClassId = {0}", request.ClassId);
                }
            }

            //articles = GetArticleIncludeClass(strWhere);
            int totalCount = 0;
            articles = GetOrderArticleIncludeClassByPage(request.PageIndex, request.PageSize, strWhere, " aco.AddTime desc", out totalCount);

            //return articles.OrderByDescending(u => u.id).ToPagedList(request.PageIndex, request.PageSize, totalCount);
            return articles.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        #endregion

        #region 根据ID列表批量删除数据

        /// <summary>
        /// 根据ID列表批量删除数据
        /// </summary>
        /// <param name="delWhere">条件Lambda表达式</param>
        /// <returns></returns>
        public int DelList<T>(List<T> delList)
        {
            //string strDelIn = string.Empty;
            //foreach(T item in delList)
            //{
            //    strDelIn+=
            //}
            //return this.DelBy(f=>delList.g);
            return 0;
        }

        #endregion
    }
}
