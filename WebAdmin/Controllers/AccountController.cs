using Common;
using Common.AspNetCore.Extensions;
using Common.Atrributes;
using Common.Config;
using Common.ValidateCode;
using IBLL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Setting.Mvc.Authorize;
using System;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    [AllowAnonymous]
    public class AccountController : BaseController
    {
        /// <summary>
        /// 默认记住账号天数
        /// </summary>
        private const int DefaultRemberDays = 7;
        /// <summary>
        /// 同一IP允许登录失败最大限制
        /// </summary>
        private const int MaxLoginErrorCount = 5;
        /// <summary>
        /// 同一IP登录失败超过最大限制后，多长时间能重新登录
        /// </summary>
        private const int LoginErrorTryMinutes = 30;

        private readonly IAdminUserService _adminUserService;
        private readonly IAdminLoginLogService _loginLogService;
        private readonly IAdminRoleService _adminRoleService;        

        public AccountController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminUserService adminUserService, IAdminRoleService adminRoleService,
            IAdminLoginLogService adminLoginLog, IAdminMenuService adminMenuService, ILogger<AccountController> logger, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            _adminUserService = adminUserService;
            _adminRoleService = adminRoleService;
            _loginLogService = adminLoginLog;
            _logger = logger;
        }

        [TempData]
        public string ErrorMessage { get; set; }       

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(DefaultAuthorizeAttribute.DefaultAuthenticationScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                if (!IsValidVerifyCode(model.VerifyCode))
                {
                    ModelState.AddModelError("error", "验证码错误");
                    return View();
                }
                DateTime? lastLoginErrorDatetime = null;
                string userIp = HttpContext.GetUserIp();
                var loginCheck = await _loginLogService.CheckLoginErrorCountAsync(MaxLoginErrorCount, LoginErrorTryMinutes, userIp);
                if (loginCheck.Item1)
                {
                    //$"尝试登录次数超过最大限制，请{LoginErrorTryMinutes}后重试！"
                    ModelState.AddModelError("error", string.Format("尝试登录次数超过最大限制，请{0}后重试！", LoginErrorTryMinutes));
                    lastLoginErrorDatetime = loginCheck.Item2;
                }
                AjaxMsgModel amm = new AjaxMsgModel();

                amm = await LoginIn(model.UserName, model.Password, model.VerifyCode, model.RememberMe, DefaultRemberDays);
                if (amm != null && amm.Status == AjaxStatus.IsSuccess)
                {
                    _logger.LogInformation($"用户{model.UserName}于{DateTime.Now}在IP:{userIp}上登录.");
                    return Redirect(!string.IsNullOrEmpty(returnUrl) ? returnUrl : amm.ReturnUrl);
                }
                if (amm.Msg == null && amm.Msg.Length <= 0)
                {
                    ModelState.AddModelError("error", "账号密码错误，请重新登录！");
                }
                else
                {
                    ModelState.AddModelError("error", amm.Msg);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "登录功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString())
                };
                MyAdminBugService.Add(Bug);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// 修改密码页面
        /// </summary>
        public IActionResult ModifyPwd()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="collection"></param>
        [HttpPost]
        public async Task<IActionResult> ModifyPwd(ModifyPwdViewModel viewModel)
        {
            var currentUser = HttpContext.User;

            try
            {
                var modifyModel = await _adminUserService.GetUserByNameAndPasswordAsync(currentUser.Identity.Name, viewModel.OldPassword);
                if (modifyModel == null)
                {
                    this.ModelState.AddModelError(viewModel.OldPassword, "原密码不正确！");
                    return View(viewModel);
                }
                modifyModel.UserPwd = viewModel.NewPassword.MD5Encrypt();
                OperateLog = new AdminOperateLog
                {
                    UserName = currentUser.Identity.Name,
                    UserIp = HttpContext.GetUserIp(),
                    OperateInfo = string.Format($"用户{0}修改密码", currentUser.Identity.Name),
                    IsSuccess = 1,
                    OperateTime = DateTime.Now
                };
                MyOperateLogService.Add(OperateLog);
                await _adminUserService.ModifyAsync(modifyModel, "UserPwd");
            }
            catch (BusinessException ex)
            {
                this.ModelState.AddModelError(ex.Name, ex.Message);
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "修改密码错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString())
                };
                MyAdminBugService.Add(Bug);
                return View(viewModel);
            }

            return await Logout();
            //return this.RefreshParent("密码修改成功，将退出重新登录！");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            var currentUser = HttpContext.User;
            await HttpContext.SignOutAsync(DefaultAuthorizeAttribute.DefaultAuthenticationScheme);
            _logger.LogInformation("User logged out.");
            OperateLog = new AdminOperateLog
            {
                UserName = currentUser.Identity.Name,
                UserIp = HttpContext.GetUserIp(),
                OperateInfo = string.Format($"User {0} logged out", currentUser.Identity.Name),
                IsSuccess = 1,
                OperateTime = DateTime.Now
            };
            MyOperateLogService.Add(OperateLog);
            //return PageReturn("密码修改成功，将退出重新登录！", "/Account/Login");
            return RedirectToAction("Login", "Account");
        }

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        public virtual async Task<IActionResult> VerifyImage()
        {
            try
            {
                var validateCodeType = new ValidateCode_Style10();
                string code = "6666";
                byte[] bytes = null;
                await Task.Run(() =>
                {
                    bytes = validateCodeType.CreateImage(out code);
                    CurrentAdminVerificationCode = code;
                });
                return File(bytes, @"image/jpeg");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = "验证码生成异常";
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "验证码生成异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString())
                };
                MyAdminBugService.Add(Bug);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// 验证码确认
        /// </summary>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxRequest]
        public async Task<IActionResult> VerifyCodeValidate(string verifycode)
        {
            bool isValid = false;
            await Task<bool>.Run(() =>
            {
                isValid = string.Equals(CurrentAdminVerificationCode,
                    verifycode, StringComparison.CurrentCultureIgnoreCase);
            });

            return isValid
                 ? PackagingAjaxMsg(AjaxStatus.IsSuccess, "输入正确！")
                 : PackagingAjaxMsg(AjaxStatus.Err, "输入错误！");
        }

        #region noaction        

        #region 实现登录功能

        /// <summary>
        ///  实现登录功能
        /// </summary>
        /// <param name="strLoginName"></param>
        /// <param name="strLoginPwd"></param>
        /// <param name="verificationCode"></param>
        /// <param name="isSaveLogin">是否记住用户名</param>
        /// <param name="saveDays">记住用户名时间天数，小于0为不记住，0为永久记住</param>
        /// <returns></returns>
        [NonAction]
        public async Task<AjaxMsgModel> LoginIn(string strLoginName, string strLoginPwd, string verificationCode = "", bool isSaveLogin = false, int saveDays = -1)
        {
            AjaxMsgModel amm = new AjaxMsgModel();
            AdminLoginLog loginLogModel = new AdminLoginLog
            {
                UserName = strLoginName,
                UserIp = HttpContext.GetUserIp(),
                City = "未知",//Oc.Request.Params["city"] ?? "未知",
                LoginTime = DateTime.Now
            };
            if (!IsValidVerifyCode(verificationCode))
            {
                amm.Msg = "验证验错误！";
                amm.Status = AjaxStatus.LoginFail;
                return amm;
            }
            AdminUser adminUser = _adminUserService.GetUserByNameAndPassword(strLoginName, strLoginPwd);
            if (adminUser == null)
            {
                amm.Msg = "错误的用户名或密码！";
                amm.Status = AjaxStatus.LoginFail;
                loginLogModel.IsSuccess = 0;
                _loginLogService.Add(loginLogModel);
            }
            else
            {
                AdminRole tempRole = _adminRoleService.GetRoleByUserId(adminUser.Id);

                if (tempRole == null)
                {
                    amm.Msg = "用户不具备角色,请联系管理员设置角色之后重试！";
                    amm.Status = AjaxStatus.LoginFail;
                    loginLogModel.IsSuccess = 0;
                    _loginLogService.Add(loginLogModel);
                }
                else
                {
                    if (SiteConfigSettings.AllowAdminRoles.Contains(tempRole.Id.ToString()))
                    {
                        var claimIdentity = new ClaimsIdentity("Cookie");                        
                        claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, adminUser.Id.ToString()));
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, adminUser.UserName));
                        if (!string.IsNullOrEmpty(adminUser.Email))
                        {
                            claimIdentity.AddClaim(new Claim(ClaimTypes.Email, adminUser.Email));
                        }
                        if (!string.IsNullOrEmpty(adminUser.Mobile))
                        {
                            claimIdentity.AddClaim(new Claim(ClaimTypes.MobilePhone, adminUser.Mobile));
                        }
                        if (!string.IsNullOrEmpty(tempRole.RoleName))
                        {
                            claimIdentity.AddClaim(new Claim(ClaimTypes.Role, tempRole.RoleName));
                        }                        
                        var claimsPrincipal = new ClaimsPrincipal(claimIdentity);
                        var authenticationProp = new AuthenticationProperties()
                        {
                            IsPersistent = true
                        };
                        if (isSaveLogin && saveDays >= 0)
                        {
                            authenticationProp.ExpiresUtc = DateTime.UtcNow.AddDays(saveDays);
                        }                        
                        await HttpContext.SignInAsync(claimsPrincipal, new AuthenticationProperties());
                       

                        amm.Msg = "登录成功！";
                        amm.Status = AjaxStatus.IsSuccess;
                        amm.ReturnUrl = "~/Article/Index";
                        loginLogModel.IsSuccess = 1;
                        _loginLogService.Add(loginLogModel);
                    }
                    else
                    {
                        amm.Msg = "用户不具备访问权限！";
                        amm.Status = AjaxStatus.LoginFail;
                        loginLogModel.IsSuccess = 0;
                        _loginLogService.Add(loginLogModel);
                    }
                }
            }           

            return amm;
        }
       

        #endregion

        /// <summary>
        /// 查证验证码是否正确
        /// </summary>
        /// <param name="verifycode"></param>
        /// <returns></returns>
        public bool IsValidVerifyCode(string verifycode)
        {
            return !string.IsNullOrEmpty(verifycode) && verifycode.Equals(CurrentAdminVerificationCode, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion noaction       
    }
}