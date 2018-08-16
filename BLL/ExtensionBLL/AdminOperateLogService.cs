/** 
* AdminOperateLogService.cs
*
* 功 能： AdminOperateLog逻辑层
* 类 名： AdminOperateLogService
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

using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL
{
    public partial class AdminOperateLogService
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
        public List<AdminOperateLog> GetListForOperateLogAdmin<TKey>(int pageIndex, int pageSize, Expression<Func<AdminOperateLog, bool>> queryWhere, Expression<Func<AdminOperateLog, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            return MyIAdminOperateLogDAL.GetListForOperateLogAdmin(pageIndex, pageSize, queryWhere, orderBy, out totalCount, isdesc);
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
            return await MyIAdminOperateLogDAL.GetListForOperateLogAdminAsync(pageIndex, pageSize, queryWhere, orderBy, isdesc);
        }

        #region 操作日志IPageList格式数据

        /// <summary>
        /// 操作日志IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminOperateLog> GetOperateLogForAdminList(BaseRequest request)
        {
            request = request ?? new BaseRequest();
            List<AdminOperateLog> getAdminOperateLogList = new List<AdminOperateLog>();
            int totalCount = 0;
            Expression<Func<AdminOperateLog, bool>> queryWhere = null;

            if (!string.IsNullOrEmpty(request.Title))//如果查询标题不为空
            {
                queryWhere = (a => a.UserName.Contains(request.Title));
            }
            getAdminOperateLogList = GetListForOperateLogAdmin<DateTime?>(request.PageIndex, request.PageSize, queryWhere, a => a.OperateTime, out totalCount, true);
            return getAdminOperateLogList.ToPagedList(request.PageIndex, request.PageSize, totalCount); ;
        }

        /// <summary>
        /// 操作日志IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdminOperateLog>> GetOperateLogForAdminListAsync(BaseRequest request)
        {
            request = request ?? new BaseRequest();
            PageData<AdminOperateLog> pageData = new PageData<AdminOperateLog>();
            int totalCount = 0;
            Expression<Func<AdminOperateLog, bool>> queryWhere = null;

            if (!string.IsNullOrEmpty(request.Title))//如果查询标题不为空
            {
                queryWhere = (a => a.UserName.Contains(request.Title));
            }
            pageData = await GetListForOperateLogAdminAsync<DateTime?>(request.PageIndex, request.PageSize, queryWhere, a => a.OperateTime, true);
            pageData.DataList = pageData.DataList ?? new List<AdminOperateLog>();
            return pageData.DataList.ToPagedList(request.PageIndex, request.PageSize, totalCount); ;
        }

        #endregion
    }
}
