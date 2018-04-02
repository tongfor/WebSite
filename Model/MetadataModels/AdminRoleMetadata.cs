/** 
* AdminRoleMetadata.cs
*
* 功 能： AdminRole元数据类
* 类 名： AdminRoleMetadata
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

namespace Models.MetadataModels
{
    public class AdminRoleMetadata
    {
        [DisplayName("ID")]
        public int Id { get; set; }
        [DisplayName("角色名")]
        [Required]
        [StringLength(50,ErrorMessage = "角色名不能超过50个字符")]
        public string RoleName { get; set; }
        [DisplayName("角色描述")]
        [StringLength(100, ErrorMessage = "角色名不能超过100个字符")]
        public string Description { get; set; }
        [DisplayName("添加时间")]
        public Nullable<System.DateTime> AddTime { get; set; }
        [DisplayName("修改时间")]
        public Nullable<System.DateTime> EditTime { get; set; }
    }
}
