using Models;
using System.Threading.Tasks;

namespace GatherExtensionBase
{
    /// <summary>
    /// 站点采集特殊处理接口
    /// </summary>
    public interface IGatherExtension
    {
        Task<Article> InvokeAsync(string siteUrl, AngleSharp.Dom.Html.IHtmlDocument document, Article oldArticle);
    }
}
