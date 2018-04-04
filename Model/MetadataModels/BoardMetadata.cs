/** 
* BoardMetadata.cs
*
* 功 能： Board
* 类 名： BoardMetadata
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/5/23 17:05:34   李庸    初版
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
    public class BoardMetadata
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("留言标题")]
        [Required]
        public string Title { get; set; }
        [DisplayName("留言内容")]
        [Required]
        public string Content { get; set; }
        [DisplayName("留言人")]
        [Required]
        public string Author { get; set; }
        [DisplayName("IP地址")]
        public string IP { get; set; }
        [DisplayName("QQ号")]
        public string QQ { get; set; }
        [DisplayName("电子邮箱")]
        public string Email { get; set; }
        [DisplayName("个人主页")]
        public string HomePage { get; set; }
        [DisplayName("是否审核通过")]
        public Nullable<sbyte> IsChecked { get; set; }
        [DisplayName("是否删除")]
        public Nullable<sbyte> IsDel { get; set; }
        [DisplayName("添加时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> AddTime { get; set; }
        [DisplayName("修改时间")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EditTime { get; set; }
    }
}
