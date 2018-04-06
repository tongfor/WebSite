using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Text;

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
    }
}
