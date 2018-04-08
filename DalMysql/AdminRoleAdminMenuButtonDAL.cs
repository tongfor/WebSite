/** 
* AdminRoleAdminMenuButtonDAL.cs
*
* 功 能： 表AdminRoleAdminMenuButton数据层
* 类 名： AdminRoleAdminMenuButton
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/8 21:13:05   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminRoleAdminMenuButton开始----------

using Models;
using IDAL;

namespace DALMySql
{
	public partial class AdminRoleAdminMenuButtonDAL : BaseDAL<AdminRoleAdminMenuButton>, IAdminRoleAdminMenuButtonDAL
    {
		public AdminRoleAdminMenuButtonDAL(CdyhcdDBContext db) : base(db)
        {
        }
    }
}

//----------AdminRoleAdminMenuButton结束----------

    