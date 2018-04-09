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
using System.Linq.Expressions;
using Common;
using Models;
using IDAL;
using System.Threading.Tasks;

namespace BLL
{
    public partial class ArticleService
    {
        #region 获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public List<ArticleIncludeClassNameView> GetArticleIncludeClass(Expression<Func<Article, bool>> queryWhere)
        {
            return articleDAL.GetArticleIncludeClass(queryWhere);
        }

        /// <summary>
        /// 异步获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public async Task<List<ArticleIncludeClassNameView>> GetArticleIncludeClassAsync(Expression<Func<Article, bool>> queryWhere)
        {
            var result = await articleDAL.GetArticleIncludeClassAsync(queryWhere);
            return result;
        }

        #endregion

        #region 根据条件获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 根据条件获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere"></param>
        /// <returns></returns>
        public ArticleView GetArticleViewBy(Expression<Func<Article, bool>> queryWhere)
        {
            return articleDAL.GetArticleViewBy(queryWhere);
        }

        /// <summary>
        /// 异步根据条件获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere"></param>
        /// <returns></returns>
        public async Task<ArticleView> GetArticleViewByAsync(Expression<Func<Article, bool>> queryWhere)
        {
            var result = await articleDAL.GetArticleViewByAsync(queryWhere);
            return result;
        }

        #endregion 根据条件获取文章关联文章类别的数据（延时加载）

        #region 根据ID获取文章关联文章类别的数据列表（延时加载，包含前一条及后一条数据）

        /// <summary>
        /// 根据ID获取文章关联文章类别的数据列表（延时加载，包含前一条及后一条数据）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ArticleNextAndPrevView GetArticleViewNearId(int id)
        {
            ArticleNextAndPrevView result = new ArticleNextAndPrevView
            {
                CurrentArticle = GetArticleViewBy(f => f.Id == id)
            };
            if (result.CurrentArticle == null)
            {
                return result;
            }
            result.NextArticle = GetArticleViewBy(f => f.Id > id && f.ClassId == result.CurrentArticle.ClassId);
            result.PrevArticle = GetArticleViewBy(f => f.Id < id && f.ClassId == result.CurrentArticle.ClassId);
            return result;
        }

        /// <summary>
        /// 异步根据ID获取文章关联文章类别的数据列表（延时加载，包含前一条及后一条数据）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ArticleNextAndPrevView> GetArticleViewNearIdAsync(int id)
        {
            ArticleNextAndPrevView result = new ArticleNextAndPrevView
            {
                CurrentArticle = await GetArticleViewByAsync(f => f.Id == id)
            };
            if (result.CurrentArticle == null)
            {
                return result;
            }
            result.NextArticle = await GetArticleViewByAsync(f => f.Id > id && f.ClassId == result.CurrentArticle.ClassId);
            result.PrevArticle = await GetArticleViewByAsync(f => f.Id < id && f.ClassId == result.CurrentArticle.ClassId);
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
        /// <returns></returns>
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy, out int totalCount)
        {
            return articleDAL.GetOrderArticleIncludeClassByPage(pageIndex, pageSize, queryWhere, strOrderBy, out totalCount);
        }

        /// <summary>
        /// 异步分页获取文章关联文章类别的数据并排序（延时加载）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <param name="strOrderBy">排序lamdba表达式</param>
        /// <param name="totalCount">总数</param>
        /// <returns>PageData类型，包括DataList和Total</returns>
        public async Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy)
        {
            var result = await articleDAL.GetOrderArticleIncludeClassByPageAsync(pageIndex, pageSize, queryWhere, strOrderBy);
            return result;
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
            return articleDAL.GetArticleIncludeClass(strWhere);
        }

        /// <summary>
        /// 异步获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(Article表用aco表示)</param>
        /// <returns></returns>
        public async Task<List<ArticleView>> GetArticleIncludeClassAsync(string strWhere)
        {
            var result = await articleDAL.GetArticleIncludeClassAsync(strWhere);
            return result;
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
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, string strWhere, string orderBy, out int totalCount)
        {
            return articleDAL.GetOrderArticleIncludeClassByPage(pageIndex, pageSize, strWhere, orderBy, out totalCount);
        }

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strWhere">查询条件(Article表用aco表示),必传</param>
        /// <param name="orderBy">排序(Article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns>PageData类型，包括DataList和Total</returns>
        public async Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, string strWhere, string orderBy)
        {
            var result = await articleDAL.GetOrderArticleIncludeClassByPageAsync(pageIndex, pageSize, strWhere, orderBy);
            return result;
        }

        #endregion

        #region 根据请求条件获取文章关联文章类别数据列表(分页)

        /// <summary>
        /// 根据请求条件获取文章关联文章类别数据列表(分页)
        /// </summary>
        /// <param name="totalCount"></param>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 根据请求条件获取文章关联文章类别数据列表(分页)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async  Task<PageData<ArticleView>> GetArticleListAsync(ArticleRequest request = null)
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
            var result = await GetOrderArticleIncludeClassByPageAsync(request.PageIndex, request.PageSize, queryWhere, request.OrderBy);

            return result;
        }

        #endregion 根据请求条件获取文章关联文章类别数据列表(分页)

        #region 根据请求条件获取IPageList格式数据

        /// <summary>
        /// 根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ArticleView>> GetArticleListBySqlAsync(ArticleRequest request = null)
        {
            request = request ?? new ArticleRequest();
            List<ArticleView> result = new List<ArticleView>();
            string strWhere = " 1=1 ";

            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                strWhere += string.Format(" and Title like '%{0}%' ", request.Title);
            }

            if (request.ClassId > 0)
            {
                if (!string.IsNullOrEmpty(request.ClassPath) && Utils.IsSafeSqlString(request.ClassPath))
                {
                    strWhere += string.Format(" and (aco.ClassId = {0} or aco.ClassId in ({0}))", request.ClassId);
                }
                else
                {
                    strWhere += string.Format(" and aco.ClassId = {0}", request.ClassId);
                }
            }
            
            var articles = await GetOrderArticleIncludeClassByPageAsync(request.PageIndex, request.PageSize, strWhere, " aco.AddTime desc");

            return articles.DataList.ToPagedList(request.PageIndex, request.PageSize, articles.TotalCount);
        }

        #endregion
    }
}
