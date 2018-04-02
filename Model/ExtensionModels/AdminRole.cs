/** 
* AdminRole.cs
*
* 功 能： AdminRole元数据与 AdminRole关联
* 类 名： AdminRole
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
using System.ComponentModel.DataAnnotations;
using Models.MetadataModels;

namespace Models
{
    [MetadataType(typeof(AdminRoleMetadata))]
    public partial class AdminRole
    {
        public AdminRole()
        {
            this.AddTime = DateTime.Now;
            this.EditTime = DateTime.Now;
        }
    }
}
