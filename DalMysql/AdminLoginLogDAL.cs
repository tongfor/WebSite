/** 
* AdminLoginLogDAL.cs
*
* 功 能： 表AdminLoginLog数据层
* 类 名： AdminLoginLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/1/24 23:24:07   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminLoginLog开始----------

using Models;
using IDAL;
using RepositoryPattern;

namespace DALMySql
{
	public partial class AdminLoginLogDAL : BaseDAL<AdminLoginLog>, IAdminLoginLogDAL
    {
		public AdminLoginLogDAL(CdyhcdDBContext db) : base(db)
        {
        }
    }
}

//----------AdminLoginLog结束----------

    