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
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class RoleController : BaseController
    {
        private IAdminRoleService _adminRoleService;
        private IAdminUserService _adminUserService;
        private IAdminRoleAdminMenuButtonService _adminRoleAdminMenuButtonService;

        public RoleController(IAdminRoleService adminRoleService, IAdminUserService adminUserService, IAdminRoleAdminMenuButtonService adminRoleAdminMenuButtonService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
            _adminRoleService = adminRoleService;
            _adminUserService = adminUserService;
            _adminRoleAdminMenuButtonService = adminRoleAdminMenuButtonService;
        }

        // GET: Role
        public async Task<IActionResult> Index(RoleRequest request)
        {
            try
            {
                if (request == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "角色查看页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色信息";
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                IEnumerable<AdminRoleView> roleList = await _adminRoleService.GetAdminRoleListAsync(request);
                await CreateLeftMenuAsync();

                return View(roleList as PagedList<AdminRoleView>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "角色查看页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Role/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                var model = new AdminRole();
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
                    BugInfo = "角色编辑页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Role/Create
        [HttpPost]
        public async Task<IActionResult> Create(AdminRole adminRole)
        {
            try
            {
                var model = new AdminRole();
                await TryUpdateModelAsync(model);

                await _adminRoleService.AddAsync(model);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "添加成功！");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "角色编辑保存异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Role/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null || id.Value <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "角色编辑页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色ID";
                    ModelState.AddModelError(string.Empty, "请传递正确参数!");
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View();
                }

                AdminRole model = await _adminRoleService.GetModelAsync(id.Value);

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
                    BugInfo = "角色编辑页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                ModelState.AddModelError(string.Empty, "内部错误，请联系管理员!");
                return View();
            }
        }

        // POST: Role/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(AdminRole adminRole)
        {
            try
            {
                if (adminRole == null || adminRole.Id <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "角色编辑保存异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色ID";
                    ModelState.AddModelError(string.Empty, "请传递正确参数!");
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View();
                }

                var model = await _adminRoleService.GetModelByAsync(f => f.Id == adminRole.Id);
                await TryUpdateModelAsync<AdminRole>(model);

                _adminRoleService.Modify(model);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "修改成功!");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "角色编辑保存异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                ModelState.AddModelError(string.Empty, "内部错误，请联系管理员!");
                return View();
            }
        }

        // GET: Role/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null || id.Value <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "角色删除功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色ID";
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                _adminRoleService.DelIncludeRelatedDataAsync(id.Value);
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
                    BugInfo = "角色删除功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        //批量删除
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            try
            {
                _adminRoleService.DelIncludeRelatedDataAsync(ids);
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
                    BugInfo = "角色批量删除功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 角色所属用户
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        // GET: RoleUser
        public async Task<IActionResult> RoleUserIndex(UserRequest request)
        {
            try
            {
                if (request == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "角色所属用户页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色信息";
                    ModelState.AddModelError(string.Empty, "请传递正确参数!");
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View();
                }

                IEnumerable<AdminUserView> userList = await _adminUserService.GetUserByRequestAsync(request);
                var pageList = userList as PagedList<AdminUserView>;

                return View(pageList);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "角色所属用户页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);

                ModelState.AddModelError(string.Empty, "内部错误，请联系管理员!");
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        /// <summary>
        /// 编辑角色菜单
        /// </summary>
        /// <param name="id">角色ID</param>
        /// <returns></returns>
        // GET: RoleMenu
        public async Task<IActionResult> EditRoleMenu(int? id)
        {
            try
            {
                if (id == null || id.Value <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "编辑角色菜单页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色ID";
                    ModelState.AddModelError(string.Empty, "请传递正确参数!");
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View();
                }

                ViewBag.RoleId = id.Value;
                var model = new RoleMenuViewModel()
                {
                    RoleId = id.Value
                };
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
                    BugInfo = "编辑角色菜单页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Role/EditRoleMenu/5
        [HttpPost]
        public async Task<IActionResult> EditRoleMenu(RoleMenuViewModel roleMenuViewModel)
        {
            try
            {
                if (roleMenuViewModel == null || roleMenuViewModel.RoleId <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "编辑角色菜单保存异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递角色ID";
                    ModelState.AddModelError(string.Empty, "请传递正确参数!");
                    await MyOperateLogService.AddAsync(OperateLog);
                    return View();
                }

                string newCheckedMenu = roleMenuViewModel.CheckedMenuIds;

                if (!string.IsNullOrEmpty(newCheckedMenu))
                {
                    _adminRoleAdminMenuButtonService.ModifyUserRoleMenuButton(roleMenuViewModel.RoleId, newCheckedMenu);
                }

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "角色菜单设置成功！");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑角色菜单保存异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
    }
}