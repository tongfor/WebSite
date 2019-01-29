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

        public GatherService(IOptionsSnapshot<SiteConfig> siteConfigOptions, IOptionsSnapshot<GatherConfig> gatherConfigOptions, IArticleService articleService, ILogger<GatherService> logger)
        {
            _siteConfig = siteConfigOptions.Value;
            _gatherConfig = gatherConfigOptions.Value;
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
                SiteKey = siteKey,
                ResultShowDomainName = _siteConfig.DefaultDomainName
            };
            List<Article> preGatherUrlList = new List<Article>();
            List<Article> gatheredArticleList = new List<Article>();
            var website = _gatherConfig?.GatherWebsiteList?.FirstOrDefault(f => siteKey.Equals(f.Key, StringComparison.CurrentCultureIgnoreCase));
            if (website == null)
            {
                throw new Exception($"{siteKey}采集设置有误！");
            }
            preGatherUrlList = await GetGatherArticleListAsync(website, pageStartNo, pageEndNo, classId, userName);
            foreach (var a in preGatherUrlList)
            {
                var details = await GetArticleDetailsAsync(website, a.Gatherurl, a.ClassId, userName);
                int addResult = await AddArticle(details);
                if (addResult > 0)
                {
                    gatheredArticleList.Add(details);
                }
            }
            gatherResult.PreGatherList = preGatherUrlList;
            gatherResult.GatheredArticleList = gatheredArticleList.OrderByDescending(o => o.AddTime).ToList();
            gatherResult.SiteName = website.Name;

            return gatherResult;
        }

        #region private

        /// <summary>
        /// 获取要采集的文章列表
        /// </summary>
        /// <param name="pageStartNo">文章列表启始页</param>
        /// <param name="pageEndNo">文章列表结束页</param>
        /// <param name="classId">文章类别</param>
        /// <param name="website">站点采集设置</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        private async Task<List<Article>> GetGatherArticleListAsync(GatherWebsite website, int? pageStartNo, int? pageEndNo, int classId, string userName)
        {
            try
            {
                if (website == null)
                {
                    return null;
                }
                List<Article> preGatherUrlList = new List<Article>();

                pageStartNo = pageStartNo == null || pageStartNo <= 0 ? 1 : pageStartNo;
                pageEndNo = pageEndNo == null || pageEndNo <= 0 ? 1 : pageEndNo;

                for (int i = pageStartNo.Value; i <= pageEndNo.Value; i++)
                {
                    string websiteGatherUrl = string.Format(website.UrlTemp, i);
                    try
                    {
                        using (HttpClient http = new HttpClient())
                        {
                            http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                            if (i == 1 && !string.IsNullOrEmpty(website.FirstPageUrl))
                            {
                                websiteGatherUrl = website.FirstPageUrl;
                            }
                            var htmlString = await http.GetStringAsync(websiteGatherUrl);

                            HtmlParser htmlParser = new HtmlParser();
                            var document = await htmlParser.ParseAsync(htmlString);
                            if (website.IsGatherByDetail)
                            {
                                //所有链接均放入列表，在分析详情页时再确认是否采集，适用于在文章列表页无法获取标题详情的页面
                                preGatherUrlList.AddRange(document.QuerySelectorAll(website.ArticleListSelector)
                                    ?.Where(t => t.QuerySelectorAll(website.TitleSelectorInList) != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault() != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    ?.Select(t => new Article()
                                    {
                                        Gatherurl = GetAbsoluteUrl(website.SiteUrl, t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value),
                                        UserName = userName
                                    }).ToList());
                            }
                            else
                            {
#if DEBUG
                                List<AngleSharp.Dom.IElement> itemList = new List<AngleSharp.Dom.IElement>();
                                foreach (var item in document.QuerySelectorAll(website.ArticleListSelector))
                                {
                                    if (item.QuerySelector(website.TitleSelectorInList) != null
                                        && item.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault() != null
                                        && item.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_gatherConfig.PolicyKeywords.Trim())
                                        && item.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords.Trim())
                                        )
                                    {
                                        itemList.Add(item);
                                    }
                                }
#endif
                                preGatherUrlList.AddRange(document.QuerySelectorAll(website.ArticleListSelector)
                                    ?.Where(t => t.QuerySelectorAll(website.TitleSelectorInList) != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault() != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_gatherConfig.NotificationKeywords.Split("and")[0].Trim())
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_gatherConfig.NotificationKeywords.Split("and")[1].Trim())
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    .Select(t => new Article()
                                    {
                                        Gatherurl = GetAbsoluteUrl(website.SiteUrl, t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value),
                                        ClassId = _siteConfig.NotificationClassId,
                                        UserName = userName
                                    }).ToList());
                                preGatherUrlList.AddRange(document.QuerySelectorAll(website.ArticleListSelector)
                                    .Where(t => t.QuerySelectorAll(website.TitleSelectorInList) != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault() != null
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ContainsAny(_gatherConfig.PolicyKeywords.Trim())
                                    && t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().TextContent.ExcludeAll(_gatherConfig.ExcludeKeywords))
                                    .Select(t => new Article()
                                    {
                                        Gatherurl = GetAbsoluteUrl(website.SiteUrl, t.QuerySelectorAll(website.TitleSelectorInList).FirstOrDefault().Attributes.FirstOrDefault(f => "href".Equals(f.Name, StringComparison.CurrentCultureIgnoreCase))?.Value),
                                        ClassId = _siteConfig.PolicyClassId,
                                        UserName = userName
                                    }).ToList());
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.Contains("404"))
                        {
                            _logger.LogError(ex, "采集文章URL清单时报错" + ex.Message);
                        }
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
        private async Task<Article> GetArticleDetailsAsync(GatherWebsite website, string url, int? classId, string userName)
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
                            Title = GetDetailValueByName(document, website, "Title", url),
                            Origin = GetDetailValueByName(document, website, "Origin", url),
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

                    detailsInfo.Content = GetFormatUrlContent(document, website, pagePath, url);
                    
                    IGatherHandler<Article> specialHanlder = (IGatherHandler<Article>)GetGatherHandlerInstance(website.Key);
                    detailsInfo = specialHanlder == null
                        ? detailsInfo
                        : await specialHanlder.InvokeAsync(website.SiteUrl, document, detailsInfo);

                    DateTime addTime = DateTime.Now;
                    string strAddTime = GetDetailValueByName(document, website, "AddTime", url);
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    detailsInfo.Origin = website.Name + detailsInfo.Origin.Replace(website.Name, "");
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("404"))
                {
                    _logger.LogError(ex, $"采集{url}文章详情时报错：" + ex.Message);
                }               
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
            var notificationCondition = _gatherConfig.NotificationKeywords.Split("and");
            if (titleElement.TextContent.ContainsAny(notificationCondition?[0].Trim())
                    && titleElement.TextContent.ContainsAny(notificationCondition?[1].Trim()))
            {
                classId = _siteConfig.NotificationClassId;
                return true;
            }
            if (titleElement.TextContent.ContainsAny(_gatherConfig.PolicyKeywords.Trim()))
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
            try
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
                    if (null == document.QuerySelectorAll(elementSelectors[i]) || 0 == document.QuerySelectorAll(elementSelectors[i]).Length || null == document.QuerySelectorAll(elementSelectors[i])[set.ValueOrder])
                    {
                        continue;
                    }
                    selectorIndex = i;
                    return document.QuerySelectorAll(elementSelectors[i])[set.ValueOrder];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"获取{name}的html元素时报错：" + ex.Message);
            }
            selectorIndex = 0;
            return null;
        }

        /// <summary>
        /// 得到细节值
        /// </summary>
        /// <param name="document"></param>
        /// <param name="website"></param>
        /// <param name="name"></param>
        private string GetDetailValueByName(AngleSharp.Dom.Html.IHtmlDocument document, GatherWebsite website, string name, string url)
        {
            var ss = document.QuerySelector("html").OuterHtml;
            try
            {
                var detailsSet = website.GetGatherDetailsByName(name);
                if (string.IsNullOrEmpty(detailsSet.Selector))
                {
                    return string.Empty;
                }
                var element = GetElementByName(document, website, name, out int selectorIndex);
                if (null == element)
                {
                    return null;
                }
                var valueAttributeNameArr = detailsSet.ValueAttributeName?.Split(',');
                var result = string.IsNullOrEmpty(detailsSet.ValueAttributeName) || valueAttributeNameArr == null || string.IsNullOrEmpty(valueAttributeNameArr[selectorIndex])
                    ? element?.TextContent.Trim()
                    : element?.GetAttribute(valueAttributeNameArr[selectorIndex])?.Trim();
                if (detailsSet.ValueForward != null && detailsSet.ValueForward != null && (detailsSet.ValueForward.Length >= 0 || detailsSet.ValueAfter.Length >= 0))
                {
                    //如果有前后特征则取前后特征之间的值
                    result = string.IsNullOrEmpty(detailsSet.ValueAfter)
                        ? result.Substring(result.IndexOf(detailsSet.ValueForward) + detailsSet.ValueForward.Length)
                        : result.Substring(result.IndexOf(detailsSet.ValueForward) + detailsSet.ValueForward.Length, result.IndexOf(detailsSet.ValueAfter) - result.IndexOf(detailsSet.ValueForward) - detailsSet.ValueForward.Length);
                }
                result = result?.ReplaceEvery(detailsSet.BeReplacedStr, detailsSet.Replacer, ',')?.Trim();
                if (string.IsNullOrEmpty(result))
                {
                    _logger.LogWarning($"在{url}采集{name}的内容时采集失败");
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"采集{name}的内容时报错：" + ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        /// 处理正文中的链接、图片地址， 得到替换为绝对地址的正文html
        /// </summary>
        /// <param name="document"></param>
        /// <param name="gatherWebsite"></param>
        /// <param name="pagePath">文章在网站的除取自身的绝对路径</param>
        /// <returns></returns>
        private string GetFormatUrlContent(AngleSharp.Dom.Html.IHtmlDocument document, GatherWebsite gatherWebsite, string pagePath,string url)
        {
            var ContentSet = gatherWebsite.GetGatherDetailsByName("Content");
            var contentSelectorArr = ContentSet.Selector.Split(',');
            var RemoveSelectorArr = ContentSet.RemoveSelector.Split(',');
            if (contentSelectorArr == null)
            {
                return null;
            }
            foreach (string contentSelector in contentSelectorArr)
            {
#region 去除要替换的标签
                if (RemoveSelectorArr != null && RemoveSelectorArr.Count() > 0)
                {
                    foreach (string rs in RemoveSelectorArr)
                    {
                        try
                        {
                            if (rs.Length > 0 && document.QuerySelector(rs) != null)
                            {
                                document.QuerySelector(contentSelector)?.RemoveChild(document.QuerySelector(rs));
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "采集文章去除要替换的标签报错！");
                            continue;
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
            _logger.LogWarning($"在{url}采集文章正文的内容时采集失败");
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
                _logger.LogError(ex, "将采集的文章持久化时报错" + ex.Message);
                return 0;
            }
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

        /// <summary>
        /// 拼接出网站绝对路径
        /// </summary>
        /// <param name="site"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private string GetAbsoluteUrl(string site, string url)
        {
            if (url.StartsWith("http") || string.IsNullOrEmpty(site) || string.IsNullOrEmpty(url))
            {
                return url;
            }
            return $"{site.TrimEnd('/')}/{url.TrimStart('/')}";
        } 
#endregion
    }    
}
