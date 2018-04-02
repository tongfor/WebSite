/** 
* ArticleMetadata.cs
*
* 功 能： Article元数据类
* 类 名： ArticleMetadata
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/25 17:05:34   李庸    初版
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
    public class ArticleMetadata
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("文章类别")]
        [Required]
        public Nullable<int> ClassId { get; set; }
        [DisplayName("标题")]
        public string Title { get; set; }
        [DisplayName("标题颜色")]
        public string TitleColor { get; set; }
        [DisplayName("内容")]
        public string Content { get; set; }
        [DisplayName("文章简介")]
        public string Introduce { get; set; }
        [DisplayName("首页图片")]
        public string IntroduceImg { get; set; }
        [DisplayName("文章作者")]
        public string Author { get; set; }
        [DisplayName("文件来源")]
        public string Origin { get; set; }
        [DisplayName("添加人用户名")]
        [Required]
        public string UserName { get; set; }
        [DisplayName("文章查看次数")]
        public Nullable<int> LookCount { get; set; }
        [DisplayName("文章生成的url")]
        public string AddHTMLUrl { get; set; }
        private sbyte _isTop = 0;
        [DisplayName("是否置顶")]
        public Nullable<sbyte> IsTop
        {
            get { return _isTop; }
            set
            {
                if (value != null)
                {
                    _isTop = value.Value;
                }
            }
        }
        private sbyte _isMarquee = 0;
        [DisplayName("是否滚动")]
        public Nullable<sbyte> IsMarquee
        {
            get { return _isMarquee; }
            set
            {
                if (value != null)
                {
                    _isMarquee = value.Value;
                }
            }
        }
        [DisplayName("排序")]
        public Nullable<int> Sort { get; set; }
        [DisplayName("添加时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> AddTime { get; set; }
        [DisplayName("修改时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EditTime { get; set; }
        [DisplayName("是否删除")]
        public Nullable<sbyte> IsDel { get; set; }
    }
}
