/** 
* IAdminRoleService.cs
*
* 功 能： IAdminRole逻辑层
* 类 名： IAdminRoleService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Models;

namespace IBLL
{
    public partial interface IAdminRoleService
    {
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

        #region 根据请求条件获取IPageList格式数据

        /// <summary>
        /// 根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<AdminRoleView> GetAdminRoleList(RoleRequest request = null);

        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<AdminRoleView>> GetAdminRoleListAsync(RoleRequest request = null);

        #endregion

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
