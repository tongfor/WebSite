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
        //政策信息图片新闻列表
        public IEnumerable<ArticleView> PolicyPicArticleList;
        //政策信息文字新闻列表
        public IEnumerable<ArticleView> PolicyTextArticleList;
        //申报快讯图片新闻列表
        public IEnumerable<ArticleView> NotificationPicArticleList;
        //申报快讯文字新闻列表
        public IEnumerable<ArticleView> NotificationTextArticleList;
    }
}
