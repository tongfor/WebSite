﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DALMySql
{
    public partial class ArticleClassDAL
    {
        /// <summary>
        /// 文章分类列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns></returns>
        public List<ArticleClass> GetArticleClassByPage<TKey>(int pageIndex, int pageSize,
            Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            List<ArticleClass> articleClassList;
            var dbQuery = _db.Set<ArticleClass>().Where(queryWhere);
            totalCount = dbQuery.Count();
            articleClassList = isdesc
                ? dbQuery.OrderByDescending(orderBy).Skip(skipIndex).Take(pageSize).ToList()
                : dbQuery.OrderBy(orderBy).Skip(skipIndex).Take(pageSize).ToList();
            return articleClassList;
        }

        /// <summary>
        /// 异步文章分类列表
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns>PageData类型，包括DataList和Total</returns>
        public async Task<PageData<ArticleClass>> GetArticleClassByPageAsync<TKey>(int pageIndex, int pageSize,Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, bool isdesc = false)
        {
            PageData<ArticleClass> result = new PageData<ArticleClass>();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            var dbQuery = _db.Set<ArticleClass>().Where(queryWhere);
            
            result.DataList = isdesc
                ? await dbQuery.OrderByDescending(orderBy).Skip(skipIndex).Take(pageSize).ToListAsync()
                : await dbQuery.OrderBy(orderBy).Skip(skipIndex).Take(pageSize).ToListAsync();
            result.TotalCount = dbQuery.Count();
            return result;
        }
    }
}
