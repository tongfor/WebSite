using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IAdminRoleDAL
    {
        /// <summary>
        /// 根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        AdminRole GetRoleByUserId(int UserId);

        /// <summary>
        /// 异步根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        Task<AdminRole> GetRoleByUserIdAsync(int UserId);
    }
}
