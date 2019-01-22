/** 
* GatherService.cs
*
* 功 能： 文章采集业务层
* 类 名： GatherService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/1/13 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using AngleSharp;
using AngleSharp.Parser.Html;
using Common;
using Common.Config;
using GatherSpecialHandler;
using IBLL;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// 文章采集业务层
    /// </summary>
    public class GatherService : IGatherService
    {
        /// <summary>
        /// 本站设置
        /// </summary>
        private readonly SiteConfig _siteConfig;

        private readonly GatherConfig _gatherConfig;

        private readonly IArticleService _articleService;

        private readonly ILogger<GatherService> _logger;

        public GatherService(IOptionsMonitor<SiteConfig> siteConfigOptions, IOptionsMonitor<GatherConfig> gatherConfigOptions, IArticleService articleService, ILogger<GatherService> logger)
        {
            _siteConfig = siteConfigOptions.CurrentValue;
            _gatherConfig = gatherConfigOptions.CurrentValue;
            _articleService = articleService;
            _logger = logger;
        }

        /// <summary>
        /// 采集站点文章
        /// </summary>
        /// <param name="siteKey">站点关键字</param>
        /// <param name="pageStartNo">文章列表启始页</param>
        /// <param name="pageEndNo">文章列表结束页</param>
        /// <param name="classId">文章类别</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        public async Task<GatherResult> GatherWebsiteAsync(string siteKey, int? pageStartNo, int? pageEndNo, int classId, string userName)
        {
            var gatherResult = new GatherResult
            {
                SiteKey = siteKey
            };
            List<Article> preGatherUrlList = new List<Article>();
            List<Article> gatheredArticleList = new List<Article>();
            var gatherWebsite = _gatherConfig.GatherWebsiteList.FirstOrDefault(f => siteKey.Equals(f.Key, StringComparison.CurrentCultureIgnoreCase));
            if (gatherWebsite == null)
            {
                throw new Exception($"{siteKey}采集设置有误！");
            }
            preGatherUrlList = await GetGatherArticleListAsync(gatherWebsite, pageStartNo, pageEndNo, classId, userName);
            foreach (var a in preGatherUrlList)
            {
                var details = await GetArticleDetailsAsync(gatherWebsite, a.Gatherurl, a.ClassId, userName);
                int addResult = await AddArticle(details);
                if (addResult > 0)
                {
                    gatheredArticleList.Add(details);
                }
            }
            gatherResult.PreGatherList = preGatherUrlList;
            gatherResult.GatheredArticleLIst = gatheredArticleList;

            return gatherResult;
        }

        /// <summary>
        /// 获取要采集的文章列表
        /// </summary>
        /// <param name="pageStartNo">文章列表启始页</param>
        /// <param name="pageEndNo">文章列表结束页</param>
        /// <param name="classId">文章类别</param>
        /// <param name="gatherWebsite">站点采集设置</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        public async Task<List<Article>> GetGatherArticleListAsync(GatherWebsite gatherWebsite, int? pageStartNo, int? pageEndNo, int classId, string userName)
        {
            try
            {
                if (gatherWebsite == null)
                {
                    return null;
                }
                List<Article> preGatherUrlList = new List<Article>();

                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;
                
                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string website = string.Format(gatherWebsite.UrlTemp, i);
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            var htmlString = await http.GetStringAsync(website);
                            if (string.IsNullOrEmpty(htmlString) && !string.IsNullOrEmpty(gatherWebsite.FirstPageUrl))
                            {
                                website = gatherWebsite.FirstPageUrl;
                            }
                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseAsync(htmlString);
                            if (gatherWebsite.IsGatherByDetail)
                            {
                                //所有链接均放入列表，在分析详情页时再确认是否采集，适用于在文章列表页无法获取标题详情的页面
                                preGatherUrlList.AddRange(document.QuerySelectorAll(gatherWebsite.ArticleListSelector)
                                    ?.Where(t => t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault() != null 
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    ?.Select(t => new Article()
                                    {
                                        Gatherurl = t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault()
                                        ?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                                        UserName = userName
                                    }).ToList());
                            }
                            else
                            {
                                preGatherUrlList.AddRange(document.QuerySelectorAll(gatherWebsite.ArticleListSelector)
                                    ?.Where(t => t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault() != null
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_siteConfig.NotificationKeywords.Split("and")[0].Trim())
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_siteConfig.NotificationKeywords.Split("and")[1].Trim())
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    .Select(t => new Article()
                                    {
                                        Gatherurl = t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                                        ClassId = _siteConfig.NotificationClassId,
                                        UserName = userName
                                    }).ToList());
                                preGatherUrlList.AddRange(document.QuerySelectorAll(gatherWebsite.ArticleListSelector)
                                    .Where(t => t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault() != null 
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_siteConfig.PolicyKeywords.Trim())
                                    && t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    .Select(t => new Article()
                                    {
                                        Gatherurl = t.QuerySelectorAll(gatherWebsite.TitleSelectorInList).FirstOrDefault()?.Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value,
                                        ClassId = _siteConfig.PolicyClassId,
                                        UserName = userName
                                    }).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("采集文章URL清单时报错" + ex.Message + ex.Message, ex.StackTrace.ToString());
                        //保障出错后继续运行
                        continue;
                    }
                }
                return preGatherUrlList;
            }
            catch (Exception ex)
            {
                throw new Exception("采集文章URL清单时报错", ex);
            }
        }

        /// <summary>
        /// 采集得到文章模型
        /// </summary>
        /// <param name="website">站点采集设置</param>
        /// <param name="url"></param>
        /// <param name="classId">文章类别</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        public async Task<Article> GetArticleDetailsAsync(GatherWebsite website, string url, int? classId, string userName)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    //创建一个自定义的配置文件,使用javascript
                    var config = Configuration.Default.WithJavaScript();
                    HtmlParser htmlParser = website.DetailIsUseJavascript ? new HtmlParser(config) : new HtmlParser();
                    var document = await htmlParser.ParseAsync(htmlString);
                    var titleElement = GetElementByName(document, website, "Title", out int titleSelectorIndex);
                    if ((website.IsGatherByDetail && !CanGatherByTitle(_siteConfig, titleElement, out classId)) || classId == null)
                    {
                        return null;
                    }                    
                    var detailsInfo = document
                        .QuerySelectorAll(website.DetailsSelector)
                        .Select(t => new Article()
                        {
                            Title = GetDetailValueByName(document, website, "Title"),
                            Origin = GetDetailValueByName(document, website, "Origin"),
                            EditTime = DateTime.Now,
                            GatherTime = DateTime.Now,
                            Author = "",
                            UserName = userName,
                            Gatherurl = url,
                            ClassId = classId,
                            Keyword = website.Name
                        })
                        .FirstOrDefault();
                    
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);

                    detailsInfo.Content = GetFormatUrlContent(document, website, pagePath);
                    
                    IGatherHandler<Article> specialHanlder = (IGatherHandler<Article>)GetGatherHandlerInstance(website.Key);
                    detailsInfo = specialHanlder == null
                        ? detailsInfo
                        : await specialHanlder.InvokeAsync(website.SiteUrl, document, detailsInfo);

                    DateTime addTime = DateTime.Now;
                    string strAddTime = GetDetailValueByName(document, website, "AddTime");
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    detailsInfo.Origin = website.Name + detailsInfo.Origin;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("采集得到文章模型时报错" + ex.Message, ex.StackTrace.ToString());
                return null;
            }
        }

        /// <summary>
        /// 根据文章标题判断是否能够采集
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private bool CanGatherByTitle(SiteConfig _siteConfig, AngleSharp.Dom.IElement titleElement, out int? classId)
        {
            if (titleElement == null)
            {
                classId = null;
                return false;
            }
            var notificationCondition = _siteConfig.NotificationKeywords.Split("and");
            if (titleElement.TextContent.ContainsAny(notificationCondition?[0].Trim())
                    && titleElement.TextContent.ContainsAny(notificationCondition?[1].Trim()))
            {
                classId = _siteConfig.NotificationClassId;
                return true;
            }
            if (titleElement.TextContent.ContainsAny(_siteConfig.PolicyKeywords.Trim()))
            {
                classId = _siteConfig.PolicyClassId;
                return true;
            }
            classId = null;
            return false;
        }

        /// <summary>
        /// 得到细节的要采集元素
        /// </summary>
        /// <param name="document"></param>
        /// <param name="gatherWebsite"></param>
        /// <param name="name"></param>
        /// <param name="selectorIndex">返回采用的特征在特征组中的位置</param>
        /// <returns></returns>
        private AngleSharp.Dom.IElement GetElementByName(AngleSharp.Dom.Html.IHtmlDocument document, GatherWebsite gatherWebsite, string name, out int selectorIndex)
        {
            var set = gatherWebsite.GetGatherDetailsByName(name);
            var elementSelectors = gatherWebsite.DetailsList.FirstOrDefault(f => name.Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Selector?.Split(',');
            if (document == null || gatherWebsite == null || elementSelectors == null || string.IsNullOrEmpty(name))
            {
                selectorIndex = 0;
                return null;
            }
            for (int i = 0; i < elementSelectors.Length; i++)
            {
                if (document.QuerySelectorAll(elementSelectors[i]) == null || document.QuerySelectorAll(elementSelectors[i])[set.ValueOrder] == null)
                {
                    continue;
                }
                selectorIndex = i;
                return document.QuerySelectorAll(elementSelectors[i])[set.ValueOrder];
            }
            selectorIndex = 0;
            return null;
        }

        /// <summary>
        /// 得到细节值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="gatherWebsite"></param>
        /// <param name="name"></param>
        private string GetDetailValueByName(AngleSharp.Dom.Html.IHtmlDocument document, GatherWebsite gatherWebsite, string name)
        {
            var set = gatherWebsite.GetGatherDetailsByName(name);
            var element = GetElementByName(document, gatherWebsite, name, out int selectorIndex);
            var valueAttributeNameArr = set.ValueAttributeName?.Split(',');
            var result = string.IsNullOrEmpty(set.ValueAttributeName) || valueAttributeNameArr == null || string.IsNullOrEmpty(valueAttributeNameArr[selectorIndex])
                ? element?.TextContent.Trim()
                : element.GetAttribute(valueAttributeNameArr[selectorIndex])?.Trim();
            result = result.ReplaceEvery(set.BeReplacedStr, set.Replacer, ',').Trim();
            return result;
        }

        /// <summary>
        /// 处理正文中的链接、图片地址， 得到替换为绝对地址的正文html
        /// </summary>
        /// <param name="document"></param>
        /// <param name="gatherWebsite"></param>
        /// <param name="pagePath">文章在网站的除取自身的绝对路径</param>
        /// <returns></returns>
        private string GetFormatUrlContent(AngleSharp.Dom.Html.IHtmlDocument document, GatherWebsite gatherWebsite, string pagePath)
        {
            var ContentSet = gatherWebsite.GetGatherDetailsByName("Content");
            var contentSelectorArr = ContentSet.Selector.Split(',');
            var RemoveSelectorArr = ContentSet.RemoveSelector.Split(',');
            if (contentSelectorArr == null)
            {
                return null;
            }
            foreach(string contentSelector in contentSelectorArr )
            {
                #region 去除要替换的标签
                if (RemoveSelectorArr != null && RemoveSelectorArr.Count() > 0)
                {
                    foreach(string rs  in RemoveSelectorArr)
                    {
                        if (document.QuerySelector(rs) != null)
                        {
                            document.QuerySelector(contentSelector)?.RemoveChild(document.QuerySelector(rs));
                        }
                    }
                }
                #endregion

                #region 处理正文中的链接、图片地址
                foreach (var item in document.QuerySelectorAll(contentSelector + " [src]").ToList())
                {
                    if (item.HasAttribute("src"))
                    {
                        string src = item.GetAttribute("src");
                        item.SetAttribute("src", src.StartsWith("http") ? src : src.StartsWith('/') ? gatherWebsite.SiteUrl.TrimEnd('/') + src : pagePath + src.TrimStart('/'));
                    }
                }
                foreach (var item in document.QuerySelectorAll(contentSelector + " [href]").ToList())
                {
                    if (item.HasAttribute("href"))
                    {
                        string href = item.GetAttribute("href");
                        item.SetAttribute("href", href.StartsWith("http") ? href : href.StartsWith('/') ? gatherWebsite.SiteUrl.TrimEnd('/') + href : pagePath + href.TrimStart('/'));
                    }
                }
                #endregion

                string newStrStyle, strStyle;
                foreach (var item in document.QuerySelectorAll(contentSelector + " p," + contentSelector + " span").ToList())
                {
                    //去掉WORD格式
                    if (item.HasAttribute("style"))
                    {
                        newStrStyle = "";
                        strStyle = item.GetAttribute("style");
                        strStyle = strStyle.Replace("text-indent", "text-indent", StringComparison.CurrentCultureIgnoreCase)
                            .Replace("text-align", "text-align", StringComparison.CurrentCultureIgnoreCase);
                        if (strStyle.Contains("text-indent"))
                        {
                            newStrStyle = "text-indent" + strStyle.Split("text-indent")?[1]?.Split(";")?[0] + ";";
                        }
                        if (strStyle.Contains("text-align"))
                        {
                            newStrStyle = "text-align" + strStyle.Split("text-align")?[1]?.Split(";")?[0] + ";";
                        }
                        if (string.IsNullOrEmpty(newStrStyle))
                        {
                            item.RemoveAttribute("style");
                        }
                        else
                        {
                            item.SetAttribute("style", newStrStyle);
                        }

                    }
                    if (item.HasAttribute("face"))
                    {
                        item.RemoveAttribute("face");
                    }
                }

                var result = document.QuerySelector(contentSelector)?.InnerHtml;
                if (string.IsNullOrEmpty(result))
                {
                    continue;
                }
                return result;
            }
            return null;           
        }

        /// <summary>
        /// 将采集的文章持久化
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private async Task<int> AddArticle(Article details)
        {
            try
            {
                if (details == null || string.IsNullOrEmpty(details.Title) || string.IsNullOrEmpty(details.Gatherurl) || string.IsNullOrEmpty(details.Content))
                {
                    return 0;
                }

                var modelInDatabase = await _articleService.GetModelByAsync(f => details.Title.ToLower() == f.Title.ToLower() && details.Gatherurl.ToLower() == f.Gatherurl.ToLower());
                if (modelInDatabase != null && !string.IsNullOrEmpty(modelInDatabase.Title))
                {
                    return 0;
                }
                Thread.Sleep(50);
                return await _articleService.AddAsync(details);
            }
            catch (Exception ex)
            {
                _logger.LogError("将采集的文章持久化时报错" + ex.Message, ex.StackTrace.ToString());
                return 0;
            }
        }

        /// <summary>
        /// 根据高新区网站接口获取网页附件下载相关数据
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private async Task<List<string>> GetCdhtAttachmentDataAsync(string cid, string n)
        {
            using (HttpClient http = new HttpClient())
            {
                var htmlString = await http.GetStringAsync($"{_gatherConfig.GatherWebsiteList.FirstOrDefault(f => f.Key == "cdht")?.SiteUrl}/attachment_url.jspx?cid={cid}&n={n}");
                if (string.IsNullOrEmpty(htmlString))
                {
                    return null;
                }
                var result = JsonConvert.DeserializeObject<List<string>>(htmlString);
                return result;
            }
        }

        /// <summary>
        /// 从高新区网页中抓取附件链接生成参数
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private List<string> GetCdhtAttachmentPara(AngleSharp.Dom.Html.IHtmlDocument document)
        {
            if (document == null)
            {
                return null;
            }
            var pageHtml = document.QuerySelector("html")?.OuterHtml;
            if (pageHtml == null)
            {
                return null;
            }
            var startPosition = pageHtml.IndexOf("Cms.attachment(") + "Cms.attachment(".Length;
            var endPosition = pageHtml.IndexOf("\");\n  Cms.viewCount(");
            if (startPosition <= 0 || endPosition <= 0 || endPosition <= startPosition)
            {
                return null;
            }
            var strPara = pageHtml.Substring(startPosition, endPosition - startPosition).Replace("\"", "");
            if (string.IsNullOrEmpty(strPara) || strPara.Split(',').Length != 4)
            {
                return null;
            }
            return strPara.Split(',').ToList();
        }

        /// <summary>
        /// 生成高新区网页链接地址
        /// </summary>
        /// <param name="paras"></param>
        /// <returns>链接标签ID与链接地址Dictionany</returns>
        private async Task<Dictionary<string, string>> GetCdhtAttachmentUrlAsync(List<string> paras)
        {
            var result = new Dictionary<string, string>();
            try
            {
                var attachmentData = await GetCdhtAttachmentDataAsync(paras[1], paras[2]);
                for (var i = 0; i < paras[2].ToInt(); i++)
                {
                    result.Add(paras[3] + i, (string.IsNullOrEmpty(paras[0]) ? _gatherConfig.GatherWebsiteList.FirstOrDefault(f => "cdht".Equals(f.Key)).SiteUrl : paras[0]) + "/attachment.jspx?cid=" + paras[1] + "&i=" + i + attachmentData[i]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("生成高新区网页链接地址", ex);
            }
            return result;
        }

        /// <summary>
        /// 高新区网站文章正文附加处理
        /// </summary>
        /// <param name="document"></param>
        /// <param name="oldArticle"></param>
        /// <returns></returns>
        private async Task<Article> CdhtContentCallbackAsync(AngleSharp.Dom.Html.IHtmlDocument document, Article oldArticle)
        {
            var detailsInfo = oldArticle;
            #region 处理正文中附件下载的链接，高新区附件下载地址是动态生成的
            var urls = await GetCdhtAttachmentUrlAsync(GetCdhtAttachmentPara(document));
            var isAttachmentGathered = false;
            foreach (var item in document.QuerySelectorAll("div[style='line-height:30px;color:#919191'] a").ToList())
            {
                var tagId = item.GetAttribute("id");
                if (string.IsNullOrEmpty(tagId) || string.IsNullOrEmpty(urls[tagId]))
                {
                    continue;
                }
                item.SetAttribute("href", urls[tagId]);
                isAttachmentGathered = true;
            }
            var attachmentHtml = isAttachmentGathered ?
                document.QuerySelector("div[style='line-height:30px;color:#919191']")?.OuterHtml
                : $"<p>附件请点击 <i><a href='{detailsInfo.Gatherurl}' title='文章原文'>原文</a></i> 下载";
            #endregion
            detailsInfo.Content += attachmentHtml;//添加附件下载部分
            return null;
        }

        /// <summary>
        /// 动态创建站点采集特殊处理对象
        /// </summary>
        /// <param name="siteKey"></param>
        /// <returns></returns>
        private object GetGatherHandlerInstance(string siteKey)
        {
            try
            {
                string dllPath = Path.Combine(AppContext.BaseDirectory, _siteConfig.GatherSpecialHandlerDllName);
                Assembly assemblyApi = Assembly.LoadFrom(dllPath);
                Type type = assemblyApi.GetType(_siteConfig.GatherSpecialHandlerAssemblyName + "." + siteKey.ToTitleCase() + _siteConfig.GatherSpecialHandlerSuffix);
                if (type == null)
                {
                    return null;
                }
                var o = Activator.CreateInstance(type);
                Console.WriteLine(siteKey + "对象创建OK！"); 
                return o;
            }
            catch (Exception ex)
            {
                throw new Exception("动态创建站点采集特殊处理对象", ex);
            }
        }
    }    
}
