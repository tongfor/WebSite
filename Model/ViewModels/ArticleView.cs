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
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public partial class ArticleView
    {
        [Key]
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public string Title { get; set; }
        public string TitleColor { get; set; }
        public string Content { get; set; }
        public string Introduce { get; set; }
        public string IntroduceImg { get; set; }
        public string Author { get; set; }
        public string Origin { get; set; }
        public string UserName { get; set; }
        public int? LookCount { get; set; }
        public string AddHtmlurl { get; set; }
        public sbyte? IsTop { get; set; }
        public sbyte? IsMarquee { get; set; }
        public int? Sort { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
        public sbyte? IsDel { get; set; }

        /// <summary>
        /// 文章类别名
        /// </summary>
        [DisplayName("文章类别")]
        public string ArticleClassName { get; set; }

        public ArticleView()
        {
        }

        public ArticleView(Article article)
        {
            this.Id = article.Id;
            this.ClassId = article.ClassId;
            this.Title = article.Title;
            this.TitleColor = article.TitleColor;
            this.Content = article.Content;
            this.UserName = article.UserName;
            this.LookCount = article.LookCount;
            this.AddHtmlurl = article.AddHtmlurl;
            this.IsTop = article.IsTop;
            this.IsMarquee = article.IsMarquee;
            this.Introduce = article.Introduce;
            this.IntroduceImg = article.IntroduceImg;
            this.AddTime = article.AddTime;
            this.EditTime = article.EditTime;
        }       

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
                AddHtmlurl = this.AddHtmlurl,
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
