/** 
* AdminUser.cs
*
* 功 能： AdminUser元数据与 AdminRole关联
* 类 名： AdminUser
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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Models
{
    public partial class AdminUser
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public void InitAdminUser()
        {
            this.AddTime = DateTime.Now;
            this.IsAble = 1;
            this.IsChangePwd = 0;
        }
    }
}
