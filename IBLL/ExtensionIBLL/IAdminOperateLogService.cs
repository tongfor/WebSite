﻿/** 
* IAdminOperateLogService.cs
*
* 功 能： IAdminOperateLog逻辑层
* 类 名： IAdminOperateLogService
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

using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IAdminOperateLogService
    {
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
        List<AdminOperateLog> GetListForOperateLogAdmin<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, out int totalCount, bool isdesc = false);

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
        Task<PageData<AdminOperateLog>> GetListForOperateLogAdminAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, bool isdesc = false);

        #region 操作日志IPageList格式数据

        /// <summary>
        /// 操作日志IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<AdminOperateLog> GetOperateLogForAdminList(BaseRequest request);

        /// <summary>
        /// 异步获取操作日志IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<AdminOperateLog>> GetOperateLogForAdminListAsync(BaseRequest request);

        #endregion
    }
}
