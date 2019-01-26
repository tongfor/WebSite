/** 
* AdminMenuView.cs
*
* 功 能： 用于业务逻辑的AdminMenu模型
* 类 名： AdminMenuView
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

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    /// <summary>
    /// 用于业务逻辑的AdminMenu模型
    /// </summary>
    public partial class AdminMenuView
    {

    }

    /// <summary>
    /// 用户当前可访问菜单模型
    /// </summary>
    [Serializable]
    public class AdminUserMenuView
    {
        /// <summary>
        /// 虚拟主键
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// 菜单名
        /// </summary>
        [Display(Name = "菜单名")]
        public string MenuName { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [Display(Name = "菜单ID")]
        public int MenuId { get; set; }

        /// <summary>
        /// 图标类名
        /// </summary>
        [Display(Name = "图标类名")]
        public string Icon { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        [Display(Name = "用户ID")]
        public int UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [Display(Name = "用户账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 菜单父ID
        /// </summary>
        [Display(Name = "菜单父ID")]
        public int MenuParentId { get; set; }

        /// <summary>
        /// 菜单排序
        /// </summary>
        [Display(Name = "菜单排序")]
        public int MenuSort { get; set; }

        /// <summary>
        /// 菜单链接
        /// </summary>
        [Display(Name = "菜单链接")]
        public string LinkAddress { get; set; }

        /// <summary>
        /// 子菜单
        /// </summary>
        [Display(Name = "子菜单")]
        [NotMapped]
        public List<AdminUserMenuView> ChildMenus { get; set; }
    }

    /// <summary>
    /// 用于业务逻辑的AdminRoleAdminMenuButton模型
    /// </summary>
    public class AdminMenuRoleButtonView
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentId { get; set; }
        public string Code { get; set; }
        public string LinkAddress { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }

        /// <summary>
        /// 角色ID
        /// </summary>
        public int? RoleId { get; set; }
        /// <summary>
        /// 按钮ID
        /// </summary>
        public int? ButtonId { get; set; }
    }
}
