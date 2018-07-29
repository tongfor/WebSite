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
    public class AdminOperateLogController : BaseController
    {
        public AdminOperateLogController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
        }

        // GET: AdminOperateLog
        //public async Task<IActionResult> Index(BaseRequest request)
        //{
        //    try
        //    {
        //        ViewBag.PageIndex = request.PageIndex;
        //        IEnumerable<AdminOperateLog> adminOperateLogList = MyOperateLogService.GetOperateLogForAdminList(request);

        //        return View(adminOperateLogList as PagedList<AdminOperateLog>);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "操作日志列表错误:" + ex.Message;
        //        OperateLog.IsSuccess = 0;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateTime = DateTime.Now;
        //        MyOperateLogService.Add(OperateLog);
        //        return View("Error");
        //    }
        //}
        //public async Task<IActionResult> OperateLogToXls(BaseRequest request)
        //{
        //    try
        //    {
        //        ViewBag.page = request.PageIndex;//第几页
        //        ViewBag.Index = (request.PageIndex - 1) * request.PageSize;//第几条
        //        ViewBag.toIndex = request.PageIndex * request.PageSize;//到多少条
        //        IEnumerable<AdminOperateLog> adminOperateLogList = MyOperateLogService.GetOperateLogForAdminList(request);

        //        return View(adminOperateLogList as PagedList<AdminOperateLog>);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "导出操作日志错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error");
        //    }
        //}


        //public async Task<IActionResult> ViewDescription(int? id)
        //{
        //    try
        //    {
        //        AdminOperateLog AdminOperateLogModel = MyOperateLogService.GetModelBy(a => a.Id == id);

        //        return View(AdminOperateLogModel);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrMsg = ex.Message;
        //        OperateLog.OperateInfo = "查看详细列表信息错误:" + ex.Message;
        //        OperateLog.IfSuccess = false;
        //        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        //        OperateLog.OperateDate = DateTime.Now;
        //        OperateLogService.Add(OperateLog);
        //        return View("Error");
        //    }
        //}

        //#region 删除操作

        ////// GET: Article/Delete/5
        ////public async Task<IActionResult> Delete(int id)
        ////{
        ////    try
        ////    {
        ////        MyOperateLogService.DelBy(f => f.Id == id);
        ////        return RedirectToAction("Index");
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ViewBag.ErrMsg = ex.Message;
        ////        OperateLog.OperateInfo = "删除操作日志错误:" + ex.Message;
        ////        OperateLog.IfSuccess = false;
        ////        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        ////        OperateLog.OperateDate = DateTime.Now;
        ////        OperateLogService.Add(OperateLog);
        ////        return View("Error");
        ////    }
        ////}

        //////批量删除
        ////[HttpPost]
        ////public async Task<IActionResult> Delete(List<int> ids)
        ////{
        ////    try
        ////    {
        ////        if (ids != null)
        ////        {
        ////            MyOperateLogService.DelBy(f => ids.Contains(f.Id));
        ////            return RedirectToAction("Index");
        ////        }
        ////        else
        ////        {
        ////            return RedirectToAction("Index");
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        ViewBag.ErrMsg = ex.Message;
        ////        OperateLog.OperateInfo = "批量删除操作日志错误:" + ex.Message;
        ////        OperateLog.IfSuccess = false;
        ////        OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
        ////        OperateLog.OperateDate = DateTime.Now;
        ////        OperateLogService.Add(OperateLog);
        ////        return View("Error");
        ////    }
        ////} 
        //#endregion
    }
}