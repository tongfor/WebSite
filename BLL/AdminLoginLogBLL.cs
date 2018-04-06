/** 
* AdminLoginLogBLL.cs
*
* 功 能： 表AdminLoginLog业务层
* 类 名： AdminLoginLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 18:17:26   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminLoginLog开始----------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Models;
using IDAL;
using DALMySql;

namespace BLL
{
	public partial class AdminLoginLogService : BaseService<AdminLoginLog>
    {
        //EF上下文
        protected readonly CdyhcdDBContext _db;

        #region 构造函数

		public AdminLoginLogService(CdyhcdDBContext db)
		{
            _db = db;
			SetIBaseDal();
		}

		#endregion
		
        #region 重写IBaseDal获取方法

		public sealed override void SetIBaseDal()
        {
            IBaseDal = new DALSession(_db).IAdminLoginLogDAL;
        }

		#endregion

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>

		public AdminLoginLog GetModelBy(Expression<Func<AdminLoginLog, bool>> queryWhere)
		{
			AdminLoginLog result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		#endregion 
    }
}

//----------AdminLoginLog结束----------

    