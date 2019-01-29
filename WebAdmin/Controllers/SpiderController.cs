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

        public SpiderController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IGatherService gatherService, ILogger<ArticleController> logger, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
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
        /// 采集网站数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherWebsite(string siteKey, int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                GatherResult gatherResult = new GatherResult();
                var is404 = await _gatherService.IsReturn404(siteKey);
                if (is404)
                {
                    gatherResult.GatherMessage = "采集页面不存在！";
                    return PackagingAjaxMsg(AjaxStatus.IsSuccess, $"采集失败！", gatherResult);
                }
                gatherResult = await _gatherService.GatherWebsiteAsync(siteKey, pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult.GatheredArticleList.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                _logger.LogError($"采集{siteKey}列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }       
    }
}