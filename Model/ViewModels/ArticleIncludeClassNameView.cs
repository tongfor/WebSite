/** 
* ArticleDetailView.cs
*
* 功 能： 用于视图的文章内容查看模型
* 类 名： ArticleDetailView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 10:24:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.ComponentModel;

namespace Models
{
    public partial class ArticleIncludeClassNameView
    {
        /// <summary>
        /// 文章类别名
        /// </summary>
        [DisplayName("文章类别")]
        public string ArticleClassName { get; set; }

        /// <summary>
        /// 文章内容对象
        /// </summary>
        public Article Article { get; set; }
    }
}
