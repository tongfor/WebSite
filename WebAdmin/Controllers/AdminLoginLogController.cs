using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class AdminLoginLogController : BaseController
    {
        private IAdminLoginLogService _adminLoginLogService;

        public AdminLoginLogController(IAdminLoginLogService adminLoginLogService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
            _adminLoginLogService = adminLoginLogService;
        }

        // GET: AdminLoginLog
        public async Task<IActionResult> Index(BaseRequest request)
        {
            try
            {
                ViewBag.PageIndex = request.PageIndex;
                IEnumerable<AdminLoginLog> adminLoginLogList = await _adminLoginLogService.GetAdminLoginLogForAdminListAsync(request);
                await CreateLeftMenuAsync();

                return View(adminLoginLogList as PagedList<AdminLoginLog>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "登录日志列表错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, "登录功能异常" + ex.Message);                
            }
        }

        public async Task<ActionResult> LoginLogToXls(BaseRequest request)
        {
            try
            {
                ViewBag.page = request.PageIndex;//第几页
                ViewBag.Index = (request.PageIndex - 1) * request.PageSize + 1;//第几条
                ViewBag.toIndex = request.PageIndex * request.PageSize;//到多少条
                IEnumerable<AdminLoginLog> adminLoginLogList = _adminLoginLogService.GetAdminLoginLogForAdminList(request);

                var filename = $"登录日志" + DateTime.Now.ToString("yyyyMMdd") + ".xls";
                System.Net.Mime.ContentDisposition cd = new System.Net.Mime.ContentDisposition
                {
                    FileName = filename,
                    Inline = false  // false = prompt the user for downloading;  true = browser to try to show the file inline
                };
                Response.Headers.Add("Content-Disposition", cd.ToString());
                Response.ContentType = "application/vnd.ms-excel";

                return View(adminLoginLogList as PagedList<AdminLoginLog>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "导出登录日志错误" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, "登录功能异常" + ex.Message);
            }
        }


        #region 删除操作
        //// GET: Article/Delete/5
        //public ActionResult Delete(int id)
        //{
        //    try
        //    {
        //        _adminLoginLogService.DelBy(f => f.Id == id);
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "删除登录日志错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //}

        ////批量删除
        //[HttpPost]
        //public ActionResult Delete(List<int> ids)
        //{
        //    try
        //    {
        //        if (ids != null)
        //        {
        //            _adminLoginLogService.DelBy(f => ids.Contains(f.Id));
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            return RedirectToAction("Index");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "批量删除登录日志错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //    }
        //} 
        #endregion
    }
}