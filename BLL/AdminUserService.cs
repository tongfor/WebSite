/** 
* AdminUserService.cs
*
* 功 能： 表AdminUser业务层
* 类 名： AdminUser
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/1/26 2:02:28   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminUser开始----------

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
	public partial class AdminUserService : BaseService<AdminUser>, IAdminUserService 
    {
        //EF上下文
        //protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IAdminUserDAL MyIAdminUserDAL;

        #region 构造函数

		//public AdminUserService(CdyhcdDBContext db, IAdminUserDAL adminUserDAl) : base(adminUserDAl)
		public AdminUserService(IAdminUserDAL adminUserDAl) : base(adminUserDAl)
		{
            //MyDBContext = db;
            MyIAdminUserDAL = adminUserDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminUser GetModelBy(Expression<Func<AdminUser, bool>> queryWhere)
		{
			AdminUser result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminUser> GetModelByAsync(Expression<Func<AdminUser, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminUser result = modelList.FirstOrDefault();
            return result;
        }		

		#endregion 
    }
}

//----------AdminUser结束----------

    