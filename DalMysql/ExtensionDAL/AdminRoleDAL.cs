using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALMySql
{
    public partial class AdminRoleDAL
    {
        /// <summary>
        /// 根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public AdminRole GetRoleByUserId(int UserId)
        {
            AdminRole AdminRole = new AdminRole();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM  AdminRole where Id=(SELECT RoleId FROM ");
            sb.Append($"AdminUserAdminRole WHERE UserId={UserId})");
            var queryRole = _db.AdminRole.FromSql(sb.ToString());

            if (queryRole != null)
            {
                AdminRole = queryRole.FirstOrDefault();
            }

            return AdminRole;
        }

        /// <summary>
        /// 异步根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public async Task<AdminRole> GetRoleByUserIdAsync(int UserId)
        {
            AdminRole AdminRole = null;
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT * FROM  AdminRole where Id=(SELECT RoleId FROM ");
            sb.Append($"AdminUserAdminRole WHERE UserId={UserId})");
            var queryRole = _db.AdminRole.FromSql(sb.ToString());

            if (queryRole != null)
            {
                AdminRole = await queryRole.FirstOrDefaultAsync();
            }

            return AdminRole;
        }

        #region 删除数据(包括关联数据)

        /// <summary>
        ///  删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public void DelIncludeRelatedData(int id)
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"delete from AdminRoleAdminMenuButton where RoleId = {id};");
                    sb.Append($"delete from AdminUserAdminRole where RoleId = {id};");
                    sb.Append($"delete from AdminRole where Id = {id};");
                    _db.Database.ExecuteSqlCommand(sb.ToString());
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///  异步删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public async void DelIncludeRelatedDataAsync(int id)
        {
            using (var tran = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"delete from AdminRoleAdminMenuButton where RoleId = {id};");
                    sb.Append($"delete from AdminUserAdminRole where RoleId = {id};");
                    sb.Append($"delete from AdminRole where Id = {id};");
                    await _db.Database.ExecuteSqlCommandAsync(sb.ToString());
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        #endregion

        #region 批量删除数据(包括关联数据)

        /// <summary>
        ///  批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public void DelIncludeRelatedData(List<int> ids)
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    string strIds = string.Join(',', ids);
                    sb.Append($"delete from AdminRoleAdminMenuButton where RoleId in ({strIds});");
                    sb.Append($"delete from AdminUserAdminRole where RoleId in ({strIds});");
                    sb.Append($"delete from AdminRole where Id in ({strIds});");                   
                    
                    _db.Database.ExecuteSqlCommand(sb.ToString());
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///  异步批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public async void DelIncludeRelatedDataAsync(List<int> ids)
        {            
            using (var tran = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    string strIds = string.Join(',', ids);
                    sb.Append($"delete from AdminRoleAdminMenuButton where RoleId in ({strIds});");
                    sb.Append($"delete from AdminUserAdminRole where RoleId in ({strIds});");
                    sb.Append($"delete from AdminRole where Id in ({strIds});");

                    await _db.Database.ExecuteSqlCommandAsync(sb.ToString());
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        #endregion
    }
}
