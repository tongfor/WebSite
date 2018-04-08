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

        #region 删除数据(包括关联数据)

        /// <summary>
        ///  删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        void DelIncludeRelatedData(int id);

        /// <summary>
        ///  异步删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        void DelIncludeRelatedDataAsync(int id);

        #endregion

        #region 批量删除数据(包括关联数据)

        /// <summary>
        ///  批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        void DelIncludeRelatedData(List<int> ids);

        /// <summary>
        ///  异步批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        void DelIncludeRelatedDataAsync(List<int> ids);

        #endregion
    }
}
