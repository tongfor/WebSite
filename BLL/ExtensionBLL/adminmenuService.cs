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
using System.Linq;
using System.Text;
using IDAL;
using Models;
using Common;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// AdminMenu业务,可得到用户可访问菜单
    /// </summary>
    public partial class AdminMenuService
    {
        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 根据用户ID获取当前用户能够查询的菜单List（直接执行查询语句）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<AdminUserMenuView> GetAdminUserMenu(int userId)
        {
            List<AdminUserMenuView> menuList = new List<AdminUserMenuView>();
            menuList = adminMenuDAL.GetAdminUserMenu(userId);
            return menuList;
        }

        /// <summary>
        /// 异步根据用户ID获取当前用户能够查询的菜单List（直接执行查询语句）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public async Task<List<AdminUserMenuView>> GetAdminUserMenuAsync(int userId)
        {
            List<AdminUserMenuView> menuList = new List<AdminUserMenuView>();
            menuList = await adminMenuDAL.GetAdminUserMenuAsync(userId);
            return menuList;
        }

        #endregion

        #region 递归获取当前用户能够查询的菜单List

        /// <summary>
        /// 递归获取当前用户能够查询的菜单List
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parentId">父菜单ID</param>
        /// <returns></returns>
        public List<AdminUserMenuView> GetAdminUserMenuTree(List<AdminUserMenuView> menuList, int parentId)
        {
            List<AdminUserMenuView> menuTreeList = new List<AdminUserMenuView>();
            List<AdminUserMenuView> exceptedMenuList = menuList.ToList();
            foreach (var menu in menuList.Where(f => f.MenuParentId == parentId))
            {
                exceptedMenuList.Remove(menu);
                menu.ChildMenus = menu.MenuParentId == 0
                    ? exceptedMenuList.Where(p => p.MenuParentId == menu.MenuId).OrderBy(o => o.MenuSort).ToList()
                    : GetAdminUserMenuTree(exceptedMenuList, menu.MenuId);
                menuTreeList.Add(menu);
            }
            return menuTreeList;
        }

        /// <summary>
        /// 异步递归获取当前用户能够查询的菜单List
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="parentId">父菜单ID</param>
        /// <returns></returns>
        public async Task<List<AdminUserMenuView>> GetAdminUserMenuTreeAsync(List<AdminUserMenuView> menuList, int parentId)
        {
            List<AdminUserMenuView> menuTreeList = new List<AdminUserMenuView>();
            List<AdminUserMenuView> exceptedMenuList = menuList.ToList();
            await Task.Run(() =>
            {
                foreach (var menu in menuList.Where(f => f.MenuParentId == parentId))
                {
                    exceptedMenuList.Remove(menu);
                    menu.ChildMenus = menu.MenuParentId == 0
                        ? exceptedMenuList.Where(p => p.MenuParentId == menu.MenuId).OrderBy(o => o.MenuSort).ToList()
                        : GetAdminUserMenuTree(exceptedMenuList, menu.MenuId);
                    menuTreeList.Add(menu);
                }
            });
            return menuTreeList;
        }

        #endregion

        #region 查询所有角色菜单类别树并返回JSON

        /// <summary>
        /// 查询角色菜单树并返回JSON
        /// </summary>
        public string GetMenuTreeJsonByRoleId(int menuId, int roleId)
        {
            List<AdminMenuRoleButtonView> modelList = adminMenuDAL.GetMenuListIncludeRoleAndButton(menuId, roleId);
            StringBuilder jsonResult = new StringBuilder();

            jsonResult.Append("[");
            foreach (AdminMenuRoleButtonView amrb in modelList)
            {
                jsonResult.Append("{\"id\":\"" + amrb.Id + "\",\"text\":\"" + amrb.Name + "\"");
                //AdminRoleAdminMenuButton aab =
                //    AdminRoleAdminMenuButtonDAL.GetListBy(
                //        f => f.RoleId == roleId && f.MenuId == amrb.Id && f.ButtonId == 1).FirstOrDefault();
                //if (aab != null)
                if (amrb.RoleId != null)
                {
                    jsonResult.Append(",\"checked\":\"" + true + "\"");
                }
                List<AdminMenuRoleButtonView> cModelList = adminMenuDAL.GetMenuListIncludeRoleAndButton(amrb.Id, roleId);
                if (cModelList.Count > 0) //根节点下有子节点
                {
                    jsonResult.Append(",");
                    jsonResult.Append("\"children\":" + GetMenuTreeJsonByRoleId(amrb.Id, roleId));
                    jsonResult.Append("},");
                }
                else //根节点下没有子节点
                {
                    jsonResult.Append("},");
                }
            }
            string tmpstr = jsonResult.ToString().TrimEnd(',');//去掉最后多余的逗号
            jsonResult.Clear().Append(tmpstr);
            jsonResult.Append("]");

            return jsonResult.ToString();
        }

        /// <summary>
        /// 异步查询角色菜单树并返回JSON
        /// </summary>
        public async Task<string> GetMenuTreeJsonByRoleIdAsync(int menuId, int roleId)
        {
            List<AdminMenuRoleButtonView> modelList = adminMenuDAL.GetMenuListIncludeRoleAndButton(menuId, roleId);
            StringBuilder jsonResult = new StringBuilder();

            await Task.Run(() =>
            {
                jsonResult.Append("[");
                foreach (AdminMenuRoleButtonView amrb in modelList)
                {
                    jsonResult.Append("{\"id\":\"" + amrb.Id + "\",\"text\":\"" + amrb.Name + "\"");
                    //AdminRoleAdminMenuButton aab =
                    //    AdminRoleAdminMenuButtonDAL.GetListBy(
                    //        f => f.RoleId == roleId && f.MenuId == amrb.Id && f.ButtonId == 1).FirstOrDefault();
                    //if (aab != null)
                    if (amrb.RoleId != null)
                    {
                        jsonResult.Append(",\"checked\":\"" + true + "\"");
                    }
                    List<AdminMenuRoleButtonView> cModelList = adminMenuDAL.GetMenuListIncludeRoleAndButton(amrb.Id, roleId);
                    if (cModelList.Count > 0) //根节点下有子节点
                    {
                        jsonResult.Append(",");
                        jsonResult.Append("\"children\":" + GetMenuTreeJsonByRoleId(amrb.Id, roleId));
                        jsonResult.Append("},");
                    }
                    else //根节点下没有子节点
                    {
                        jsonResult.Append("},");
                    }
                }
                string tmpstr = jsonResult.ToString().TrimEnd(',');//去掉最后多余的逗号
                jsonResult.Clear().Append(tmpstr);
                jsonResult.Append("]");
            });

            return jsonResult.ToString();
        }

        #endregion

        #region 根据父菜单ID查询所有角色菜单树并返回JSON

        /// <summary>
        /// 根据父菜单ID查询角色菜单树并返回JSON
        /// </summary>
        public string GetAllMenuTreeJson(int parentId)
        {
            List<AdminMenu> menuList = GetAllMenuOrderList(parentId);
            StringBuilder jsonResult = new StringBuilder();

            jsonResult.Append("[");
            foreach (AdminMenu am in menuList)
            {
                jsonResult.Append("{\"id\":\"" + am.Id + "\",\"text\":\"" + am.Name + "\"");
                List<AdminMenu> cModelList = GetAllMenuOrderList(am.Id);
                if (cModelList.Count > 0) //根节点下有子节点
                {
                    jsonResult.Append(",");
                    jsonResult.Append("\"children\":" + GetAllMenuTreeJson(am.Id));
                    jsonResult.Append("},");
                }
                else //根节点下没有子节点
                {
                    jsonResult.Append("},");
                }
            }
            string tmpstr = jsonResult.ToString().TrimEnd(',');//去掉最后多余的逗号
            jsonResult.Clear().Append(tmpstr);
            jsonResult.Append("]");

            return jsonResult.ToString();
        }

        /// <summary>
        /// 异步根据父菜单ID查询角色菜单树并返回JSON
        /// </summary>
        public async Task<string> GetAllMenuTreeJsonAsync(int parentId)
        {
            List<AdminMenu> menuList = GetAllMenuOrderList(parentId);
            StringBuilder jsonResult = new StringBuilder();

            await Task.Run(() =>
            {
                jsonResult.Append("[");
                foreach (AdminMenu am in menuList)
                {
                    jsonResult.Append("{\"id\":\"" + am.Id + "\",\"text\":\"" + am.Name + "\"");
                    List<AdminMenu> cModelList = GetAllMenuOrderList(am.Id);
                    if (cModelList.Count > 0) //根节点下有子节点
                    {
                        jsonResult.Append(",");
                        jsonResult.Append("\"children\":" + GetAllMenuTreeJson(am.Id));
                        jsonResult.Append("},");
                    }
                    else //根节点下没有子节点
                    {
                        jsonResult.Append("},");
                    }
                }
                string tmpstr = jsonResult.ToString().TrimEnd(',');//去掉最后多余的逗号
                jsonResult.Clear().Append(tmpstr);
                jsonResult.Append("]");
            });

            return jsonResult.ToString();
        }

        #endregion

        #region 根据请求条件获取IPageList格式数据

        /// <summary>
        /// 根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminMenu> GetAdminMenuList(BaseRequest request = null)
        {
            request = request ?? new BaseRequest();
            List<AdminMenu> menuList = new List<AdminMenu>();
            int totalCount = 0;

            var roleList = !string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title) ? adminMenuDAL.GetPageListBy(request.PageIndex, request.PageSize, f => f.Name.Contains(request.Title), o => o.Id, out totalCount, true)
              :
              adminMenuDAL.GetPageListBy(request.PageIndex, request.PageSize, f => true, o => o.Id, out totalCount, true);          
            
            return roleList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdminMenu>> GetAdminMenuListAsync(BaseRequest request = null)
        {
            request = request ?? new BaseRequest();
            List<AdminMenu> menuList = new List<AdminMenu>();
            PageData<AdminMenu> pageData = new PageData<AdminMenu>();

            pageData = !string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title)
                ? await adminMenuDAL.GetPageDataAsync(request.PageIndex, request.PageSize, f => f.Name.Contains(request.Title), o => o.Id, true)
                : await adminMenuDAL.GetPageDataAsync(request.PageIndex, request.PageSize, f => true, o => o.Id, true);

            return pageData.DataList.ToPagedList(request.PageIndex, request.PageSize, pageData.TotalCount);
        }

        #endregion

        #region 得到所有菜单List并排序

        /// <summary>
        /// 得到所有菜单List并排序
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public List<AdminMenu> GetAllMenuOrderList(int parentId = -1)
        {
            List<AdminMenu> menuList = new List<AdminMenu>();
            menuList = parentId < 0 ?
                GetListBy(f => true).OrderBy(o => o.ParentId).ThenBy(o => o.Sort).ToList()
                : GetListBy(f => f.ParentId == parentId).OrderBy(o => o.ParentId).ThenBy(o => o.Sort).ToList();
            return menuList;
        }

        /// <summary>
        /// 异步得到所有菜单List并排序
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<AdminMenu>> GetAllMenuOrderListAsync(int parentId = -1)
        {
            List<AdminMenu> adminMenus;
            if (parentId < 0)
            {
                adminMenus = await GetListByAsync(f => true);
            }
            else
            {
                adminMenus  = await GetListByAsync(f => f.ParentId == parentId);
            }            
            return adminMenus.OrderBy(o => o.ParentId).ThenBy(o => o.Sort).ToList();
        }

        #endregion

        #region 删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public void DelIncludeRelatedData(int id)
        {
            adminMenuDAL.DelIncludeRelatedData(id);
        }

        /// <summary>
        ///  异步删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public async Task DelIncludeRelatedDataAsync(int id)
        {
            await adminMenuDAL.DelIncludeRelatedDataAsync(id);
        }

        #endregion 删除数据(包括关联数据)

        #region 批量删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public void DelIncludeRelatedData(List<int> ids)
        {
            adminMenuDAL.DelIncludeRelatedData(ids);
        }

        /// <summary>
        ///  异步批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public async Task DelIncludeRelatedDataAsync(List<int> ids)
        {
            await adminMenuDAL.DelIncludeRelatedDataAsync(ids);
        }

        #endregion
    }
}
