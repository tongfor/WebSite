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
using Common.Config;
using Models;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using IBLL;
using System.Threading;
using Microsoft.Extensions.Options;

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

        public GatherService(IOptionsMonitor<SiteConfig> siteConfigOptions, IOptionsMonitor<GatherConfig> gatherConfigOptions, IArticleService articleService)
        {
            _siteConfig = siteConfigOptions.CurrentValue;
            _gatherConfig = gatherConfigOptions.CurrentValue;
            _articleService = articleService;
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
                        //保障出错后继续运行
                        continue;
                    }
                }
                return preGatherUrlList;
            }
            catch (Exception ex)
            {
                throw new Exception("采集文章URL清单", ex);
            }
        }

        /// <summary>
        /// 采集得到文章模型
        /// </summary>
        /// <param name="gatherWebsite">站点采集设置</param>
        /// <param name="url"></param>
        /// <param name="classId">文章类别</param>
        /// <param name="userName">采集员账号</param>
        /// <returns></returns>
        public async Task<Article> GetArticleDetailsAsync(GatherWebsite gatherWebsite, string url, int? classId, string userName)
        {
            try
            {
                using (HttpClient http = new HttpClient())
                {
                    http.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0");
                    var htmlString = await http.GetStringAsync(url);
                    //创建一个自定义的配置文件,使用javascript
                    var config = Configuration.Default.WithJavaScript();
                    HtmlParser htmlParser = gatherWebsite.DetailIsUseJavascript ? new HtmlParser(config) : new HtmlParser();
                    var document = await htmlParser.ParseAsync(htmlString);
                    var titleElement = GetElementByName(document, gatherWebsite, "Title", out int titleSelectorIndex);
                    if ((gatherWebsite.IsGatherByDetail && !CanGatherByTitle(_siteConfig, titleElement, out classId)) || classId == null)
                    {
                        return null;
                    }                    
                    var detailsInfo = document
                        .QuerySelectorAll(gatherWebsite.DetailsSelector)
                        .Select(t => new Article()
                        {
                            Title = GetDetailValueByName(document, gatherWebsite, "Title"),
                            Origin = GetDetailValueByName(document, gatherWebsite, "Origin"),
                            EditTime = DateTime.Now,
                            GatherTime = DateTime.Now,
                            Author = "",
                            UserName = userName,
                            Gatherurl = url,
                            ClassId = classId,
                            Keyword = gatherWebsite.Name
                        })
                        .FirstOrDefault();
                    
                    string pagePath = url.Substring(0, url.LastIndexOf('/') + 1);

                    detailsInfo.Content = GetFormatUrlContent(document, gatherWebsite, pagePath);
                    
                    DateTime addTime = DateTime.Now;
                    string strAddTime = GetDetailValueByName(document, gatherWebsite, "AddTime");
                    DateTime.TryParse(strAddTime, out addTime);
                    detailsInfo.AddTime = addTime;
                    detailsInfo.Origin = gatherWebsite.Name + detailsInfo.Origin;
                    return detailsInfo;
                }
            }
            catch (Exception ex)
            {
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

                foreach (var item in document.QuerySelectorAll(contentSelector + " p," + contentSelector + " span").ToList())
                {
                    //去掉WORD格式
                    if (item.HasAttribute("style"))
                    {
                        string newStrStyle = "";
                        string strStyle = item.GetAttribute("style");
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
                return 0;
            }
        }
    }
}
