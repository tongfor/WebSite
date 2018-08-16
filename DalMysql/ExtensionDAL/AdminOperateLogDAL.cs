/** 
* AdminOperateLogDAL.cs
*
* 功 能： AdminOperateLog数据层扩展实现
* 类 名： AdminOperateLogDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/20 17:05:34   李庸    初版
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
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DALMySql
{
    /// <summary>
    /// 账号登录日志DAL
    /// </summary>
    public partial class AdminOperateLogDAL
    {
        #region 获取后台list数据

        /// <summary>
        /// 获取后台list数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="totalCount">总页数</param>
        /// <param name="isdesc">升降序</param>
        /// <returns></returns>
        public List<AdminOperateLog> GetListForOperateLogAdmin<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            List<AdminOperateLog> adminOperateLogList = new List<AdminOperateLog>();
            if (isdesc)
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminOperateLog>().Where(queryWhere).OrderByDescending(orderBy);
                    totalCount = dbQuery.Count();
                    adminOperateLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
                else
                {
                    var dbQuery = _db.Set<AdminOperateLog>().OrderByDescending(orderBy);
                    totalCount = dbQuery.Count();
                    adminOperateLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
            }
            else
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminOperateLog>().Where(queryWhere).OrderBy(orderBy);
                    totalCount = dbQuery.Count();
                    adminOperateLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }
                else
                {
                    var dbQuery = _db.Set<AdminOperateLog>().OrderBy(orderBy);
                    totalCount = dbQuery.Count();
                    adminOperateLogList = dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToList();
                }

            }
            return adminOperateLogList;
        }

        /// <summary>
        /// 异步获取后台list数据
        /// </summary>
        /// <typeparam name="TKey">排序字段</typeparam>
        /// <param name="pageIndex">索引页</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="queryWhere">过滤条件</param>
        /// <param name="orderBy">排序条件</param>
        /// <param name="totalCount">总页数</param>
        /// <param name="isdesc">升降序</param>
        /// <returns></returns>
        public async Task<PageData<AdminOperateLog>> GetListForOperateLogAdminAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, bool isdesc = false)
        {
            PageData<AdminOperateLog> result = new PageData<AdminOperateLog>();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;           
            if (isdesc)
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminOperateLog>().Where(queryWhere).OrderByDescending(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
                else
                {
                    var dbQuery = _db.Set<AdminOperateLog>().OrderByDescending(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
            }
            else
            {
                if (queryWhere != null)
                {
                    var dbQuery = _db.Set<AdminOperateLog>().Where(queryWhere).OrderBy(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }
                else
                {
                    var dbQuery = _db.Set<AdminOperateLog>().OrderBy(orderBy);
                    result.TotalCount = await dbQuery.CountAsync();
                    result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).AsNoTracking().ToListAsync();
                }

            }
            return result;
        }

        #endregion
    }
}
