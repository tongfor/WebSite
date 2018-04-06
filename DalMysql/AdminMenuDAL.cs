/** 
* AdminMenuDAL.cs
*
* 功 能： 表AdminMenu数据层
* 类 名： AdminMenu
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
//----------AdminMenu开始----------

using Models;
using IDAL;

namespace DALMySql
{
	public partial class AdminMenuDAL : BaseDAL<AdminMenu>, IAdminMenuDAL
    {
		public AdminMenuDAL(CdyhcdDBContext db) : base(db)
        {
        }
    }
}

//----------AdminMenu结束----------

    