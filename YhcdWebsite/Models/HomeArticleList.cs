using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YhcdWebsite.Models
{
    /// <summary>
    /// 首页文章列表
    /// </summary>
    public class HomeArticleList
    {
        //图片新闻列表
        public IEnumerable<ArticleView> PicArticleList;
        //文字新闻列表
        public IEnumerable<ArticleView> TextArticleList;
    }
}
