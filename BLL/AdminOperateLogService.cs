/** 
* AdminOperateLogService.cs
*
* 功 能： 表AdminOperateLog业务层
* 类 名： AdminOperateLog
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
//----------AdminOperateLog开始----------

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
	public partial class AdminOperateLogService : BaseService<AdminOperateLog>, IAdminOperateLogService 
    {
        //EF上下文
        //protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IAdminOperateLogDAL MyIAdminOperateLogDAL;

        #region 构造函数

		//public AdminOperateLogService(CdyhcdDBContext db, IAdminOperateLogDAL adminOperateLogDAl) : base(adminOperateLogDAl)
		public AdminOperateLogService(IAdminOperateLogDAL adminOperateLogDAl) : base(adminOperateLogDAl)
		{
            //MyDBContext = db;
            MyIAdminOperateLogDAL = adminOperateLogDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public AdminOperateLog GetModelBy(Expression<Func<AdminOperateLog, bool>> queryWhere)
		{
			AdminOperateLog result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<AdminOperateLog> GetModelByAsync(Expression<Func<AdminOperateLog, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            AdminOperateLog result = modelList.FirstOrDefault();
            return result;
        }		

		#endregion 

		public override void Dispose()
        {
            MyIAdminOperateLogDAL = null;
            //MyDBContext.Dispose();
            base.Dispose();
        }
    }
}

//----------AdminOperateLog结束----------

    