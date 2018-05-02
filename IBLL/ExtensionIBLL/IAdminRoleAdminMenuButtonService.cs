/** 
* IAdminRoleAdminMenuButtonService.cs
*
* 功 能： IAdminRoleAdminMenuButton逻辑层扩展
* 类 名： IAdminRoleAdminMenuButtonService
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
using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IBLL
{
    /// <summary>
    /// AdminRoleAdminMenuButton逻辑层扩展
    /// </summary>
    public partial interface IAdminRoleAdminMenuButtonService
    {
        #region 更新用户菜单权限

        /// <summary>
        /// 更新用户菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="selectedMenuIds">选择的菜单ID清单</param>
        void ModifyUserRoleMenuButton(int roleId, string selectedMenuIds);

        /// <summary>
        /// 异步更新用户菜单权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="selectedMenuIds">选择的菜单ID清单</param>
        void ModifyUserRoleMenuButtonAsync(int roleId, string selectedMenuIds);

        #endregion
    }

}
