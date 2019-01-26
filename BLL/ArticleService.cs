/** 
* ArticleService.cs
*
* 功 能： 表Article业务层
* 类 名： Article
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
//----------Article开始----------

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
	public partial class ArticleService : BaseService<Article>, IArticleService 
    {
        //EF上下文
        //protected readonly CdyhcdDBContext MyDBContext;
        //操作DAL
        protected IArticleDAL MyIArticleDAL;

        #region 构造函数

		//public ArticleService(CdyhcdDBContext db, IArticleDAL articleDAl) : base(articleDAl)
		public ArticleService(IArticleDAL articleDAl) : base(articleDAl)
		{
            //MyDBContext = db;
            MyIArticleDAL = articleDAl;
		}

		#endregion        

        #region 根据条件获取模型

		/// <summary>
        /// 根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		public Article GetModelBy(Expression<Func<Article, bool>> queryWhere)
		{
			Article result = this.GetListBy(queryWhere).FirstOrDefault();
			return result;
		}

		/// <summary>
        /// 异步根据条件得到模型
        /// </summary>
        /// <param name="queryWhere">条件Lambda表达式</param>
        /// <returns></returns>
		 public async Task<Article> GetModelByAsync(Expression<Func<Article, bool>> queryWhere)
        {
            var modelList = await this.GetListByAsync(queryWhere);
            Article result = modelList.FirstOrDefault();
            return result;
        }		

		#endregion 
    }
}

//----------Article结束----------

    