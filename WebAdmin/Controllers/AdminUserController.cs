using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class AdminUserController : BaseController
    {
        //用户BLL
        private readonly IAdminUserService _adminUserService;
        private readonly IAdminRoleService _adminRoleService;
        private readonly IAdminUserAdminRoleService _userRoleService;

        public AdminUserController(IAdminUserService adminUserService, IAdminRoleService adminRoleService, IAdminUserAdminRoleService adminUserAdminRoleService, IAdminOperateLogService MyOperateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options) : base(MyOperateLogService, adminBugService, adminMenuService, options)
        {
            this._adminUserService = adminUserService;
            this._adminRoleService = adminRoleService;
            this._userRoleService = adminUserAdminRoleService;
        }

        // GET: AdminUser
        public async Task<ActionResult> Index(AdminUserRequest request)
        {
            try
            {
                IEnumerable<AdminUser> adminUserList = await _adminUserService.GetAdminUserListAsync(request);
                await CreateLeftMenuAsync();

                return View(adminUserList as PagedList<AdminUser>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "查询用户清单异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        #region 新增
        // GET: AdminUser/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var model = new AdminUser();
                string tempPwd = MD5Helper.MD5Encrypt(SiteConfigSettings.DefaultAdminPwd).ToUpper();
                model.UserPwd = tempPwd;
                ViewBag.DefaultPwd = SiteConfigSettings.DefaultAdminPwd;
                ViewBag.IsEdit = false;
                return View("Edit", model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "新增用户页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: AdminUser/Create
        [HttpPost]
        public async Task<ActionResult> Create(AdminUser adminUser)
        {
            try
            {
                var model = new AdminUser();
                await TryUpdateModelAsync<AdminUser>(model);
                string userName = adminUser.UserName;
                bool canUse = await _adminUserService.UserCanRegisterAsync(userName);
                if (!canUse)
                {
                    ModelState.AddModelError("UserName", "已存在同名的用户，请重新填写!");
                    return View("Edit", model);
                }

                model.AddTime = DateTime.Now;
                model.IsAble = 1;
                _adminUserService.Add(model);
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "用户添加成功！");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;              
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "新增用户功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        #region 编辑
        // GET: AdminUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                OperateLog.OperateInfo = "编辑用户页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "用户未传id";
                await MyOperateLogService.AddAsync(OperateLog);
                ViewBag.IsEdit = false;
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                AdminUser adminUser = await _adminUserService.GetModelAsync(id.Value);
                if (adminUser != null)
                {
                    return View(adminUser.ToPOCO());
                }
                ViewBag.ErrMsg = "请传递正确参数!";
                OperateLog.OperateInfo = "编辑用户页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未找到此用户";
                await MyOperateLogService.AddAsync(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
               
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑用户页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: AdminUser/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(AdminUser adminUser)
        {
            try
            {
                if (adminUser == null || adminUser.Id <= 0)
                {
                    ViewBag.ErrMsg = "请传递参数!";
                    OperateLog.OperateInfo = "编辑用户功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "用户未传id";
                    await MyOperateLogService.AddAsync(OperateLog);
                }
                var model = await _adminUserService.GetModelAsync(adminUser.Id);
                string userName = adminUser.UserName;
                if (string.IsNullOrEmpty(userName))//登录名空验证
                {
                    ModelState.AddModelError("UserName", "请填写登录名!");
                    return View("Edit", model);
                }
                //重名验证
                var nameUsedModel = await _adminUserService.GetModelByAsync(f => f.UserName == userName);
                if (nameUsedModel != null && nameUsedModel.Id != adminUser.Id)
                {
                    ModelState.AddModelError("UserName", "已存在同名的用户，请重新填写!");
                    return View("Edit", model);
                }

                model.EditTime = DateTime.Now;
                await TryUpdateModelAsync<AdminUser>(model);

                _adminUserService.Modify(model);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "用户修改成功!");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
              
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑用户功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        #region 删除
        // GET: AdminUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || id <= 0)
                {
                    ViewBag.ErrMsg = "请传递参数!";
                    OperateLog.OperateInfo = "编辑用户功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "用户未传id";
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                await _userRoleService.DelByAsync(f => f.UserId == id.Value);
                await _adminUserService.DelByAsync(f => f.Id == id.Value);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "删除用户功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: AdminUser/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            try
            {
                await _adminUserService.DelByAsync(f => ids.Contains(f.Id));
                await _userRoleService.DelByAsync(f => f.UserId != null && ids.Contains(f.UserId.Value));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
               
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "批量删除用户功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        //设置用户可用/不可用
        public async Task<IActionResult> SetEnable(int? id, string isable)
        {
            if (id == null || id <= 0)
            {
                ViewBag.ErrMsg = "请传递参数！";
                OperateLog.OperateInfo = "激活用户功能异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未传递用户ID";
                await MyOperateLogService.AddAsync(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                //根据ID查看是否有角色   
                AdminUser user = await _adminUserService.GetModelAsync(id.Value);
                if (user == null)
                {
                    ViewBag.ErrMsg = "无此用户";
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                if ("0".Equals(isable))
                {
                    user.IsAble = 0;
                }
                else
                {
                    user.IsAble = 1;
                }
                await _adminUserService.ModifyAsync(user);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "激活用户功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        //设置角色
        public async Task<IActionResult> SetRole(int? id)
        {
            if (id == null || id <= 0)
            {
                ViewBag.ErrMsg = "请传递参数！";
                OperateLog.OperateInfo = "设置用户角色页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未传递用户ID";
                await MyOperateLogService.AddAsync(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                //根据ID查看是否有角色   
                AdminUser user = await _adminUserService.GetModelAsync(id.Value);
                //如果有角色就选中
                AdminUserAdminRole auRole = await _userRoleService.GetModelByAsync(a => a.UserId == id);
                if (auRole == null)
                {
                    auRole = new AdminUserAdminRole()
                    {
                        UserId = id
                    };
                }
                ViewBag.thisUserName = user.UserName;
                var adminRoles = await _adminRoleService.GetListByAsync(a => true);
                ViewBag.RoleId = adminRoles.Select(c => new SelectListItem()
                {
                    Text = c.RoleName,
                    Value = c.Id.ToString(),
                    Selected = c.Id == (auRole == null ? -1 : auRole.RoleId)
                }).ToList();

                return View(auRole);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "设置用户角色页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        //设置角色
        // POST: AdminUser/Edit/5
        [HttpPost]
        public async Task<IActionResult> SetRole(AdminUserAdminRole adminUserAdminRole)
        {
            try
            {
                //if (id == null || id <= 0)
                if (adminUserAdminRole == null || adminUserAdminRole.UserId == null || adminUserAdminRole.UserId.Value <= 0)
                {
                    ViewBag.ErrMsg = "请传递参数！";
                    OperateLog.OperateInfo = "设置用户角色功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递用户ID";
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                if (adminUserAdminRole.RoleId.Value > 0)//如果选了角色才操作
                {
                    //判断是否有数据
                    AdminUserAdminRole auRole = await _userRoleService.GetModelByAsync(f => f.UserId == adminUserAdminRole.UserId);
                    if (auRole == null)
                    {
                        //新增
                        auRole = new AdminUserAdminRole
                        {
                            UserId = adminUserAdminRole.UserId,
                            RoleId = adminUserAdminRole.RoleId,
                            AddTime = DateTime.Now,
                            EditTime = DateTime.Now
                        };
                        await _userRoleService.AddAsync(auRole);
                    }
                    else
                    {
                        //编辑
                        auRole.UserId = adminUserAdminRole.UserId;
                        auRole.RoleId = adminUserAdminRole.RoleId;
                        auRole.EditTime = DateTime.Now;
                        await _userRoleService.ModifyAsync(auRole);
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "设置用户角色成功！");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "设置用户角色功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        //重设密码
        //AdminUser/ResetUserPwd/5
        public async Task<IActionResult> ResetUserPwd(int? id)
        {
            if (id == null || id <= 0)
            {
                ViewBag.ErrMsg = "请传递参数！";
                OperateLog.OperateInfo = "设置用户角色页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未传递用户ID";
                await MyOperateLogService.AddAsync(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                await _adminUserService.ModifyPwdAsync(id.Value, MD5Helper.MD5Encrypt(SiteConfigSettings.DefaultAdminPwd));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "重设用户角色功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                MyAdminBugService.Add(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
    }
}