/** 
* AdminUserDAL.cs
*
* 功 能： AdminUser数据层扩展实现
* 类 名： AdminUserDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/17 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DALMySql
{
    public partial class AdminUserDAL
    {
        //EF上下文
        private readonly CdyhcdDBContext _db;

        #region 根据用户名返回模型

        /// <summary>
        /// 根据用户名返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public AdminUser GetModelByUserName(string username)
        {
            AdminUser adminUser = null;
            adminUser =
                _db.Set<AdminUser>()
                    .SingleOrDefault(f => f.UserName == username && f.IsAble != null && f.IsAble.Value == 1);
            //adminUser = _db.Set<AdminUser>().FirstOrDefault(f => f.UserName == username && f.IsAble != null && f.IsAble.Value == 1);
            return adminUser;
        }

        #endregion

        #region  根据用户名和密码返回模型
        /// <summary>
        /// 根据用户名和密码返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public AdminUser GetModelByUserNameAndPwd(string username, string password)
        {
            AdminUser adminUser = null;
            adminUser =
                _db.Set<AdminUser>()
                    .SingleOrDefault(
                        f => f.UserName == username && f.UserPwd == password && f.IsAble != null && f.IsAble.Value == 1);
            return adminUser;
        }
        #endregion

        #region 获取用户关联角色的数据（直接执行查询语句）

        /// <summary>
        /// 获取用户关联角色的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(article表用aco表示)</param>
        /// <returns></returns>
        public List<AdminUserView> GetUserIncludeRole(string strWhere)
        {
            List<AdminUserView> userList = new List<AdminUserView>();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select au.*,ul.RoleId as RoleId,ar.RoleName as RoleName from AdminUser as au ");
            sb.Append("left join AdminUserAdminRole as ul on au.id=ul.UserId ");
            sb.Append("left join AdminRole as ar on ul.RoleId=ar.Id ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append("where 1=1 and ");
                sb.Append(strWhere);
            }
            var queryResult =
                _db.Set<AdminUserView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                userList = queryResult.ToList();
            }
            return userList;
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
            List<UserIncludeGroupView> projectEntryList = new List<UserIncludeGroupView>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select au.*,auar.RoleId as RoleId,up.GroupId as GroupId from AdminUser as au ");
            sb.Append(" left join AdminUserAdminRole as auar on au.id=auar.UserId ");
            sb.AppendFormat(" left join UserProjectGropuInfo as up on au.id=up.UserId  and up.GroupId= {0}", groupId);
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append("where 1=1 and ");
                sb.Append(strWhere);
            }
            var queryResult = _db.Set<UserIncludeGroupView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                projectEntryList = queryResult.ToList();
            }
            return projectEntryList;
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
        public List<UserIncludeGroupView> GetUserIncludeGroupByPage(int pageIndex, int pageSize, int groupId, string strWhere,
            string orderBy, out int totalCount)
        {

            List<UserIncludeGroupView> projectEntryList = new List<UserIncludeGroupView>();
            totalCount = 0;
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int beginIndex = (pageIndex - 1) * pageSize + 1;
            int endIndex = pageIndex * pageSize;
            int skipIndex = (pageIndex - 1) * pageSize;
            StringBuilder sb = new StringBuilder();
            sb.Append("select au.*,auar.RoleId as RoleId,up.GroupId as GroupId from AdminUser as au ");
            sb.Append(" left join AdminUserAdminRole as auar on au.id=auar.UserId ");
            sb.AppendFormat(" left join UserProjectGropuInfo as up on au.id=up.UserId and up.GroupId={0}", groupId);
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append("where 1=1 and ");
                sb.Append(strWhere);
            }

            StringBuilder pageSb = new StringBuilder();
            pageSb.Append("select * from (select row_number() over(order by T.id) as Rownum,T.* from ");
            pageSb.Append(" ( " + sb + ") as T) as TT ");
            pageSb.AppendFormat(" where  TT.Rownum between {0} and {1} ", beginIndex, endIndex);
            if (!string.IsNullOrEmpty(orderBy))
            {
                pageSb.AppendFormat(" order by {0}", orderBy);
            }

            var queryResult = _db.Set<UserIncludeGroupView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                projectEntryList = _db.Set<UserIncludeGroupView>().FromSql(pageSb.ToString()).ToList();
                totalCount = queryResult.Count();
            }
            return projectEntryList;
        }

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistUserByLoginName(string userName)
        {
            var resultCount = _db.Set<AdminUser>().Count(a => a.UserName == userName);
            return resultCount == 0;
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
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            List<AdminUser> adminUserList;
            if (isdesc)
            {
                var dbQuery = _db.Set<AdminUser>().Where(queryWhere).OrderByDescending(orderBy);
                totalCount = dbQuery.Count();
                adminUserList = dbQuery.Skip(skipIndex).Take(pageSize).ToList(); ;
            }
            else
            {
                var dbQuery = _db.Set<AdminUser>().Where(queryWhere).OrderBy(orderBy);
                totalCount = dbQuery.Count();
                adminUserList = dbQuery.Skip(skipIndex).Take(pageSize).ToList(); ;
            }
            return adminUserList;
        }
    }
}
