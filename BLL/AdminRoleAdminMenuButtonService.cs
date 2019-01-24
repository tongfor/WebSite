/** 
* AdminRoleAdminMenuButtonService.cs
*
* 功 能： 表AdminRoleAdminMenuButton业务层
* 类 名： AdminRoleAdminMenuButton
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/1/24 22:57:12   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminRoleAdminMenuButton开始----------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Models;
using IDAL;
using DALMySql;
using IBLL;
using RepositoryPattern;

namespace BLL
{
	public partial class AdminRoleAdminMenuButtonService : BaseService<AdminRoleAdminMenuButton>, IAdminRoleAdminMenuButtonService 
    {
        //EF上下文
        //protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IAdminRoleAdminMenuButtonDAL MyIAdminRoleAdminMenuButtonDAL;

        #region 构造函数

		//public AdminRoleAdminMenuButtonService(CdyhcdDBContext db, IAdminRoleAdminMenuButtonDAL adminRoleAdminMenuButtonDAl) : base(adminRoleAdminMenuButtonDAl)
		public AdminRoleAdminMenuButtonService(IAdminRoleAdminMenuButtonDAL adminRoleAdminMenuButtonDAl) : base(adminRoleAdminMenuButtonDAl)
		{
            //MyDBContext = db;
            MyIAdminRoleAdminMenuButtonDAL = adminRoleAdminMenuButtonDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminRoleAdminMenuButton GetModelBy(Expression<Func<AdminRoleAdminMenuButton, bool>> queryWhere)
		{
			AdminRoleAdminMenuButton result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminRoleAdminMenuButton> GetModelByAsync(Expression<Func<AdminRoleAdminMenuButton, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminRoleAdminMenuButton result = modelList.FirstOrDefault();
            return result;
        }		

		#endregion 

		public override void Dispose()
        {
            MyIAdminRoleAdminMenuButtonDAL = null;
            //MyDBContext.Dispose();
            base.Dispose();
        }
    }
}

//----------AdminRoleAdminMenuButton结束----------

    