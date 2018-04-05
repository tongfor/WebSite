﻿/** 
* AdminUserView.cs
*
* 功 能： 用于业务逻辑的AdminUse模型
* 类 名： AdminUserView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/26 17:05:34   李庸    初版
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

namespace Models
{
    /// <summary>
    /// 用于业务逻辑的AdminUse模型
    /// </summary>
    public class AdminUserView : AdminUser
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public int? DepartmentId { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }
    }

    /// <summary>
    /// 用于业务逻辑的包含用户分组信息的AdminUse模型
    /// </summary>
    public class UserIncludeGroupView : AdminUser
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        public int? GroupId { get; set; }
    }
}