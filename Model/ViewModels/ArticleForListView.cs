using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ArticleForListView : BaseRequest
    {
        public ArticleForListView()
        {
            ArticleViewList = new List<ArticleView>();
        }

        /// <summary>
        /// 文章列表
        /// </summary>
        public List<ArticleView> ArticleViewList { get; set; }
    }
}
