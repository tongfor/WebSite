/** 
* ArticleDAL.cs
*
* 功 能： 数据层Article接口MYSQL实现
* 类 名： ArticleDAL
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
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DALMySql
{
    public partial class ArticleDAL
    {
        #region annotation
        //public Article GetArticleById(string id)
        //{
        //    var dbQuery = _db.Article.FromSql($"Select * from Article where id = {id}");
        //    //_db.Article.FromSql($"Select * from Article where id = {0}", id);
        //    if (dbQuery!=null)
        //    {
        //        return dbQuery.FirstOrDefault();
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion annotation

        #region 获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public List<ArticleIncludeClassNameView> GetArticleIncludeClass(Expression<Func<Article, bool>> queryWhere)
        {
            List<ArticleIncludeClassNameView> articleList = new List<ArticleIncludeClassNameView>();
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), aco => aco.ClassId, acl => acl.Id,
                    (aco, acl) => new ArticleIncludeClassNameView { Article = aco, ArticleClassName = acl.Name });
            if (dbQuery != null)
            {
                articleList = dbQuery.ToList();
            }
            return articleList;
        }

        /// <summary>
        /// 异步获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public async Task<List<ArticleIncludeClassNameView>> GetArticleIncludeClassAsync(Expression<Func<Article, bool>> queryWhere)
        {
            List<ArticleIncludeClassNameView> articleList = new List<ArticleIncludeClassNameView>();
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), aco => aco.ClassId, acl => acl.Id,
                    (aco, acl) => new ArticleIncludeClassNameView { Article = aco, ArticleClassName = acl.Name });
            if (dbQuery != null)
            {
                articleList = await dbQuery.ToListAsync();
            }
            return articleList;
        }

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
        /// <returns></returns>
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy, out int totalCount)
        {
            totalCount = 0;
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), a => a.ClassId, ac => ac.Id,
                    (a, ac) => new ArticleView(a)
                    {
                        ArticleClassName = ac.Name
                    });
            var orderQuery = dbQuery;
            Type type = typeof(ArticleView);
            string[] orderByArray = strOrderBy.Split(',');
            foreach (string s in orderByArray)
            {
                string[] orderBy = s.TrimStart().TrimEnd().Split(' ');
                //排序属性名
                string orderProperty = orderBy[0];
                //正序或反序，正序可省略
                string order = orderBy.Length > 1 ? orderBy[1] : "asc";
                PropertyInfo property = type.GetProperty(orderProperty); //获取指定名称的属性
                if (property == null)
                {
                    continue;
                }
                var parameter = Expression.Parameter(type, "o");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                if (order.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending",
                        new Type[] { typeof(ArticleView), property.PropertyType }, orderQuery.Expression,
                        Expression.Quote(orderByExp));
                    orderQuery = orderQuery.Provider.CreateQuery<ArticleView>(resultExp);
                }
                else
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] { typeof(ArticleView), property.PropertyType }, orderQuery.Expression,
                        Expression.Quote(orderByExp));
                    orderQuery = orderQuery.Provider.CreateQuery<ArticleView>(resultExp);
                }
            }
            var articleViewList = orderQuery.ToList();
            articleViewList = articleViewList.Skip(skipIndex).Take(pageSize).OrderByDescending(o => o.AddTime).ToList();
            totalCount = dbQuery.Count();
            return articleViewList;
        }       

        /// <summary>
        /// 异步分页获取文章关联文章类别的数据并排序（延时加载）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <param name="strOrderBy">排序lamdba表达式</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public async Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, Expression<Func<Article, bool>> queryWhere, string strOrderBy)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), a => a.ClassId, ac => ac.Id,
                    (a, ac) => new ArticleView(a)
                    {
                        ArticleClassName = ac.Name
                    });
            var orderQuery = dbQuery;
            Type type = typeof(ArticleView);
            string[] orderByArray = strOrderBy.Split(',');
            foreach (string s in orderByArray)
            {
                string[] orderBy = s.TrimStart().TrimEnd().Split(' ');
                //排序属性名
                string orderProperty = orderBy[0];
                //正序或反序，正序可省略
                string order = orderBy.Length > 1 ? orderBy[1] : "asc";
                PropertyInfo property = type.GetProperty(orderProperty); //获取指定名称的属性
                if (property == null)
                {
                    continue;
                }
                var parameter = Expression.Parameter(type, "o");
                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var orderByExp = Expression.Lambda(propertyAccess, parameter);
                if (order.Equals("desc", StringComparison.CurrentCultureIgnoreCase))
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderByDescending",
                        new Type[] { typeof(ArticleView), property.PropertyType }, orderQuery.Expression,
                        Expression.Quote(orderByExp));
                    orderQuery = orderQuery.Provider.CreateQuery<ArticleView>(resultExp);
                }
                else
                {
                    MethodCallExpression resultExp = Expression.Call(typeof(Queryable), "OrderBy",
                        new Type[] { typeof(ArticleView), property.PropertyType }, orderQuery.Expression,
                        Expression.Quote(orderByExp));
                    orderQuery = orderQuery.Provider.CreateQuery<ArticleView>(resultExp);
                }
            }
            PageData<ArticleView> result = new PageData<ArticleView>();
            result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).ToListAsync();
            result.TotalCount = await dbQuery.CountAsync();
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
            List<ArticleView> articleList = new List<ArticleView>();
            StringBuilder sb = new StringBuilder();
            sb.Append($"SELECT aco.*,acl.Name as ArticleClassName from Article as aco");
            sb.Append(" LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            var queryResult =
                _db.Set<ArticleView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                articleList = queryResult.ToList();
            }
            return articleList;
        }

        /// <summary>
        /// 异步获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(Article表用aco表示)</param>
        /// <returns></returns>
        public async Task<List<ArticleView>> GetArticleIncludeClassAsync(string strWhere)
        {
            List<ArticleView> articleList = new List<ArticleView>();
            StringBuilder sb = new StringBuilder();
            sb.Append($"SELECT aco.*,acl.Name as ArticleClassName from Article as aco");
            sb.Append(" LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            var queryResult =
                _db.Set<ArticleView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                articleList = await queryResult.ToListAsync();
            }
            return articleList;
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
            List<ArticleView> articleList = new List<ArticleView>();
            totalCount = 0;
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT aco.*,acl.Name as ArticleClassName from Article as aco ");
            sb.Append(" LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sb.AppendFormat(" order by {0} ", orderBy);
            }

            var queryResult = _db.Set<ArticleView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                articleList = queryResult.Skip(skipIndex).Take(pageSize).OrderByDescending(o => o.AddTime).ToList();
                totalCount = queryResult.Count();
            }
            return articleList;
        }

        /// <summary>
        /// 异步获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="strWhere">查询条件(Article表用aco表示),必传</param>
        /// <param name="orderBy">排序(Article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns>PageData类型，包括DataList和TotalCount</returns>
        public async Task<PageData<ArticleView>> GetOrderArticleIncludeClassByPageAsync(int pageIndex, int pageSize, string strWhere, string orderBy)
        {
            PageData<ArticleView> result = null;
            
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT aco.*,acl.Name as ArticleClassName from Article as aco ");
            sb.Append(" LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            if (!string.IsNullOrEmpty(orderBy))
            {
                sb.AppendFormat(" order by {0} ", orderBy);
            }

            var queryResult = _db.Set<ArticleView>().FromSql(sb.ToString());
            result = new PageData<ArticleView>
            {
                DataList = await queryResult.Skip(skipIndex).Take(pageSize).OrderByDescending(o => o.AddTime).ToListAsync(),
                TotalCount = await queryResult.CountAsync()
            };
            return result;
        }

        #endregion

        #region 根据ID获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 根据ID获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere"></param>
        /// <returns></returns>
        public ArticleView GetArticleViewBy(Expression<Func<Article, bool>> queryWhere)
        {
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), a => a.ClassId, ac => ac.Id,
                    (a, ac) => new ArticleView(a)
                    {
                        ArticleClassName = ac.Name
                    });
            ArticleView av;
            try
            {
                av = dbQuery.AsNoTracking().FirstOrDefault();
            }
            catch
            {
                av = null;
            }
            return av;
        }

        /// <summary>
        /// 异步根据ID获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere"></param>
        /// <returns></returns>
        public async Task<ArticleView> GetArticleViewByAsync(Expression<Func<Article, bool>> queryWhere)
        {
            var dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), a => a.ClassId, ac => ac.Id,
                    (a, ac) => new ArticleView(a)
                    {
                        ArticleClassName = ac.Name
                    });
            ArticleView av;
            try
            {
                av = await dbQuery.AsNoTracking().FirstOrDefaultAsync();
            }
            catch
            {
                av = null;
            }
            return av;
        }

        #endregion 根据ID获取文章关联文章类别的数据（延时加载）

        #region 点击量累加

        /// <summary>
        /// 根据文章ID点击量累加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int AccumulationClickCount(int id)
        {
            string strSql = $"update Article set LookCount = LookCount + 1 where {id}";
            int resultCount = _db.Database.ExecuteSqlCommand(strSql);
            return resultCount;
        }

        /// <summary>
        /// 异步根据文章ID点击量累加
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<int> AccumulationClickCountAsync(int id)
        {
            string strSql = $"update Article set LookCount = LookCount + 1 where {id}";
            int resultCount = await _db.Database.ExecuteSqlCommandAsync(strSql);
            return resultCount;
        }

        #endregion 点击量累加
    }
}
