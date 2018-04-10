/** 
* IAdminMenuDAL.cs
*
* 功 能： AdminMenu接口
* 类 名： IAdminMenuDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/21 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace IDAL
{
    public partial interface IAdminMenuDAL
    {
        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 根据用户ID获取当前用户能够查询的菜单List（直接执行查询语句）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        List<AdminUserMenuView> GetAdminUserMenu(int userId);

        /// <summary>
        /// 异步根据用户ID获取当前用户能够查询的菜单List（直接执行查询语句）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        Task<List<AdminUserMenuView>> GetAdminUserMenuAsync(int userId);

        #endregion

        #region 根据用户组获取关联按钮的菜单数据（直接执行查询语句）

        /// <summary>
        /// 根据用户组获取关联按钮的菜单数据（直接执行查询语句）
        /// </summary>
        /// <param name="ParentMenuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        List<AdminMenuRoleButtonView> GetMenuListIncludeRoleAndButton(int ParentMenuId, int roleId);

        /// <summary>
        /// 异步根据用户组获取关联按钮的菜单数据（直接执行查询语句）
        /// </summary>
        /// <param name="ParentMenuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        Task<List<AdminMenuRoleButtonView>> GetMenuListIncludeRoleAndButtonAsync(int ParentMenuId, int roleId);

        #endregion

        #region 删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        void DelIncludeRelatedData(int id);

        /// <summary>
        ///  异步删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
       Task DelIncludeRelatedDataAsync(int id);

        #endregion 删除数据(包括关联数据)

        #region 批量删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        void DelIncludeRelatedData(List<int> ids);

        /// <summary>
        ///  异步批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        Task DelIncludeRelatedDataAsync(List<int> ids);

        #endregion
    }
}