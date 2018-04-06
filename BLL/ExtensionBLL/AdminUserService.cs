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
using System.Text;
using System.Threading.Tasks;
using IDAL;
using Models;
using Common;
using Common.Config;
using Common.Html;

namespace BLL
{
    /// <summary>
    /// 后台用户逻辑扩展
    /// </summary>
    public partial class AdminUserService
    {
        protected IAdminUserDAL AdminUserDAL = new DALSession().IAdminUserDAL;

        /// <summary>
        /// 站点设置
        /// </summary>
        private readonly SiteConfig _siteConfig = new SiteConfig();

        #region 根据用户名获取用户数据
        /// <summary>
        /// 根据用户名获取用户数据
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public AdminUser GetUserByName(string userName)
        {
            var adminUserInfo = AdminUserDAL.GetModelByUserName(userName);
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
            adminUserInfo = AdminUserDAL.GetModelByUserNameAndPwd(userName, passwordMd5);
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
                users = AdminUserDAL.GetUserIncludeRole(strWhere).ToList();
            }

            return users.OrderByDescending(u => u.Id).ToPagedList(request.PageIndex, request.PageSize); ;
        }
        #endregion

        #region 获取用户关联分组的数据（直接执行查询语句）

        /// <summary>
        /// 获取用户关联分组的数据（直接执行查询语句）
        /// </summary>
        /// <param name="groupId">组ID</param>
        /// <param name="strWhere">查询条件(ProjectBase表用pb表示,CreativeActivityProjectShip用ap表示，EntrepreneurshipActivity用ea表示)</param>
        /// <returns></returns>
        /// <returns></returns>
        public List<UserIncludeGroupView> GetUserIncludeGroup(int groupId, string strWhere)
        {
            return AdminUserDAL.GetUserIncludeGroup(groupId, strWhere);
        }

        #endregion

        #region 分页获取用户关联分组的数据并排序（直接执行查询语句）

        /// <summary>
        /// 获取用户关联分组的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="groupId">组ID</param>
        /// <param name="strWhere">查询条件(article表用aco表示),必传</param>
        /// <param name="orderBy">排序(article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        public List<UserIncludeGroupView> GetOrderProjectIncludeActivityByPage(int pageIndex, int pageSize, int groupId, string strWhere,
            string orderBy, out int totalCount)
        {
            return AdminUserDAL.GetUserIncludeGroupByPage(pageIndex, pageSize, groupId, strWhere, orderBy,
                out totalCount);
        }

        #endregion

        #region 未分配组的用户的IPageList格式数据

        public IEnumerable<UserIncludeGroupView> GetUnallotUserList(int groupId, BaseRequest request)
        {
            request = request ?? new BaseRequest();
            List<UserIncludeGroupView> userList = new List<UserIncludeGroupView>();
            int totalCount = 0;
            string strWhere = " au.IsAble>0 and auar.RoleId=" + _siteConfig.ProfessorRoleId + " ";
            if (groupId <= 0)
            {
                return userList.ToPagedList(request.PageIndex, request.PageSize, 0);
            }
            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                strWhere = strWhere + " and au.UserName like '%" + request.Title + "%'";
            }
            userList = GetOrderProjectIncludeActivityByPage(request.PageIndex, request.PageSize, groupId, strWhere, " id asc", out totalCount);
            return userList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool UserCanRegister(string userName)
        {
            return AdminUserDAL.UserCanRegister(userName);
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
            return AdminUserDAL.GetAdminUserByPage<TKey>(pageIndex, pageSize, queryWhere, orderBy, out  totalCount, isdesc);
        }
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

    }
}
