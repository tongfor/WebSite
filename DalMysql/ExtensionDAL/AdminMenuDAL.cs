/** 
* AdminMenuDAL.cs
*
* 功 能： AdminMenu数据层扩展实现
* 类 名： AdminMenuDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/21 17:05:34   李庸    初版
* V0.02  2016/9/27 17:05:34   李庸    增加根据用户组获取关联按钮的菜单数据（直接执行查询语句）
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DALMySql
{
    /// <summary>
    /// AdminMenu数据层扩展实现,可得到用户可访问菜单
    /// </summary>
    public partial class AdminMenuDAL
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
            StringBuilder sb = new StringBuilder();
            //sb.Append("select TT.RowNo as Id,TT.menuname,TT.menuid,TT.icon,");
            //sb.Append("TT.userid, TT.UserName, TT.menuparentid, TT.menusort,");
            //sb.Append("TT.linkaddress from (");
            //EF CORE需要AdminUserMenuViewId列
            sb.Append("SELECT TT.AdminUserMenuViewId as Id,TT.* from (");
            sb.Append("select distinct(m.Name) menuname,m.Id menuid,m.Icon icon,u.Id userid,u.UserName UserName,");
            sb.Append("m.ParentId menuparentid,m.Sort menusort,m.LinkAddress linkaddress,");
            sb.Append("@RowNo :=@RowNo + 1 AS AdminUserMenuViewId from AdminUser u join AdminUserAdminRole");
            sb.Append(" ur on u.Id=ur.UserId  join AdminRoleAdminMenuButton rmb on ur.RoleId=rmb.RoleId  join AdminMenu m");
            sb.Append(" on rmb.MenuId=m.Id,(SELECT @RowNo := 0) t ");
            sb.Append($" where u.id={userId} order by m.ParentId,m.Sort");
            sb.Append(") as TT");
            var queryResult = _db.Set<AdminUserMenuView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                menuList = queryResult.ToList();
            }
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
            StringBuilder sb = new StringBuilder();
            //sb.Append("select TT.RowNo as Id,TT.menuname,TT.menuid,TT.icon,");
            //sb.Append("TT.userid, TT.UserName, TT.menuparentid, TT.menusort,");
            //sb.Append("TT.linkaddress from (");
            //EF CORE需要AdminUserMenuViewId列
            sb.Append("SELECT TT.AdminUserMenuViewId as Id,TT.* from (");
            sb.Append("select distinct(m.Name) menuname,m.Id menuid,m.Icon icon,u.Id userid,u.UserName UserName,");
            sb.Append("m.ParentId menuparentid,m.Sort menusort,m.LinkAddress linkaddress,");
            sb.Append("@RowNo :=@RowNo + 1 AS AdminUserMenuViewId from AdminUser u join AdminUserAdminRole");
            sb.Append(" ur on u.Id=ur.UserId  join AdminRoleAdminMenuButton rmb on ur.RoleId=rmb.RoleId  join AdminMenu m");
            sb.Append(" on rmb.MenuId=m.Id,(SELECT @RowNo := 0) t ");
            sb.Append($" where u.id={userId} order by m.ParentId,m.Sort");
            sb.Append(") as TT");
            var queryResult = _db.Set<AdminUserMenuView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                menuList = await queryResult.ToListAsync();
            }
            return menuList;
        }

        #endregion

        #region 根据用户组获取关联按钮的菜单数据（直接执行查询语句）

        /// <summary>
        /// 根据用户组获取关联按钮的菜单数据（直接执行查询语句）
        /// </summary>
        /// <param name="ParentMenuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public List<AdminMenuRoleButtonView> GetMenuListIncludeRoleAndButton(int ParentMenuId, int roleId)
        {
            List<AdminMenuRoleButtonView> menuList = new List<AdminMenuRoleButtonView>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select am.*,am.Id as AdminMenuId,rmb.RoleId as RoleId,");
            sb.Append("rmb.ButtonId as ButtonId from AdminMenu as am ");
            sb.Append(" left join AdminRoleAdminMenuButton as rmb ");
            sb.Append($" on am.Id=rmb.menuid and rmb.RoleId={roleId}");
            sb.Append(" and (rmb.ButtonId=1 or rmb.ButtonId=0) ");
            sb.Append($" where am.ParentId={ParentMenuId} ");
            sb.Append(" order by parentid,sort");
            var queryResult =
                _db.Set<AdminMenuRoleButtonView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                menuList = queryResult.ToList();
            }
            return menuList;
        }

        /// <summary>
        /// 异步根据用户组获取关联按钮的菜单数据（直接执行查询语句）
        /// </summary>
        /// <param name="ParentMenuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<List<AdminMenuRoleButtonView>> GetMenuListIncludeRoleAndButtonAsync(int ParentMenuId, int roleId)
        {
            List<AdminMenuRoleButtonView> menuList = new List<AdminMenuRoleButtonView>();
            StringBuilder sb = new StringBuilder();
            sb.Append("select am.*,am.Id as AdminMenuId,rmb.RoleId as RoleId,");
            sb.Append("rmb.ButtonId as ButtonId from AdminMenu as am ");
            sb.Append(" left join AdminRoleAdminMenuButton as rmb ");
            sb.Append($" on am.Id=rmb.menuid and rmb.RoleId={roleId}");
            sb.Append(" and (rmb.ButtonId=1 or rmb.ButtonId=0) ");
            sb.Append($" where am.ParentId={ParentMenuId} ");
            sb.Append(" order by parentid,sort");
            var queryResult =
                _db.Set<AdminMenuRoleButtonView>().FromSql(sb.ToString());
            if (queryResult != null)
            {
                menuList = await queryResult.ToListAsync();
            }
            return menuList;
        }

        #endregion
    }
}
