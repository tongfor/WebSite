using Common;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAdmin.Controllers
{
    public class SpiderController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;
        private readonly IGatherService _gatherService;
        private static List<GatherResult> _gatherResultList;

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
                GatherResult gatherResult = await _gatherService.GatherWebsiteAsync(siteKey, pageStartNo, pageEndNo, classId, User?.Identity?.Name);
                int gatherCount = gatherResult == null || gatherResult.GatheredArticleList == null
                    ? 0 : gatherResult.GatheredArticleList.Count;
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResult != null && gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                _logger.LogInnerError(ex, $"采集{siteKey}列表");
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集所有已配网站数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <param name="classId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GatherAllWebsites(int? pageStartNo, int? pageEndNo, int classId)
        {
            //try
            //{
            //    _gatherResultList = new List<GatherResult>();
            //    //默认采集前3页数据
            //    pageStartNo = pageStartNo == null || pageStartNo == 0 ? 1 : pageStartNo;
            //    pageEndNo = pageEndNo == null || pageEndNo == 0 ? 3 : pageEndNo;
            //    if (GatherSettings!=null &&GatherSettings.GatherWebsiteList!=null)
            //    {
            //        GatherResult gatherResult = new GatherResult();
            //        foreach (var site in GatherSettings.GatherWebsiteList)
            //        {
            //            if (string.IsNullOrEmpty(site.Key) || string.IsNullOrEmpty(site.Name) || string.IsNullOrEmpty(site.SiteUrl) || string.IsNullOrEmpty(site.UrlTemp))
            //            {
            //                continue;
            //            }
            //            try
            //            {
            //                gatherResult = await _gatherService.GatherWebsiteAsync(site.Key, pageStartNo, pageEndNo, classId, User?.Identity?.Name);
            //                _gatherResultList.Add(gatherResult);
            //            }
            //            catch(Exception ex)
            //            {
            //                _logger.LogInnerError(ex, $"一键采集中采集{site.Name}数据时报错！");
            //                gatherResult.ErrorStatus = GatherErrorCode.Other;
            //                gatherResult.GatherTime = DateTime.Now;
            //                gatherResult.GatherMessage = ex.Message;
            //                continue;
            //            }
            //        }
            //    }

            //    return PackagingAjaxMsg(AjaxStatus.IsSuccess, _gatherResultList.Count > 0 ? $"采集成功！采集了{_gatherResultList.Count}个网站的数据！" : "暂无新增数据!", _gatherResultList);
            //}
            //catch (Exception ex)
            //{
            //    ViewBag.ErrMsg = ex.Message;

            //    _logger.LogInnerError(ex, $"一键采集报错！");
            //    return PackagingAjaxMsg(AjaxStatus.Err, $"一键采集报错！");
            //}
            var gatherResultList = await _gatherService.GatherAllWebsites(pageStartNo, pageEndNo, classId, User?.Identity?.Name);
            return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherResultList.Count > 0 ? $"采集成功！采集了{gatherResultList.Count}个网站的数据！" : "暂无新增数据!", gatherResultList);
        }

        /// <summary>
        /// 得到一键采集的采集结果，也用于保持心跳线连接
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetGatherAllResult()
        {
            return PackagingAjaxMsg(AjaxStatus.IsSuccess, $"获取成功", _gatherService.GetGatherAllResult());
        }
    }
}