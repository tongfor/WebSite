/** 
* AdminUserService.cs
*
* 功 能： AdminUser逻辑层扩展
* 类 名： AdminUserService
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
using System.Linq;
using System.Linq.Expressions;
using Models;
using Common;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 后台用户逻辑扩展
    /// </summary>
    public partial class AdminUserService
    {
        #region 根据用户名获取用户数据

        /// <summary>
        /// 根据用户名获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public AdminUser GetUserByName(string userName)
        {
            var adminUserInfo = adminUserDAL.GetModelByUserName(userName);
            return adminUserInfo;
        }

        /// <summary>
        /// 异步根据用户名获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<AdminUser> GetUserByNameAsync(string userName)
        {
            var adminUserInfo = await adminUserDAL.GetModelByUserNameAsync(userName);
            return adminUserInfo;
        }

        #endregion

        #region 根据用户名和密码获取用户数据

        /// <summary>
        /// 根据用户名和密码获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AdminUser GetUserByNameAndPassword(string userName, string password)
        {
            AdminUser adminUserInfo = null;
            string passwordMd5 = password.MD5Encrypt();
            adminUserInfo = adminUserDAL.GetModelByUserNameAndPwd(userName, passwordMd5);
            return adminUserInfo;
        }

        /// <summary>
        /// 异步根据用户名和密码获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AdminUser> GetUserByNameAndPasswordAsync(string userName, string password)
        {
            AdminUser adminUserInfo = null;
            string passwordMd5 = password.MD5Encrypt();
            adminUserInfo = await adminUserDAL.GetModelByUserNameAndPwdAsync(userName, passwordMd5);
            return adminUserInfo;
        }

        #endregion

        #region 根据请求条件获取用户数据

        /// <summary>
        /// 根据请求条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminUserView> GetUserByRequest(UserRequest request)
        {
            request = request ?? new UserRequest();
            List<AdminUserView> users = new List<AdminUserView>();

            if (request.RoleId > 0)
            {
                var strWhere = string.Format("Roleid={0}", request.RoleId);
                users = adminUserDAL.GetUserIncludeRole(strWhere).ToList();
            }

            return users.OrderByDescending(u => u.Id).ToPagedList(request.PageIndex, request.PageSize); ;
        }

        /// <summary>
        /// 异步根据请求条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdminUserView>> GetUserByRequestAsync(UserRequest request)
        {
            request = request ?? new UserRequest();
            List<AdminUserView> users = new List<AdminUserView>();

            if (request.RoleId > 0)
            {
                var strWhere = string.Format("Roleid={0}", request.RoleId);
                users = await adminUserDAL.GetUserIncludeRoleAsync(strWhere);
            }

            return users.OrderByDescending(u => u.Id).ToPagedList(request.PageIndex, request.PageSize); ;
        }

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册true</returns>
        public bool UserCanRegister(string userName)
        {
            return adminUserDAL.UserCanRegister(userName);
        }

        /// <summary>
        /// 异步判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册true</returns>
        public async Task<bool> UserCanRegisterAsync(string userName)
        {
            return await adminUserDAL.UserCanRegisterAsync(userName);
        }

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
        public List<AdminUser> GetAdminUserByPage<TKey>(int pageIndex, int pageSize, Expression<Func<AdminUser, bool>> queryWhere, Expression<Func<AdminUser, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            return adminUserDAL.GetAdminUserByPage<TKey>(pageIndex, pageSize, queryWhere, orderBy, out  totalCount, isdesc);
        }

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
        public async Task<PageData<AdminUser>> GetAdminUserByPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<AdminUser, bool>> queryWhere, Expression<Func<AdminUser, TKey>> orderBy, bool isdesc = false)
        {
            var pageData = new PageData<AdminUser>();
            pageData = await adminUserDAL.GetAdminUserByPageAsync<TKey>(pageIndex, pageSize, queryWhere, orderBy, isdesc);
            return pageData;
        }

        /// <summary>
        /// 根据查询条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminUser> GetAdminUserList(AdminUserRequest request)
        {
            List<AdminUser> adminUserList = null;
            int totalCount = 0;
            Expression<Func<AdminUser, bool>> queryWhere = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                queryWhere = a => a.UserName.Contains(request.Title);
            }
            else
            {
                queryWhere = a => true;
            }
            adminUserList = GetAdminUserByPage<int>(request.PageIndex, request.PageSize, queryWhere, p => p.Id, out totalCount, false);
            return adminUserList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        /// <summary>
        /// 异步根据查询条件获取用户数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<AdminUser>> GetAdminUserListAsync(AdminUserRequest request)
        {
            PageData<AdminUser> pageData = null;
            Expression<Func<AdminUser, bool>> queryWhere = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                queryWhere = a => a.UserName.Contains(request.Title);
            }
            else
            {
                queryWhere = a => true;
            }
            pageData = await GetAdminUserByPageAsync<int>(request.PageIndex, request.PageSize, queryWhere, p => p.Id, false);
            return pageData.DataList.ToPagedList(request.PageIndex, request.PageSize, pageData.TotalCount);
        }
    }
}
