using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;

namespace DALMySql
{
    public partial class AdminRoleDAL
    {
        //EF上下文
        private readonly CdyhcdDBContext _db;

        /// <summary>
        /// 根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public AdminRole GetRoleByUserId(int UserId)
        {
            AdminRole AdminRole = new AdminRole();
            string queryStr = string.Empty;
            queryStr += string.Format(@"  SELECT * FROM  AdminRole where Id=(SELECT RoleId FROM  AdminUserAdminRole WHERE UserId={0})", UserId);
            var queryRole = _db.AdminRole.FromSql(queryStr);

            if (queryRole != null)
            {
                AdminRole = queryRole.ToList().FirstOrDefault();//计算总页数
            }

            return AdminRole;
        }
    }
}
