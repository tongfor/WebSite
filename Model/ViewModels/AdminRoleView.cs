/** 
* AdminRoleView.cs
*
* 功 能： AdminRole用于展示的模型
* 类 名： AdminRoleView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/25 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
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
    public class AdminRoleView : AdminRole
    {
        public AdminRoleView()
        {
        }

        public AdminRoleView(AdminRole role)
        {
            this.Id = role.Id;
            this.RoleName = role.RoleName;
            this.Description = role.Description;
            this.AddTime = role.AddTime;
            this.EditTime = role.EditTime;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("修改时间")]
        public new Nullable<System.DateTime> AddTime { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("修改时间")]
        public new Nullable<System.DateTime> EditTime { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        [DisplayName("用户ID")]
        public string UserId { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        [DisplayName("菜单ID")]
        public string MenuId { get; set; }

        /// <summary>
        /// 按钮ID
        /// </summary>
        [DisplayName("按钮ID")]
        public string ButtonId { get; set; }
    }
}
