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
using Microsoft.EntityFrameworkCore;
using Models;

namespace DALMySql
{
    /// <summary>
    /// AdminMenu数据层扩展实现,可得到用户可访问菜单
    /// </summary>
    public partial class AdminMenuDAL
    {
        //EF上下文
        private readonly CdyhcdDBContext _db;

        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 根据用户ID获取当前用户能够查询的菜单List（直接执行查询语句）
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<AdminUserMenuModel> GetAdminUserMenu(int userId)
        {
            List<AdminUserMenuModel> menuList = new List<AdminUserMenuModel>();
            StringBuilder querySb = new StringBuilder();
            querySb.Append("select distinct(m.Name) menuname,m.Id menuid,m.Icon icon,u.Id userid,u.UserName UserName,");
            querySb.Append("m.ParentId menuparentid,m.Sort menusort,m.LinkAddress linkaddress from AdminUser u join AdminUserAdminRole");
            querySb.Append(" ur on u.Id=ur.UserId  join AdminRoleAdminMenuButton rmb on ur.RoleId=rmb.RoleId  join AdminMenu m");
            querySb.Append(" on rmb.MenuId=m.Id where u.id=?userId order by m.ParentId,m.Sort");

            var userIdParameter = MySql.Data.EntityFrameworkCore.DataAnnotations. new MySqlParameter("?userId", userId);
            var queryResult = _db.Set<AdminUserMenuModel>().FromSql(querySb.ToString(), userIdParameter);
            if (queryResult != null)
            {
                menuList = queryResult.ToList();
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
            string strQuery = "select am.*,rmb.RoleId as RoleId,rmb.ButtonId as ButtonId from AdminMenu as am ";
            strQuery += " left join AdminRoleAdminMenuButton as rmb ";
            strQuery += " on am.Id=rmb.menuid and rmb.RoleId=" + roleId + " and (rmb.ButtonId=1 or rmb.ButtonId=0) ";
            strQuery += " where am.ParentId=" + ParentMenuId; ;
            strQuery += " order by parentid,sort";
            var queryResult =
                _db.Set<AdminMenuRoleButtonView>().FromSql(strQuery);
            if (queryResult != null)
            {
                menuList = queryResult.ToList();
            }
            return menuList;
        }

        #endregion
    }
}
