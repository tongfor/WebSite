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
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public partial class ArticleView : Article
    {
        public ArticleView()
        {
            this.ToPOCO();
        }

        /// <summary>
        /// 文章类别名
        /// </summary>
        [DisplayName("文章类别")]
        public string ArticleClassName { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayName("添加时间")]
        //public new Nullable<System.DateTime> AddTime { get; set; }

        //[DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        //[DisplayName("修改时间")]
        //public new Nullable<System.DateTime> EditTime { get; set; }

        /// <summary>
        /// 还原父类
        /// </summary>
        /// <returns></returns>
        public Article ToOriginal()
        {
            return new Article()
            {
                Id = this.Id,
                ClassId = this.ClassId,
                Title = this.Title,
                TitleColor = this.TitleColor,
                Content = this.Content,
                UserName = this.UserName,
                LookCount = this.LookCount,
                AddHTMLUrl = this.AddHTMLUrl,
                IsTop = this.IsTop,
                IsMarquee = this.IsMarquee,
                Introduce = this.Introduce,
                IntroduceImg = this.IntroduceImg,
                AddTime = this.AddTime,
                EditTime = this.EditTime,
            };
        }
    }
}
