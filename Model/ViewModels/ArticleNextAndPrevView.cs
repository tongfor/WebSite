/** 
* ArticleNextAndPrevView.cs
*
* 功 能： 包含文章下一条及上一条数据的模型
* 类 名： ArticleNextAndPrevView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/6/12 10:24:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace Models
{
    public class ArticleNextAndPrevView
    {
        /// <summary>
        /// 当前文章
        /// </summary>
        public ArticleView CurrentArticle { get; set; }
       
        /// <summary>
        /// 下一条文章
        /// </summary>
        public ArticleView NextArticle { get; set; }

        /// <summary>
        /// 上一条文章
        /// </summary>
        public ArticleView PrevArticle { get; set; }
    }
}
