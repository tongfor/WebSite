/** 
* BoardView.cs
*
* 功 能： 用于视图的留言查看模型
* 类 名： BoardView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/5/23 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BoardView : Board
    {
        public BoardView()
        {
            this.ToPOCO();
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("添加时间")]
        public new Nullable<System.DateTime> AddTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("修改时间")]
        public new Nullable<System.DateTime> EditTime { get; set; }

        /// <summary>
        /// 还原父类
        /// </summary>
        /// <returns></returns>
        public Board ToOriginal()
        {
            return new Board
            {
                Id = this.Id,
                Title = this.Title,
                Content = this.Content,
                IsChecked=this.IsChecked,
                IsDel=this.IsDel,
                Author=this.Author,
                Email=this.Email,
                QQ=this.QQ,
                HomePage=this.HomePage,
                AddTime = this.AddTime,
                EditTime = this.EditTime
            };
        }
    }
}
