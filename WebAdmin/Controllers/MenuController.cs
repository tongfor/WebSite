using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using Common;
using Common.AspNetCore.Extensions;
using Common.Atrributes;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class MenuController : BaseController
    {
        public MenuController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {

        }

        // GET: Menu
        public async Task<ActionResult> Index(BaseRequest request)
        {
            try
            {
                IEnumerable<AdminMenu> menuList = await MyAdminMenuService.GetAdminMenuListAsync(request);

                await CreateLeftMenuAsync();
                return View(menuList as PagedList<AdminMenu>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "菜单查看页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Menu/Create
        public async Task<ActionResult> Create()
        {
            try
            {
                var model = new AdminMenu();
                var parentIdList = await MyAdminMenuService.GetAllMenuOrderListAsync();
                this.ViewBag.ParentId = new SelectList(parentIdList, "Id", "Name");
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
                    BugInfo = "菜单创建页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Menu/Create
        [HttpPost]
        public async Task<ActionResult> Create(AdminMenu adminMenu)
        {
            try
            {
                var model = new AdminMenu();
                await TryUpdateModelAsync<AdminMenu>(model);

                return await MyAdminMenuService.AddAsync(model) > 0
                    ? PackagingAjaxMsg(AjaxStatus.IsSuccess, "添加成功!")
                    : PackagingAjaxMsg(AjaxStatus.Err, "添加失败!");
                //return this.RefreshParent();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "菜单创建保存异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Menu/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "菜单编辑页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递菜单ID";
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                AdminMenu model = await MyAdminMenuService.GetModelAsync(id.Value);
                this.ViewBag.ParentId = model.ParentId;

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
                    BugInfo = "菜单编辑页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Menu/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(AdminMenu adminMenu)
        {
            try
            {
                if (adminMenu == null || adminMenu.Id <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "菜单编辑保存异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递菜单ID";
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                var model = await MyAdminMenuService.GetModelAsync(adminMenu.Id);
                await TryUpdateModelAsync<AdminMenu>(model);

                return await MyAdminMenuService.ModifyAsync(model) > 0
                    ? PackagingAjaxMsg(AjaxStatus.IsSuccess, "修改成功!")
                    : PackagingAjaxMsg(AjaxStatus.Err, "修改失败!");


                //return this.RefreshParent();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "菜单编辑保存异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Menu/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "菜单删除功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递菜单ID";
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                await MyAdminMenuService.DelIncludeRelatedDataAsync(id.Value);
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
                    BugInfo = "菜单删除功能异常" + ex.Message,
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
                await MyAdminMenuService.DelIncludeRelatedDataAsync(ids);
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
                    BugInfo = "菜单批量删除功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        ///// <summary>
        ///// 得到用户可访问菜单
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[AjaxRequest]
        //public ActionResult GetAdminMenuTree()
        //{
        //    try
        //    {
        //        AjaxMsgModel amm = Model_UserInfo.GetAdminUserMenuTreeAjax(oc.CurrentAdminUser.Id);
        //        return PackagingAjaxMsg(amm.Status, amm.Msg, amm.Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        return PackagingAjaxMsg(AjaxStatus.Err, ex.Message);
        //    }
        //}

        /// <summary>
        /// 查询所有角色菜单树,并以JSON字符串返回
        /// </summary>
        /// <param name="menuId"></param>
        /// <param name="roleId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAdminUserMenuTreeJson(int menuId, int roleId)
        {
            try
            {
                string data = await MyAdminMenuService.GetMenuTreeJsonByRoleIdAsync(menuId, roleId);
                return Content(data);
            }
            catch (Exception ex)
            {
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "查询所有角色菜单树异常" + ex.Message,
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
        /// 查询所有菜单树,并以JSON字符串返回
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetAllMenuTreeJson(int parentId)
        {
            try
            {
                var jsonTree = await MyAdminMenuService.GetAllMenuTreeJsonAsync(parentId);
                string data = jsonTree.Insert(1, "{\"id\":\"0\",\"text\":\"根节点\"},");
                return Content(data);
            }
            catch (Exception ex)
            {
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "查询所有菜单树,并以JSON字符串返回异常" + ex.Message,
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