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
using System.Threading.Tasks;

namespace DALMySql
{
    public partial class AdminUserDAL
    {
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

        /// <summary>
        /// 异步根据用户名返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<AdminUser> GetModelByUserNameAsync(string username)
        {
            AdminUser adminUser = null;
            adminUser = await _db.Set<AdminUser>()
                .SingleOrDefaultAsync(f => f.UserName == username
                && f.IsAble != null && f.IsAble.Value == 1);
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

        /// <summary>
        /// 异步根据用户名和密码返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<AdminUser> GetModelByUserNameAndPwdAsync(string username, string password)
        {
            AdminUser adminUser = null;
            adminUser = await _db.Set<AdminUser>()
                    .SingleOrDefaultAsync(f => f.UserName == username
                    && f.UserPwd == password && f.IsAble != null && f.IsAble.Value == 1);
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
            sb.Append("Select au.*,au.id as AdminUserViewId,ul.RoleId as RoleId,");
            sb.Append("ar.RoleName as RoleName,ud.DepartmentId as DepartmentId,");
            sb.Append("ad.DepartmentName as DepartmentName ");
            sb.Append("from AdminUser as au ");
            sb.Append("left join AdminUserAdminRole as ul on au.id=ul.UserId ");
            sb.Append("left join AdminRole as ar on ul.RoleId=ar.Id ");
            sb.Append("LEFT JOIN AdminUserAdminDepartment as ud ");
            sb.Append("on au.Id = ud.UserId LEFT JOIN AdminDepartment as ad ");
            sb.Append("on ud.DepartmentId = ad.Id");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            var queryResult =
                _db.Set<AdminUserView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                userList = queryResult.ToList();
            }
            return userList;
        }

        /// <summary>
        /// 异步获取用户关联角色的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(article表用aco表示)</param>
        /// <returns></returns>
        public async Task<List<AdminUserView>> GetUserIncludeRoleAsync(string strWhere)
        {
            List<AdminUserView> userList = new List<AdminUserView>();
            StringBuilder sb = new StringBuilder();
            sb.Append("Select au.*,au.id as AdminUserViewId,ul.RoleId as RoleId,");
            sb.Append("ar.RoleName as RoleName,ud.DepartmentId as DepartmentId,");
            sb.Append("ad.DepartmentName as DepartmentName ");
            sb.Append("from AdminUser as au ");
            sb.Append("left join AdminUserAdminRole as ul on au.id=ul.UserId ");
            sb.Append("left join AdminRole as ar on ul.RoleId=ar.Id ");
            sb.Append("LEFT JOIN AdminUserAdminDepartment as ud ");
            sb.Append("on au.Id = ud.UserId LEFT JOIN AdminDepartment as ad ");
            sb.Append("on ud.DepartmentId = ad.Id");
            if (!string.IsNullOrEmpty(strWhere))
            {
                sb.Append($"where 1=1 and {strWhere}");
            }
            var queryResult =
                _db.Set<AdminUserView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                userList = await queryResult.ToListAsync();
            }
            return userList;
        }

        #endregion

        /// <summary>
        /// 判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册返回True</returns>
        public bool UserCanRegister(string userName)
        {
            var resultCount = _db.Set<AdminUser>().Count(a => a.UserName == userName);
            return resultCount == 0;
        }

        /// <summary>
        /// 异步判断用户是否还可以注册
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>能注册返回True</returns>
        public async Task<bool> UserCanRegisterAsync(string userName)
        {
            var resultCount = await _db.Set<AdminUser>().CountAsync(a => a.UserName == userName);
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
            var result = new PageData<AdminUser>();
            if (pageIndex < 1)
            {
                pageIndex = 1;
            }
            int skipIndex = (pageIndex - 1) * pageSize;
            if (isdesc)
            {
                var dbQuery = _db.Set<AdminUser>().Where(queryWhere).OrderByDescending(orderBy);
                result.DataList = await dbQuery.Skip(skipIndex).Take(pageSize).ToListAsync();
                result.TotalCount= await dbQuery.CountAsync();
            }
            else
            {
                var dbQuery = _db.Set<AdminUser>().Where(queryWhere).OrderBy(orderBy);
                result.DataList =await dbQuery.Skip(skipIndex).Take(pageSize).ToListAsync(); 
                result.TotalCount = await dbQuery.CountAsync();
            }
            return result;
        }
    }
}
