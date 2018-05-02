/** 
* IAdminUserService.cs
*
* 功 能： IAdminUser逻辑层扩展接口
* 类 名： IAdminUserService
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
using Models;
using Common;
using System.Threading.Tasks;

namespace IBLL
{
    /// <summary>
    /// 后台用户逻辑扩展
    /// </summary>
    partial interface IAdminUserService
    {
        #region 根据用户名获取用户数据

        /// <summary>
        /// 根据用户名获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        AdminUser GetUserByName(string userName);

        /// <summary>
        /// 异步根据用户名获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<AdminUser> GetUserByNameAsync(string userName);

        #endregion

        #region 根据用户名和密码获取用户数据

        /// <summary>
        /// 根据用户名和密码获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AdminUser GetUserByNameAndPassword(string userName, string password);

        /// <summary>
        /// 异步根据用户名和密码获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AdminUser> GetUserByNameAndPasswordAsync(string userName, string password);

        #endregion

        #region 根据请求条件获取用户数据

        /// <summary>
        /// 根据请求条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<AdminUserView> GetUserByRequest(UserRequest request);

        /// <summary>
        /// 异步根据请求条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<AdminUserView>> GetUserByRequestAsync(UserRequest request);

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册true</returns>
        bool UserCanRegister(string userName);

        /// <summary>
        /// 异步判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册true</returns>
        Task<bool> UserCanRegisterAsync(string userName);

        /// <summary>
        /// 获取用户的List
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns></returns>
        List<AdminUser> GetAdminUserByPage<TKey>(int pageIndex, int pageSize, Expression<Func<AdminUser, bool>> queryWhere, Expression<Func<AdminUser, TKey>> orderBy, out int totalCount, bool isdesc = false);

        /// <summary>
        /// 异步获取用户的List
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns></returns>
        Task<PageData<AdminUser>> GetAdminUserByPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminUser, bool>> queryWhere, Expression<Func<AdminUser, TKey>> orderBy, bool isdesc = false);

        /// <summary>
        /// 根据查询条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<AdminUser> GetAdminUserList(AdminUserRequest request);

        /// <summary>
        /// 异步根据查询条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<List<AdminUser>> GetAdminUserListAsync(AdminUserRequest request);
    }
}
