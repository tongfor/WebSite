using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace WebAdmin.Controllers
{
    public class SpiderController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;

        public SpiderController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, ILogger<ArticleController> logger, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            _articleService = articleService;
            _articleClassService = articleClassService;
            _logger = logger;
        }


        // GET: Spider
        public async Task<IActionResult> Index()
        {
            await CreateLeftMenuAsync();
            return View();
        }

        /// <summary>
        /// 采集成都工业数据
        /// </summary>
        /// <param name="pageStartNo"></param>
        /// <param name="pageEndNo"></param>
        /// <returns></returns>
        public async Task<IActionResult> GatherCdgyWebsite(int? pageStartNo, int? pageEndNo, int classId)
        {
            try
            {
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdgy" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    using (HttpClient http = new HttpClient())
                    {
                        http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                        var htmlString = await http.GetStringAsync(website);
                        HtmlParser htmlParser = new HtmlParser();
                        var document = await htmlParser.ParseDocumentAsync(htmlString);
                        if (SiteConfigSettings.NotificationClassId == classId)
                        {
                            gatherUrlList.AddRange(document.QuerySelectorAll("cmspro_documents ul li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                .Select(t => t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value
                                ).ToList());
                        }
                        else if (SiteConfigSettings.PolicyClassId == classId)
                        {
                            gatherUrlList.AddRange(document.QuerySelectorAll("cmspro_documents ul li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                        && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                            .Select(t => t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value
                            ).ToList());
                        }
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetCdgyDetails(u);
                    if (details==null || string.IsNullOrEmpty(details.Title) || string.IsNullOrEmpty(details.AddHtmlurl) || string.IsNullOrEmpty(details.Content))
                    {
                        continue;
                    }

                    var modelInDatabase = await _articleService.GetModelByAsync(f => details.Title.ToLower() == f.Title.ToLower() && details.AddHtmlurl.ToLower() == f.AddHtmlurl.ToLower());
                    if (modelInDatabase != null && !string.IsNullOrEmpty(modelInDatabase.Title))
                    {
                        continue;
                    }
                    articles.Add(details);
                    Thread.Sleep(50);
                    int addResult = await _articleService.AddAsync(details);
                    if (addResult > 0)
                    {
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
           catch(Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "文章列表页面异常" + ex.Message,
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
        /// 采集市经委详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetCdgyDetails(string url)
        {
            using (HttpClient http = new HttpClient())
            {
                http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                var htmlString = await http.GetStringAsync(url);
                HtmlParser htmlParser = new HtmlParser();
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdgy" == f.Key);
                var document = await htmlParser.ParseDocumentAsync(htmlString);
                var detailsInfo = document
                    .QuerySelectorAll("html")
                    .Select(t => new Article()
                    {
                        Title = t.QuerySelectorAll("meta[name='ArticleTitle']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                        Origin = gatherwebsite.Name + t.QuerySelectorAll("meta[name='PubDate']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                        EditTime = DateTime.Now,
                        Author = "",
                        UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                        AddHtmlurl = url,
                        ClassId = 3,
                        Keyword= gatherwebsite.Name
                    })
                    .FirstOrDefault();
                document.QuerySelector("div#top")?.RemoveChild(document.QuerySelector("div#top .up"));
                detailsInfo.Title = detailsInfo.Title ?? document.QuerySelector("div#top .up span")?.TextContent;
                detailsInfo.Content = detailsInfo.Content ?? document.QuerySelector("div#top")?.InnerHtml;
                var addHtmlurlArray = detailsInfo.AddHtmlurl?.Split('/');
                string strAddTime = addHtmlurlArray[5] + "-" + addHtmlurlArray[6];
                DateTime.TryParse(strAddTime, out DateTime addTime);
                detailsInfo.AddTime = addTime;
                detailsInfo.AddTime = detailsInfo.AddTime ?? document.QuerySelectorAll("meta[name = 'others']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.Replace("页面生成时间 ", "").ToDateTime();
                if (detailsInfo == null || string.IsNullOrEmpty(detailsInfo.Title) || string.IsNullOrEmpty(detailsInfo.Content))
                {
                    document
                    .QuerySelectorAll("html")
                    .Select(t => new Article()
                    {
                        Title = t.QuerySelectorAll("meta[name='ArticleTitle']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                        Content = t.QuerySelector("div.main-show-txt")?.InnerHtml,
                        Origin = gatherwebsite.Name + t.QuerySelectorAll("meta[name='PubDate']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                        AddTime = t.QuerySelectorAll("meta[name = 'others']").FirstOrDefault()?.Attributes.FirstOrDefault(a => "content".Equals(a.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.Replace("页面生成时间 ", "").ToDateTime(),
                        EditTime = DateTime.Now,
                        Author = "",
                        UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                        AddHtmlurl = url,
                        ClassId = 3,
                        Keyword = gatherwebsite.Name
                    })
                    .FirstOrDefault();
                    detailsInfo.Title = detailsInfo.Title ?? document.QuerySelector("h2.main-show-title")?.TextContent;
                    detailsInfo.Content = detailsInfo.Content ?? document.QuerySelector("div.main-show-txt")?.InnerHtml;
                }
                return detailsInfo;
            }
        }       
    }
}