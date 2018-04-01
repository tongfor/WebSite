using System.Text;

namespace System.Web.Mvc
{
    /// <summary>
    /// MVC html扩展
    /// </summary>
    public static class HtmlExtensionUtil
    {

        /// <summary>
        /// AJAX分布控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="isShowTotal"></param>
        /// <returns></returns>
        public static string ShowPageNavigate(int currentPage, int pageSize, int totalCount, bool isShowTotal = false)
        {
            var redirectTo = HttpContext.Current.Request.Url.AbsolutePath;
            pageSize = pageSize <= 0 ? 3 : pageSize;
            var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
            var output = new StringBuilder();
            if (totalPages > 1)
            {
                //if (currentPage != 1)
                {//处理首页连接
                    output.AppendFormat("<a class='pageLink' href='javascript:void(0);' onclick='FY(1)' >首页</a> ");
                }
                if (currentPage > 1)
                {//处理上一页的连接
                    output.AppendFormat("<a class='pageLink'  href='javascript:void(0);' onclick='FY({0})'>上一页</a> ",  currentPage - 1);
                }
                else
                {
                    // output.Append("<span class='pageLink'>上一页</span>");
                }

                output.Append(" ");
                int currint = 5;
                for (int i = 0; i <= 10; i++)
                {//一共最多显示10个页码，前面5个，后面5个
                    if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                    {
                        if (currint == i)
                        {//当前页处理
                            //output.Append(string.Format("[{0}]", currentPage));
                            output.AppendFormat("<a class='cpb'   href='javascript:void(0);'>{0}</a> ", currentPage);
                        }
                        else
                        {//一般页处理
                            output.AppendFormat("<a class='pageLink'    href='javascript:void(0);' onclick='FY({0})'>{1}</a> ", currentPage + i - currint, currentPage + i - currint);
                        }
                    }
                    output.Append(" ");
                }
                if (currentPage < totalPages)
                {//处理下一页的链接
                    output.AppendFormat("<a class='pageLink' href='javascript:void(0);' onclick='FY({0})'>下一页</a> ", currentPage + 1);
                }
                else
                {
                    //output.Append("<span class='pageLink'>下一页</span>");
                }
                output.Append(" ");
                if (currentPage != totalPages)
                {
                    output.AppendFormat("<a class='pageLink' href='javascript:void(0);' onclick='FY({0})'>末页</a> ", totalPages);
                }
                output.Append(" ");
            }
            if (isShowTotal)
            {
                output.AppendFormat("第 {0} 页 / 共 {1} 页", currentPage, totalPages);//这个统计加不加都行
            }

            return output.ToString();
        }

        /// <summary>
        /// 分布控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="isShowTotal"></param>
        /// <returns></returns>
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, bool isShowTotal = true)
        {
            if (htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url != null)
            {
                var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
                pageSize = pageSize == 0 ? 3 : pageSize;
                var totalPages = Math.Max((totalCount + pageSize - 1)/pageSize, 1); //总页数
                var output = new StringBuilder();
                if (totalPages > 1)
                {
                    output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex=1&pageSize={1}'>首页</a></li> ", redirectTo,
                        pageSize);
                    if (currentPage > 1)
                    {
//处理上一页的连接
                        output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>上一页</a></li> ",
                            redirectTo, currentPage - 1, pageSize);
                    }

                    output.Append(" ");
                    int currint = 5;
                    for (int i = 0; i <= 10; i++)
                    {
//一共最多显示10个页码，前面5个，后面5个
                        if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                        {
                            if (currint == i)
                            {
//当前页处理                           
                                output.AppendFormat("<li><a class='cpb' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a></li> ",
                                    redirectTo, currentPage, pageSize, currentPage);
                            }
                            else
                            {
//一般页处理
                                output.AppendFormat(
                                    "<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>{3}</a></li> ", redirectTo,
                                    currentPage + i - currint, pageSize, currentPage + i - currint);
                            }
                        }
                        output.Append(" ");
                    }
                    if (currentPage < totalPages)
                    {
//处理下一页的链接
                        output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>下一页</a></li> ",
                            redirectTo, currentPage + 1, pageSize);
                    }

                    output.Append(" ");
                    if (currentPage != totalPages)
                    {
                        output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}'>末页</a></li> ",
                            redirectTo, totalPages, pageSize);
                    }
                    output.Append(" ");
                }
                if (isShowTotal)
                {
                    output.AppendFormat("<li><label> 第{0}页 / 共{1}页 </label>", currentPage, totalPages); //这个统计加不加都行
                }

                return new HtmlString(output.ToString());
            }
            return new HtmlString(string.Empty);
        }
        /// <summary>
        /// 带跳转条件参数分布控件
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="isShowTotal"></param>
        /// <returns></returns>
        public static HtmlString ShowPageNavigate(this HtmlHelper htmlHelper, int currentPage, int pageSize, int totalCount, string hrefParam, bool isShowTotal = true)
        {
            if (htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url != null)
            {

                var redirectTo = htmlHelper.ViewContext.RequestContext.HttpContext.Request.Url.AbsolutePath;
                pageSize = pageSize == 0 ? 3 : pageSize;
                var totalPages = Math.Max((totalCount + pageSize - 1) / pageSize, 1); //总页数
                var output = new StringBuilder();
                if (hrefParam != "" && hrefParam.Length > 0)
                {
                    if (totalPages > 1)
                    {
                        output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex=1&pageSize={1}&{2}'>首页</a></li> ", redirectTo,
                            pageSize, hrefParam);
                        if (currentPage > 1)
                        {
                            //处理上一页的连接
                            output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&{3}'>上一页</a></li> ",
                                redirectTo, currentPage - 1, pageSize, hrefParam);
                        }

                        output.Append(" ");
                        int currint = 5;
                        for (int i = 0; i <= 10; i++)
                        {
                            //一共最多显示10个页码，前面5个，后面5个
                            if ((currentPage + i - currint) >= 1 && (currentPage + i - currint) <= totalPages)
                            {
                                if (currint == i)
                                {
                                    //当前页处理                           
                                    output.AppendFormat("<li><a class='cpb' href='{0}?pageIndex={1}&pageSize={2}&{4}'>{3}</a></li> ",
                                        redirectTo, currentPage, pageSize, currentPage, hrefParam);
                                }
                                else
                                {
                                    //一般页处理
                                    output.AppendFormat(
                                        "<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&{4}'>{3}</a></li> ", redirectTo,
                                        currentPage + i - currint, pageSize, currentPage + i - currint, hrefParam);
                                }
                            }
                            output.Append(" ");
                        }
                        if (currentPage < totalPages)
                        {
                            //处理下一页的链接
                            output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&{3}'>下一页</a></li> ",
                                redirectTo, currentPage + 1, pageSize, hrefParam);
                        }

                        output.Append(" ");
                        if (currentPage != totalPages)
                        {
                            output.AppendFormat("<li><a class='pageLink' href='{0}?pageIndex={1}&pageSize={2}&{3}'>末页</a></li> ",
                                redirectTo, totalPages, pageSize, hrefParam);
                        }
                        output.Append(" ");
                    }
                    if (isShowTotal)
                    {
                        output.AppendFormat("<li><label> 第{0}页 / 共{1}页 </label>", currentPage, totalPages); //这个统计加不加都行
                    }

                }
                return new HtmlString(output.ToString());
            }

            return new HtmlString(string.Empty);
        }
    }
}