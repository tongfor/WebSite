using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Parser.Html;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;

namespace WebAdmin.Controllers
{
    public class SpiderController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;
        private readonly IGatherService _gatherService;        

        public SpiderController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IGatherService gatherService, ILogger<ArticleController> logger, IOptions<SiteConfig> options, IOptions<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
            _articleService = articleService;
            _articleClassService = articleClassService;
            _gatherService = gatherService;
            _logger = logger;
        }

        // GET: Spider
        public async Task<IActionResult> Index()
        {
            await CreateLeftMenuAsync();
            return View();
        }

        /// <summary>
        /// 采集市经委数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherCdgyWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("cdgy", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集市经委列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集省经委数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherJxtWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("jxt", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集省经委列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集市科技局数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherCdstWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("cdst", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集市科技局列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集省科技厅数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherKjtWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("kjt", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集省科技厅列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集高新区数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherCdhtWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("cdht", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);               
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集高新区列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集天府新区数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherCdtfWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                var gatherResult = await _gatherService.GatherWebsiteAsync("cdtf", pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleLIst.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecordBug("采集天府新区列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
    }
}