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
using System.ComponentModel.DataAnnotations;

namespace Models
{
    /// <summary>
    /// 用于业务逻辑的AdminUse模型
    /// </summary>
    public class AdminUserView
    {
        public AdminUserView()
        {

        }

        public AdminUserView(AdminUser adminUser)
        {
            Id = adminUser.Id;
            UserName = adminUser.UserName;
            UserPwd = adminUser.UserPwd;
            Name = adminUser.Name;
            Mobile = adminUser.Mobile;
            Qq = adminUser.Qq;
            Email = adminUser.Email;
            Postcode = adminUser.Postcode;
            MemberLevel = adminUser.MemberLevel;
            IsFromThird = adminUser.IsFromThird;
            ThirdUrl = adminUser.ThirdUrl;
            ThirdToken = adminUser.ThirdToken;
            ThirdType = adminUser.ThirdType;
            IsAble = adminUser.IsAble;
            IsChangePwd = adminUser.IsChangePwd;
            AddTime = adminUser.AddTime;
            EditTime = adminUser.EditTime;
        }

        [Key]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required, StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string UserPwd { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Mobile { get; set; }
        public string Qq { get; set; }
        [EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }
        public string Postcode { get; set; }
        public int? MemberLevel { get; set; }
        public sbyte? IsFromThird { get; set; }
        public string ThirdUrl { get; set; }
        public string ThirdToken { get; set; }
        public string ThirdType { get; set; }
        public sbyte? IsAble { get; set; }
        public sbyte? IsChangePwd { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }

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

        public AdminUser ToOriginal(AdminUserView adminUserView)
        {
            return new AdminUser()
            {
                Id = adminUserView.Id,
                UserName = adminUserView.UserName,
                UserPwd = adminUserView.UserPwd,
                Name = adminUserView.Name,
                Mobile = adminUserView.Mobile,
                Qq = adminUserView.Qq,
                Email = adminUserView.Email,
                Postcode = adminUserView.Postcode,
                MemberLevel = adminUserView.MemberLevel,
                IsFromThird = adminUserView.IsFromThird,
                ThirdUrl = adminUserView.ThirdUrl,
                ThirdToken = adminUserView.ThirdToken,
                ThirdType = adminUserView.ThirdType,
                IsAble = adminUserView.IsAble,
                IsChangePwd = adminUserView.IsChangePwd,
                AddTime = adminUserView.AddTime,
                EditTime = adminUserView.EditTime
            };          
        }
    }

    /// <summary>
    /// 用于业务逻辑的包含用户分组信息的AdminUse模型
    /// </summary>
    public class UserIncludeGroupView
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Qq { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public int? MemberLevel { get; set; }
        public sbyte? IsFromThird { get; set; }
        public string ThirdUrl { get; set; }
        public string ThirdToken { get; set; }
        public string ThirdType { get; set; }
        public sbyte? IsAble { get; set; }
        public sbyte? IsChangePwd { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }

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
