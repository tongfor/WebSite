using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;

namespace WebAdmin.Controllers
{
    public class AdminLoginLogController : BaseController
    {
        private IAdminLoginLogService _adminLoginLogService;

        public AdminLoginLogController(IAdminLoginLogService adminLoginLogService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
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
                OperateLog.OperateInfo = "登录日志列表错误:" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                OperateLog.OperateTime = DateTime.Now;
                MyOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        public ActionResult LoginLogToXls(BaseRequest request)
        {
            try
            {
                ViewBag.page = request.PageIndex;//第几页
                ViewBag.Index = (request.PageIndex - 1) * request.PageSize;//第几条
                ViewBag.toIndex = request.PageIndex * request.PageSize;//到多少条
                IEnumerable<AdminLoginLog> adminLoginLogList = _adminLoginLogService.GetAdminLoginLogForAdminList(request);

                return View(adminLoginLogList as PagedList<AdminLoginLog>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "导出登录日志错误:" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                OperateLog.OperateTime = DateTime.Now;
                MyOperateLogService.Add(OperateLog);
                return View("Error");
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
        //        return View("Error");
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
        //        return View("Error");
        //    }
        //} 
        #endregion
    }
}