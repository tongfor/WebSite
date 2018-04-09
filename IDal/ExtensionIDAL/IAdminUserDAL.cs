using System.Collections.Generic;
using Models;
/** 
* IAdminUserDAL.cs
*
* 功 能： AdminUser数据层接口
* 类 名： IAdminUserDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/17 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Linq.Expressions;
using System;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IAdminUserDAL
    {
        #region 根据用户名返回模型

        /// <summary>
        /// 根据用户名返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        AdminUser GetModelByUserName(string username);

        /// <summary>
        /// 异步根据用户名返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<AdminUser> GetModelByUserNameAsync(string username);

        #endregion

        #region  根据用户名和密码返回模型

        /// <summary>
        /// 根据用户名和密码返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AdminUser GetModelByUserNameAndPwd(string username, string password);

        /// <summary>
        /// 异步根据用户名和密码返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<AdminUser> GetModelByUserNameAndPwdAsync(string username, string password);

        #endregion

        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(article表用aco表示)</param>
        /// <returns></returns>
        List<AdminUserView> GetUserIncludeRole(string strWhere);

        /// <summary>
        /// 异步获取用户关联角色的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(article表用aco表示)</param>
        /// <returns></returns>
        Task<List<AdminUserView>> GetUserIncludeRoleAsync(string strWhere);

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool UserCanRegister(string userName);

        /// <summary>
        /// 异步判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
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
    }
}