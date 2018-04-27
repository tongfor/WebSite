/** 
* AdminMenuService.cs
*
* 功 能： AdminMenu业务层
* 类 名： AdminMenuService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/21 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Threading.Tasks;
using Models;

namespace IBLL
{
    /// <summary>
    /// AdminMenu业务,可得到用户可访问菜单
    /// </summary>
    public partial interface IAdminMenuService
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

        #region 递归获取当前用户能够查询的菜单List

        /// <summary>
        /// 递归获取当前用户能够查询的菜单List
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parentId">父菜单ID</param>
        /// <returns></returns>
        List<AdminUserMenuView> GetAdminUserMenuTree(List<AdminUserMenuView> menuList, int parentId);

        /// <summary>
        /// 异步递归获取当前用户能够查询的菜单List
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parentId">父菜单ID</param>
        /// <returns></returns>
        Task<List<AdminUserMenuView>> GetAdminUserMenuTreeAsync(List<AdminUserMenuView> menuList, int parentId);

        #endregion

        #region 查询所有角色菜单类别树并返回JSON

        /// <summary>
        /// 查询角色菜单树并返回JSON
        /// </summary>
        string GetMenuTreeJsonByRoleId(int menuId, int roleId);

        /// <summary>
        /// 异步查询角色菜单树并返回JSON
        /// </summary>
        Task<string> GetMenuTreeJsonByRoleIdAsync(int menuId, int roleId);

        #endregion

        #region 根据父菜单ID查询所有角色菜单树并返回JSON

        /// <summary>
        /// 根据父菜单ID查询角色菜单树并返回JSON
        /// </summary>
        string GetAllMenuTreeJson(int parentId);

        /// <summary>
        /// 异步根据父菜单ID查询角色菜单树并返回JSON
        /// </summary>
        Task<string> GetAllMenuTreeJsonAsync(int parentId);

        #endregion

        #region 根据请求条件获取IPageList格式数据

        /// <summary>
        /// 根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<AdminMenu> GetAdminMenuList(BaseRequest request = null);

        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<AdminMenu>> GetAdminMenuListAsync(BaseRequest request = null);

        #endregion

        #region 得到所有菜单List并排序

        /// <summary>
        /// 得到所有菜单List并排序
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        List<AdminMenu> GetAllMenuOrderList(int parentId = -1);

        /// <summary>
        /// 异步得到所有菜单List并排序
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        Task<List<AdminMenu>> GetAllMenuOrderListAsync(int parentId = -1);

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
