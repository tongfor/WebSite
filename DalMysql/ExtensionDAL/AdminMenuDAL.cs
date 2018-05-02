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
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
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
        #region 获取用户关联用户菜单的数据（直接执行查询语句）

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

        #region 删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public void DelIncludeRelatedData(int id)
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"delete from AdminRoleAdminMenuButton where MenuId = {id};");
                    sb.Append($"delete from AdminMenuAdminButton where MenuId = {id};");
                    sb.Append($"delete from AdminMenu where id = {id};");

                    _db.Database.ExecuteSqlCommand(sb.ToString());

                    //new AdminRoleAdminMenuButtonDAL(_db).DelBy(f => f.MenuId == id);
                    //new AdminMenuAdminButtonDAL(_db).DelBy(f => f.MenuId == id);
                    //DelBy(f => f.Id == id);
                    //_db.SaveChanges();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///  异步删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public async Task DelIncludeRelatedDataAsync(int id)
        {
            using (var tran = await _db.Database.BeginTransactionAsync())
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append($"delete from AdminRoleAdminMenuButton where MenuId = {id};");
                    sb.Append($"delete from AdminMenuAdminButton where MenuId = {id};");
                    sb.Append($"delete from AdminMenu where id = {id};");

                    await _db.Database.ExecuteSqlCommandAsync(sb.ToString());

                    //await new AdminRoleAdminMenuButtonDAL(_db).DelByAsync(f => f.MenuId == id);
                    //await new AdminMenuAdminButtonDAL(_db).DelByAsync(f => f.MenuId == id);
                    //await DelByAsync(f => f.Id == id);
                    //_db.SaveChangesAsync();

                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("异步删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        #endregion 删除数据(包括关联数据)

        #region 批量删除数据(使用事务，包括关联数据)

        /// <summary>
        ///  批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public void DelIncludeRelatedData(List<int> ids)
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                try
                {
                    string strIds = string.Join(',', ids);
                    StringBuilder sb = new StringBuilder();

                    sb.Append($"delete from AdminRoleAdminMenuButton where MenuId in ({strIds});");
                    sb.Append($"delete from AdminMenuAdminButton where MenuId in ({strIds});");
                    sb.Append($"delete from AdminMenu where id in ({strIds});");

                    _db.Database.ExecuteSqlCommand(sb.ToString());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("批量删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        /// <summary>
        ///  异步批量删除数据(使用事务，包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public async Task DelIncludeRelatedDataAsync(List<int> ids)
        {
            using (var tran = _db.Database.BeginTransaction())
            {
                try
                {
                    string strIds = string.Join(',', ids);
                    StringBuilder sb = new StringBuilder();

                    sb.Append($"delete from AdminRoleAdminMenuButton where MenuId in ({strIds});");
                    sb.Append($"delete from AdminMenuAdminButton where MenuId in ({strIds});");
                    sb.Append($"delete from AdminMenu where id in ({strIds});");

                    await _db.Database.ExecuteSqlCommandAsync(sb.ToString());
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw new Exception("异步批量删除数据异常，在" + ex.StackTrace.ToString() + "。错误信息：" + ex.Message);
                }
            }
        }

        #endregion
    }
}
