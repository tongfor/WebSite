using AngleSharp.Dom.Html;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GatherSpecialHandler
{
    public class CdhtSpecialHandler : IGatherHandler
    {
        /// <summary>
        /// 对高新区的文章正文进行特殊处理
        /// </summary>
        /// <param name="siteUrl"></param>
        /// <param name="document"></param>
        /// <param name="oldArticle"></param>
        /// <returns></returns>
        public async Task<Article> InvokeAsync(string siteUrl, IHtmlDocument document, Article oldArticle)
        {
            var detailsInfo = oldArticle;
            #region 处理正文中附件下载的链接，高新区附件下载地址是动态生成的
            var urls = await GetCdhtAttachmentUrlAsync(siteUrl, GetCdhtAttachmentPara(document));
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
                document.QuerySelector("div[style='line-height:30px;color:#919191']")?.OuterHtml.Replace("&amp;", "&")
                : $"<p>附件请点击 <i><a href='{detailsInfo.Gatherurl}' title='文章原文'>原文</a></i> 下载";
            #endregion
            detailsInfo.Content += attachmentHtml;//添加附件下载部分
            return detailsInfo;
        }

        /// <summary>
        /// 根据高新区网站接口获取网页附件下载相关数据
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private async Task<List<string>> GetCdhtAttachmentDataAsync(string siteUrl, string cid, string n)
        {
            using (HttpClient http = new HttpClient())
            {
                var htmlString = await http.GetStringAsync($"{siteUrl}/attachment_url.jspx?cid={cid}&n={n}");
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
        private List<string> GetCdhtAttachmentPara(IHtmlDocument document)
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
        private async Task<Dictionary<string, string>> GetCdhtAttachmentUrlAsync(string siteUrl, List<string> paras)
        {
            var result = new Dictionary<string, string>();
            try
            {
                var attachmentData = await GetCdhtAttachmentDataAsync(siteUrl, paras[1], paras[2]);
                for (var i = 0; i < int.Parse(paras[2]); i++)
                {
                    result.Add(paras[3] + i, (string.IsNullOrEmpty(paras[0])
                        ? siteUrl
                        : paras[0]) + "/attachment.jspx?cid=" + paras[1] + "&i=" + i + attachmentData[i]);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("生成高新区网页链接地址", ex);
            }
            return result;
        }
    }
}
