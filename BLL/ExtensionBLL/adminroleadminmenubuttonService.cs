

using IDAL;
using Models;
using RepositoryPattern;
/** 
* AdminRoleAdminMenuButtonService.cs
*
* 功 能： AdminRoleAdminMenuButton逻辑层扩展
* 类 名： AdminRoleAdminMenuButtonService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/28 17:05:34   李庸    初版
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

namespace BLL
{
    /// <summary>
    /// AdminRoleAdminMenuButton逻辑层扩展
    /// </summary>
    public partial class AdminRoleAdminMenuButtonService
    {
        protected IAdminMenuDAL MyIAdminMenuDAL;

        #region 构造函数

        public AdminRoleAdminMenuButtonService(CdyhcdDBContext db, IAdminRoleAdminMenuButtonDAL adminRoleAdminMenuButtonDAl, IAdminMenuDAL adminMenuDAL) : base(adminRoleAdminMenuButtonDAl)
        {
            MyDBContext = db;
            MyIAdminRoleAdminMenuButtonDAL = adminRoleAdminMenuButtonDAl;
            MyIAdminMenuDAL = adminMenuDAL;
        }

        #endregion

        #region 更新用户菜单权限

        /// <summary>
        /// 更新用户菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="selectedMenuIds">选择的菜单ID清单</param>
        public void ModifyUserRoleMenuButton(int roleId, string selectedMenuIds)
        {
            List<AdminRoleAdminMenuButton> oldSelectedList = MyIAdminRoleAdminMenuButtonDAL.GetListBy(f => f.RoleId == roleId);
            List<int> newSelectedMenuIdList = selectedMenuIds.Split(',').Select(s => int.Parse(s)).ToList();
            AdminRoleAdminMenuButton newModel = new AdminRoleAdminMenuButton();
            newModel.RoleId = roleId;
            foreach (int sMenuId in newSelectedMenuIdList)
            {
                AdminRoleAdminMenuButton oldSelected = oldSelectedList.FirstOrDefault(f => f.RoleId == roleId && f.MenuId == sMenuId);
                
                if (oldSelected == null)
                {
                    newModel.MenuId = sMenuId;
                    var menuModel = MyIAdminMenuDAL.GetModel(sMenuId);
                    newModel.ButtonId = menuModel != null && menuModel.ParentId == 0 ? 0 : 1;
                    int addint = MyIAdminRoleAdminMenuButtonDAL.Add(newModel);
                }
                else
                {
                    continue;
                }
            }

            //取出应删除的
            List<AdminRoleAdminMenuButton> needDelSelectedList = oldSelectedList.Where(f => !newSelectedMenuIdList.Contains(f.MenuId.Value)).ToList();
            foreach(var delModel in needDelSelectedList)
            {
                int delint = MyIAdminRoleAdminMenuButtonDAL.Del(delModel);
            }
        }

        /// <summary>
        /// 异步更新用户菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="selectedMenuIds">选择的菜单ID清单</param>
        public async void ModifyUserRoleMenuButtonAsync(int roleId, string selectedMenuIds)
        {
            List<AdminRoleAdminMenuButton> oldSelectedList = await MyIAdminRoleAdminMenuButtonDAL.GetListByAsync(f => f.RoleId == roleId);
            List<int> newSelectedMenuIdList = selectedMenuIds.Split(',').Select(s => int.Parse(s)).ToList();
            AdminRoleAdminMenuButton newModel = new AdminRoleAdminMenuButton();
            newModel.RoleId = roleId;
            foreach (int sMenuId in newSelectedMenuIdList)
            {
                AdminRoleAdminMenuButton oldSelected = oldSelectedList.FirstOrDefault(f => f.RoleId == roleId && f.MenuId == sMenuId);

                if (oldSelected == null)
                {
                    newModel.MenuId = sMenuId;
                    var menuModel = MyIAdminMenuDAL.GetModel(sMenuId);
                    newModel.ButtonId = menuModel != null && menuModel.ParentId == 0 ? 0 : 1;
                    int addint = await MyIAdminRoleAdminMenuButtonDAL.AddAsync(newModel);
                }
                else
                {
                    continue;
                }
            }

            //取出应删除的
            List<AdminRoleAdminMenuButton> needDelSelectedList = oldSelectedList.Where(f => !newSelectedMenuIdList.Contains(f.MenuId.Value)).ToList();
            foreach (var delModel in needDelSelectedList)
            {
                int delint = await MyIAdminRoleAdminMenuButtonDAL.DelAsync(delModel);
            }
        }

        #endregion
    }

}
