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
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Models;

namespace DALMySql
{
    public partial class ArticleDAL
    {
        //EF上下文
        private readonly DbContext _db = new DbContextFactory().GetDbContext();


        #region 获取文章关联文章类别的数据（延时加载）

        /// <summary>
        /// 获取文章关联文章类别的数据（延时加载）
        /// </summary>
        /// <param name="queryWhere">对Article的查询lamdba表达式</param>
        /// <returns></returns>
        public List<ArticleIncludeClassNameView> GetArticleIncludeClass(Expression<Func<Article, bool>> queryWhere)
        {
            List<ArticleIncludeClassNameView> articleList = new List<ArticleIncludeClassNameView>();
            DbQuery<ArticleIncludeClassNameView> dbQuery = _db.Set<Article>().Where(queryWhere)
                .Join(_db.Set<ArticleClass>(), aco => aco.ClassId, acl => acl.Id,
                    (aco, acl) => new ArticleIncludeClassNameView { Article = aco, ArticleClassName = acl.Name }) as DbQuery<ArticleIncludeClassNameView>;
            if (dbQuery != null)
            {
                articleList = dbQuery.ToList();
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
                    (a, ac) => new ArticleView()
                    {
                        Id = a.Id,
                        ClassId = a.ClassId,
                        Title = a.Title,
                        TitleColor = a.TitleColor,
                        Content = a.Content,
                        UserName = a.UserName,
                        LookCount = a.LookCount,
                        AddHTMLUrl = a.AddHTMLUrl,
                        IsTop = a.IsTop,
                        IsMarquee = a.IsMarquee,
                        Introduce = a.Introduce,
                        IntroduceImg = a.IntroduceImg,
                        AddTime = a.AddTime,
                        EditTime = a.EditTime,
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
            articleViewList = articleViewList.Skip(skipIndex).Take(pageSize).ToList();
            totalCount = dbQuery.Count();
            return articleViewList;
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
            var queryResult =
                _db.Database.SqlQuery<ArticleView>(
                    "SELECT aco.*,acl.Name as ArticleClassName from Article as aco LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id where 1=1 and " +
                    strWhere);
            if (queryResult != null)
            {
                articleList = queryResult.ToList();
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
        public List<ArticleView> GetOrderArticleIncludeClassByPage(int pageIndex, int pageSize, string strWhere,
            string orderBy, out int totalCount)
        {

            List<ArticleView> articleList = new List<ArticleView>();
            totalCount = 0;
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            string strsql = string.Empty;
            strsql +=
                "SELECT aco.*,acl.Name as ArticleClassName from Article as aco LEFT JOIN ArticleClass as acl on aco.ClassId=acl.id where 1=1 and ";
            strsql += strWhere;
            if (!string.IsNullOrEmpty(orderBy))
            {
                strsql += " order by " + orderBy;
            }

            var queryResult = _db.Database.SqlQuery<ArticleView>(strsql);
            if (queryResult != null)
            {
                articleList = queryResult.Skip(skipIndex).Take(pageSize).ToList();
                totalCount = queryResult.Count();
            }
            return articleList;
        }

        #endregion
    }
}
