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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdgy" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    try
                    {
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
                                    .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("cmspro_documents ul li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                ).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetCdgyDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集市经委列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集市经委详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetCdgyDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdgy" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
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
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                    if (document.QuerySelector("div#top .up") != null)
                    {
                        document.QuerySelector("div#top")?.RemoveChild(document.QuerySelector("div#top .up"));
                    }
                    if (document.QuerySelector("div.bottom-icon01") != null)
                    {
                        document.QuerySelector("div#top")?.RemoveChild(document.QuerySelector("div.bottom-icon01"));
                    }
                    foreach (var item in document.QuerySelectorAll("div#top [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div#top [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    //ResetLink(document.QuerySelectorAll("div#top [src]").ToList(), gatherwebsite.SiteUrl, url);
                    //ResetLink(document.QuerySelectorAll("div#top [href]").ToList(), gatherwebsite.SiteUrl, url);
                    detailsInfo.Title = detailsInfo.Title ?? document.QuerySelector("div#top .up span")?.TextContent;
                    detailsInfo.Content = detailsInfo.Content ?? document.QuerySelector("div#top")?.InnerHtml;
                    var addHtmlurlArray = detailsInfo.AddHtmlurl?.Split('/');
                    string strAddTime = addHtmlurlArray[5] + "-" + addHtmlurlArray[6];
                    DateTime addTime = DateTime.Now;
                    DateTime.TryParse(strAddTime, out addTime);
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
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                        foreach (var item in document.QuerySelectorAll("div.main-show-txt [src]").ToList())
                        {
                            if (item.HasAttribute("src"))
                            {
                                string src = item.GetAttribute("src");
                                item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                            }
                        }
                        foreach (var item in document.QuerySelectorAll("div.main-show-txt [href]").ToList())
                        {
                            if (item.HasAttribute("href"))
                            {
                                string href = item.GetAttribute("href");
                                item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                            }
                        }
                        detailsInfo.Title = detailsInfo.Title ?? document.QuerySelector("h2.main-show-title")?.TextContent;
                        if (document.QuerySelector("div.bottom-icon01") != null)
                        {
                            document.QuerySelector("div.main-show-txt")?.RemoveChild(document.QuerySelector("div.bottom-icon01"));
                        }
                        detailsInfo.Content = detailsInfo.Content ?? document.QuerySelector("div.main-show-txt")?.InnerHtml;
                    }
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集市经委详情", ex);
                return null;
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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "jxt" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    if (1 == i)
                    {
                        website= website.Replace("_1.", ".");
                    }
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseDocumentAsync(htmlString);
                            if (SiteConfigSettings.NotificationClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul.list-li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                                && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                                && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                    .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.TrimStart('/')
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul.list-li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.TrimStart('/')
                                ).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //RecoreBug("采集省经委列表", ex);
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetJxtDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", gatherCount);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集省经委列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集省经委详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetJxtDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "jxt" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    var detailsInfo = document
                        .QuerySelectorAll("div.listlmtt")
                        .Select(t => new Article()
                        {
                            Title = t.QuerySelector("div#zoomtitl")?.TextContent,
                            Origin = gatherwebsite.Name + t.QuerySelector("div.source").TextContent.Replace("信息来源：", ""),
                            Content = t.QuerySelector("div#zoomcon")?.InnerHtml,
                            AddTime = t.QuerySelector("div.date")?.TextContent.Replace("发布日期:","").ToDateTime(),
                            Author = "",
                            UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                            AddHtmlurl = url,
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
                    foreach (var item in document.QuerySelectorAll("div#zoomcon [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div#zoomcon [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    detailsInfo.Content = document.QuerySelector("div#zoomcon")?.InnerHtml;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集省经委详情", ex);
                return null;
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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdst" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseDocumentAsync(htmlString);
                            if (SiteConfigSettings.NotificationClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul#showMoreNChildren li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                    .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.TrimStart('/')
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul#showMoreNChildren li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.TrimStart('/')
                                ).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetCdstDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集市科技局列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集市科技局详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetCdstDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdst" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
                    var detailsInfo = document
                        .QuerySelectorAll("html")
                        .Select(t => new Article()
                        {
                            Title = t.QuerySelector(".detail .title h3")?.TextContent,
                            Origin = gatherwebsite.Name + t.QuerySelector("#source")?.TextContent.Replace("来源：", ""),
                            EditTime = DateTime.Now,
                            Author = "",
                            UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                            AddHtmlurl = url,
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();                  
                    foreach (var item in document.QuerySelectorAll("div#content [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div#content [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    detailsInfo.Content = document.QuerySelector("div#content")?.InnerHtml;
                    var addHtmlurlArray = detailsInfo.AddHtmlurl?.Split('/');
                    string strAddTime = document.QuerySelector("div.detail .title p span")?.TextContent;
                    DateTime addTime = DateTime.Now;
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集市经委详情", ex);
                return null;
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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "kjt" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseDocumentAsync(htmlString);
                            if (SiteConfigSettings.NotificationClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("div.news_right h2").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                    .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("div.news_right h2").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t => gatherwebsite.SiteUrl + "/" + t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value.TrimStart('/')
                                ).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetKjtDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集省科技厅列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集省科技厅详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetKjtDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "kjt" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
                    var detailsInfo = document
                        .QuerySelectorAll("html")
                        .Select(t => new Article()
                        {
                            Title = t.QuerySelector(".newsTex h1")?.TextContent,
                            Origin = gatherwebsite.Name + t.QuerySelector(".msgbar")?.TextContent.Split("来源：")[1]?.Replace("取消收藏", "").Replace("收藏", "").Trim(),
                            EditTime = DateTime.Now,
                            Author = "",
                            UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                            AddHtmlurl = url,
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                    foreach (var item in document.QuerySelectorAll("div.newsCon [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div.newsCon [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    detailsInfo.Content = document.QuerySelector("div.newsCon")?.InnerHtml;
                    DateTime addTime = DateTime.Now;
                    string strAddTime = document.QuerySelector(".msgbar")?.TextContent.Split("来源：")[0]?.Replace("发布时间： ", "").Trim();
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集市经委详情", ex);
                return null;
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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdht" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    try {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseDocumentAsync(htmlString);
                            if (SiteConfigSettings.NotificationClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("div.news-list-list td")?.Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                                && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                                && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                    .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("div.news-list-list td").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().TextContent.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t => t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                ).ToList());
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetCdhtDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集高新区列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集高新区详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetCdhtDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdht" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
                    var detailsInfo = document
                        .QuerySelectorAll("html")
                        .Select(t => new Article()
                        {
                            Title = t.QuerySelector("div.page h1")?.TextContent,
                            Origin = gatherwebsite.Name + t.QuerySelectorAll("div.sx span")?[1]?.TextContent.Replace("来源:", "").Trim(),
                            EditTime = DateTime.Now,
                            Author = "",
                            UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                            AddHtmlurl = url,
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                    foreach (var item in document.QuerySelectorAll("div#d_content [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div#d_content [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    detailsInfo.Content = document.QuerySelector("div#d_content")?.InnerHtml;
                    DateTime addTime = DateTime.Now;
                    string strAddTime = document.QuerySelectorAll("div.sx span")?[0]?.TextContent.Trim();
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集市经委详情", ex);
                return null;
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
                int gatherCount = 0;
                List<string> gatherUrlList = new List<string>();
                List<Article> articles = new List<Article>();
                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdtf" == f.Key);
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherwebsite.UrlTemp, i);
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseDocumentAsync(htmlString);
                            if (SiteConfigSettings.NotificationClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul.list-ul li")?.Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[0].Trim())
                                && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.NotificationKeywords.Split("and")[1].Trim()))
                                    .Select(t =>
                                        t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                    ).ToList());
                            }
                            else if (SiteConfigSettings.PolicyClassId == classId)
                            {
                                gatherUrlList.AddRange(document.QuerySelectorAll("ul.list-ul li").Where(t => t.QuerySelectorAll("a").FirstOrDefault() != null
                            && t.QuerySelectorAll("a").FirstOrDefault().Attributes.FirstOrDefault(f => "title".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase)).Value.MyContains(SiteConfigSettings.PolicyKeywords.Trim()))
                                .Select(t =>
                                    t.QuerySelectorAll("a").FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value
                                ).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                foreach (string u in gatherUrlList)
                {
                    var details = await GetCdtfDetails(u, classId);
                    int addResult = await AddArticle(details);
                    if (addResult > 0)
                    {
                        articles.Add(details);
                        gatherCount++;
                    }
                }
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, gatherCount > 0 ? $"采集成功！采集数据{gatherCount}条！" : "暂无新增数据!", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                RecoreBug("采集天府新区列表", ex);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 采集天府新区详情页
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<Article> GetCdtfDetails(string url, int classId)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    HtmlParser htmlParser = new HtmlParser();
                    var gatherwebsite = SiteConfigSettings.GatherWebsiteList.FirstOrDefault(f => "cdtf" == f.Key);
                    var document = await htmlParser.ParseDocumentAsync(htmlString);
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);
                    var detailsInfo = document
                        .QuerySelectorAll("html")
                        .Select(t => new Article()
                        {
                            Title = t.QuerySelector("p.detail-title")?.TextContent,
                            Origin = gatherwebsite.Name + t.QuerySelector("table.detail-table td[width='30%']")?.TextContent.Replace("来源:", "").Trim(),
                            EditTime = DateTime.Now,
                            Author = "",
                            UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name,
                            AddHtmlurl = url,
                            ClassId = classId,
                            Keyword = gatherwebsite.Name
                        })
                        .FirstOrDefault();
                    foreach (var item in document.QuerySelectorAll("div#NewsContent [src]").ToList())
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                    }
                    foreach (var item in document.QuerySelectorAll("div#NewsContent [href]").ToList())
                    {
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherwebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    detailsInfo.Content = document.QuerySelector("div#NewsContent")?.InnerHtml;
                    DateTime addTime = DateTime.Now;
                    string strAddTime = document.QuerySelector("table.detail-table td[width='25%']")?.TextContent.Replace("发布时间：", "").Trim();
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                //RecoreBug("采集市经委详情", ex);
                return null;
            }
        }

        /// <summary>
        /// 记录BUG
        /// </summary>
        /// <param name="bugTitle"></param>
        /// <param name="ex"></param>
        [NonAction]
        private async void RecoreBug(string bugTitle, Exception ex)
        {
            Bug = new AdminBug
            {
                UserIp = HttpContext.GetUserIp(),
                IsShow = 1,
                IsSolve = 0,
                BugInfo = "文章采集异常" + ex.Message,
                BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                AddTime = DateTime.Now,
                EditTime = DateTime.Now
            };
            await MyAdminBugService.AddAsync(Bug);
        }

        /// <summary>
        /// 将采集的文章持久化
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        [NonAction]
        private async Task<int> AddArticle(Article details)
        {
            try
            {
                if (details == null || string.IsNullOrEmpty(details.Title) || string.IsNullOrEmpty(details.AddHtmlurl) || string.IsNullOrEmpty(details.Content))
                {
                    return 0;
                }

                var modelInDatabase = await _articleService.GetModelByAsync(f => details.Title.ToLower() == f.Title.ToLower() && details.AddHtmlurl.ToLower() == f.AddHtmlurl.ToLower());
                if (modelInDatabase != null && !string.IsNullOrEmpty(modelInDatabase.Title))
                {
                    return 0;
                }
                Thread.Sleep(50);
                return await _articleService.AddAsync(details);
            }
            catch(Exception ex)
            {
                //RecoreBug("采集详情页入库", ex);
                return 0;
            }
        }

        /// <summary>
        /// 补全采集网址中链接地址
        /// </summary>
        private async void ResetLink(IEnumerable<AngleSharp.Dom.IElement> elements, string siteUrl, string pageUrl)
        {
            await Task.Run(() => {
                string pagePath = pageUrl.Substring(0, pageUrl.LastIndexOf('/') + 1);
                foreach (var item in elements)
                {
                    try
                    {
                        if (item.HasAttribute("src"))
                        {
                            string src = item.GetAttribute("src");
                            item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? siteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                        }
                        if (item.HasAttribute("href"))
                        {
                            string href = item.GetAttribute("href");
                            item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? siteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                        }
                    }
                    catch(Exception ex)
                    {
                        //RecoreBug("补全采集网址中链接地址", ex);
                        continue;
                    }
                }
            });
        }
    }
}