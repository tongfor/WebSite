/** 
* AdminMenuAdminButtonDAL.cs
*
* 功 能： 表AdminMenuAdminButton数据层
* 类 名： AdminMenuAdminButton
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 13:11:32   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminMenuAdminButton开始----------

using Models;
using IDAL;

namespace DALMySql
{
	public partial class AdminMenuAdminButtonDAL : BaseDAL<AdminMenuAdminButton>, IAdminMenuAdminButtonDAL
    {
		public AdminMenuAdminButtonDAL(CdyhcdDBContext db) : base(db)
        {
        }
    }
}

//----------AdminMenuAdminButton结束----------

    