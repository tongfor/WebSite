using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebAdmin.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "手机号/电子邮件")]
        public string EmailOrPhone { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "代码")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "记住此浏览器?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "手机号/电子邮件")]
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$|^0?1[3|4|5|8][0-9]\d{8}$", ErrorMessage = "{0}格式错误!")]
        public string EmailOrPhone { get; set; }
    }

    /// <summary>
    /// 登录视图模型
    /// </summary>
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "验证码")]
        public string VerifyCode { get; set; }

        [Display(Name = "记住我?")]
        public bool RememberMe { get; set; }
    }

    /// <summary>
    /// 注册视图模型
    /// </summary>
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "手机号/电子邮件")]
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$|^0?1[3|4|5|8][0-9]\d{8}$|^[A-Za-z0-9]+$", ErrorMessage = "{0}格式错误!")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }
    }

    /// <summary>
    /// 密码修改视图模型
    /// </summary>
    public class ModifyPwdViewModel
    {
        [Required]
        [Display(Name = "用户名")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "原密码")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} 必须至少包含 {2} 个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("NewPassword", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    /// <summary>
    /// 忘记密码视图模型
    /// </summary>
    public class ForgotPasswordViewModel
    {
        [Required]
        [Display(Name = "手机号/电子邮件")]
        [RegularExpression(@"^([a-z0-9_\.-]+)@([\da-z\.-]+)\.([a-z\.]{2,6})$|^0?1[3|4|5|8][0-9]\d{8}$|^[A-Za-z0-9]+$", ErrorMessage = "{0}格式错误!")]
        public string UserName { get; set; }
    }
}
