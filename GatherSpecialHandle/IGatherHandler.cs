using Models;
using System.Threading.Tasks;

namespace GatherSpecialHandler
{
    /// <summary>
    /// 站点采集特殊处理接口
    /// </summary>
    public interface IGatherHandler<T> where T : class
    {
        Task<T> InvokeAsync(string siteUrl, AngleSharp.Dom.Html.IHtmlDocument document, T oldArticle);
    }
}
