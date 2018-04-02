/** 
* AdminUserMetadata.cs
*
* 功 能： AdminUser元数据类
* 类 名： AdminUserMetadata
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
using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// AdminUser元数据类
    /// </summary>
    public partial class AdminUserMetadata
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string QQ { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public Nullable<int> MemberLevel { get; set; }
        public Nullable<sbyte> IsFromThird { get; set; }
        public string ThirdUrl { get; set; }
        public string ThirdToken { get; set; }
        public string ThirdType { get; set; }
        public Nullable<sbyte> IsAble { get; set; }
        public Nullable<sbyte> IsChangePwd { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public Nullable<System.DateTime> EditTime { get; set; }
    }
}
