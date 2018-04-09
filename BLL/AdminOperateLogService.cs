/** 
* AdminOperateLogService.cs
*
* 功 能： 表AdminOperateLog业务层
* 类 名： AdminOperateLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/9 23:03:57   N/A    初版
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

namespace BLL
{
	public partial class AdminOperateLogService : BaseService<AdminOperateLog>
    {
        //EF上下文
        protected readonly CdyhcdDBContext _db;
        //操作DAL
        protected IAdminOperateLogDAL adminOperateLogDAL;

        #region 构造函数

		public AdminOperateLogService(CdyhcdDBContext db)
		{
            _db = db;
			SetIBaseDal();
		}

		#endregion
		
        #region 重写IBaseDal获取方法

		public sealed override void SetIBaseDal()
        {
            IBaseDal = adminOperateLogDAL = new DALSession(_db).IAdminOperateLogDAL;
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
    }
}

//----------AdminOperateLog结束----------

    