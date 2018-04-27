/** 
* ArticleClassService.cs
*
* 功 能： 表ArticleClass业务层
* 类 名： ArticleClass
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
//----------ArticleClass开始----------

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
	public partial class ArticleClassService : BaseService<ArticleClass>, IArticleClassService 
    {
        //EF上下文
        protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IArticleClassDAL MyIArticleClassDAL;

        #region 构造函数

		public ArticleClassService(CdyhcdDBContext db, IArticleClassDAL articleClassDAl) : base(articleClassDAl)
		{
            MyDBContext = db;
            MyIArticleClassDAL = articleClassDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public ArticleClass GetModelBy(Expression<Func<ArticleClass, bool>> queryWhere)
		{
			ArticleClass result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<ArticleClass> GetModelByAsync(Expression<Func<ArticleClass, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            ArticleClass result = modelList.FirstOrDefault();
            return result;
        }

		#endregion 
    }
}

//----------ArticleClass结束----------

    