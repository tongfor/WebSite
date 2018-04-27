/** 
* AdminUserAdminDepartmentService.cs
*
* 功 能： 表AdminUserAdminDepartment业务层
* 类 名： AdminUserAdminDepartment
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/27 15:47:24   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminUserAdminDepartment开始----------

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
	public partial class AdminUserAdminDepartmentService : BaseService<AdminUserAdminDepartment>, IAdminUserAdminDepartmentService 
    {
        //EF上下文
        protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IAdminUserAdminDepartmentDAL MyIAdminUserAdminDepartmentDAL;

        #region 构造函数

		public AdminUserAdminDepartmentService(CdyhcdDBContext db, IAdminUserAdminDepartmentDAL adminUserAdminDepartmentDAl) : base(adminUserAdminDepartmentDAl)
		{
            MyDBContext = db;
            MyIAdminUserAdminDepartmentDAL = adminUserAdminDepartmentDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminUserAdminDepartment GetModelBy(Expression<Func<AdminUserAdminDepartment, bool>> queryWhere)
		{
			AdminUserAdminDepartment result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminUserAdminDepartment> GetModelByAsync(Expression<Func<AdminUserAdminDepartment, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminUserAdminDepartment result = modelList.FirstOrDefault();
            return result;
        }

		#endregion 
    }
}

//----------AdminUserAdminDepartment结束----------

    