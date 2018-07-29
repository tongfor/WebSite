/** 
* AdminLoginLogService.cs
*
* 功 能： AdminLoginLog逻辑层
* 类 名： AdminLoginLogService
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
    public partial class AdminLoginLogService
    {
        #region 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录

        /// <summary>
        /// 判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录
        /// </summary>
        /// <param name="maxErrorCount">最大错误次数,如果小于等于0则不判断</param>
        /// <param name="tyrMinutes">多少分钟后可重新登录</param>
        /// <param name="ip">用户ip</param>
        /// <param name="lastLoginTime">输出参数：如果30分钟没有5次的失败登录，那么返回null；如果有，就返回下一次可以登录的时间</param>
        public bool CheckLoginErrorCount(int maxErrorCount, int tyrMinutes, string ip, out DateTime? lastLoginTime)
        {
            return MyIAdminLoginLogDAL.CheckLoginErrorCount(maxErrorCount, tyrMinutes, ip, out lastLoginTime);
        }

        /// <summary>
        /// 异步判断登录：如果30分钟内同一个ip连续最大错误次数次登录失败，那么30分钟后才可以继续登录
        /// </summary>
        /// <param name="maxErrorCount">最大错误次数,如果小于等于0则不判断</param>
        /// <param name="tryMinutes">多少分钟后可重新登录</param>
        /// <param name="ip">用户ip</param>
        /// <returns>元组，</returns>
        public async Task<Tuple<bool, DateTime>> CheckLoginErrorCountAsync(int maxErrorCount, int tryMinutes, string ip)
        {
            Tuple<bool, DateTime> result = await MyIAdminLoginLogDAL.CheckLoginErrorCountAsync(maxErrorCount, tryMinutes, ip);
            return result;
        }

        #endregion

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
        public List<AdminLoginLog> GetOrderLoginLogViewByPage<TKey>(int pageIndex, int pageSize, Expression<Func<AdminLoginLog, bool>> queryWhere, Expression<Func<AdminLoginLog, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            return MyIAdminLoginLogDAL.GetListForLoginLogAdmin(pageIndex, pageSize, queryWhere, orderBy, out totalCount, isdesc);
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
        public async Task<PageData<AdminLoginLog>> GetOrderLoginLogViewByPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminLoginLog, bool>> queryWhere, Expression<Func<AdminLoginLog, TKey>> orderBy, bool isdesc = false)
        {
            return await MyIAdminLoginLogDAL.GetListForLoginLogAdminAsync(pageIndex, pageSize, queryWhere, orderBy, isdesc);
        }

        #region 日志IPageList格式数据

        /// <summary>
        /// 日志IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminLoginLog> GetAdminLoginLogForAdminList(BaseRequest request)
        {
            request = request ?? new BaseRequest();
            List<AdminLoginLog> getAdminLoginLogList = new List<AdminLoginLog>();
            int totalCount = 0;
            Expression<Func<AdminLoginLog, bool>> queryWhere = null;

            if (!string.IsNullOrEmpty(request.Title))//如果查询标题不为空
            {
                queryWhere = (a => a.UserName.Contains(request.Title));
            }
            getAdminLoginLogList = GetOrderLoginLogViewByPage<DateTime?>(request.PageIndex, request.PageSize, queryWhere, a => a.LoginTime, out totalCount, true);
            return getAdminLoginLogList.ToPagedList(request.PageIndex, request.PageSize, totalCount); ;
        }

        /// <summary>
        /// 日志IPageList格式数据(异步)
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdminLoginLog>> GetAdminLoginLogForAdminListAsync(BaseRequest request)
        {
            request = request ?? new BaseRequest();
            var pageData = new PageData<AdminLoginLog>();
            List<AdminLoginLog> getAdminLoginLogList = new List<AdminLoginLog>();
            Expression<Func<AdminLoginLog, bool>> queryWhere = null;

            if (!string.IsNullOrEmpty(request.Title))//如果查询标题不为空
            {
                queryWhere = (a => a.UserName.Contains(request.Title));
            }
            pageData = await GetOrderLoginLogViewByPageAsync<DateTime?>(request.PageIndex, request.PageSize, queryWhere, a => a.LoginTime, true);
            return pageData.DataList.ToPagedList(request.PageIndex, request.PageSize, pageData.TotalCount); 
        }

        #endregion
    }
}
