/** 
* IArticleClassService.cs
*
* 功 能： IArticleClassService逻辑层接口
* 类 名： IArticleClassService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using Models;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace IBLL
{
    /// <summary>
    /// ArticleClassService逻辑层
    /// </summary>
    partial interface IArticleClassService
    {
        #region 添加文章类别

        /// <summary>
        /// 添加文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int AddArticleClass(ArticleClass model);

        /// <summary>
        /// 异步添加文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> AddArticleClassAsync(ArticleClass model);

        #endregion 添加文章类别

        #region 修改文章类别

        /// <summary>
        /// 修改文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int ModifyArticleClass(ArticleClass model);

        /// <summary>
        /// 异步修改文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<int> ModifyArticleClassAsync(ArticleClass model);

        #endregion 编辑文章类别

        #region 得到频道清单

        /// <summary>
        /// 得到频道清单,即父ID为0的文章类别
        /// </summary>
        /// <returns></returns>
        List<ArticleClass> GetChannelList();

        /// <summary>
        /// 异步得到频道清单,即父ID为0的文章类别
        /// </summary>
        /// <returns></returns>
        Task<List<ArticleClass>> GetChannelListAsync();

        #endregion

        #region 查询所有文章类别树

        /// <summary>
        /// 查询所有文章类别树
        /// </summary>
        List<ArticleClassTreeView> GetAllArticleClassTree(int? classid);

        /// <summary>
        /// 异步查询所有文章类别树
        /// </summary>
        Task<List<ArticleClassTreeView>> GetAllArticleClassTreeAsync(int classid);

        #endregion

        #region 查询所有文章类别树并返回JSON

        /// <summary>
        /// 查询所有文章类别树并返回JSON
        /// </summary>
        string GetAllArticleClassTreeJson(int classid);

        /// <summary>
        /// 异步查询所有文章类别树并返回JSON
        /// </summary>
        Task<string> GetAllArticleClassTreeJsonAsync(int classid);

        /// <summary>
        /// 异步查询所有文章类别树并返回JSON（zTree使用）
        /// </summary>
        Task<string> GetAllArticleClassTreeJsonForzTreeAsync(int classid);

        #endregion

        /// <summary>
        /// 分页查询文章类别
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns></returns>
        List<ArticleClass> GetArticleClassByPage<TKey>(int pageIndex, int pageSize, Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, out int totalCount, bool isdesc = false);

        /// <summary>
        /// 异步分页查询文章类别
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="queryWhere"></param>
        /// <param name="orderBy"></param>
        /// <param name="totalCount"></param>
        /// <param name="isdesc"></param>
        /// <returns>PageData类型数据，包含DataList和TotalCount</returns>
        Task<PageData<ArticleClass>> GetArticleClassByPageAsync<TKey>(int pageIndex, int pageSize, Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, bool isdesc = false);

        /// <summary>
        /// 得到分好页的文章类别清单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        IEnumerable<ArticleClass> GetPagedArticleClassList(BaseRequest request);

        /// <summary>
        /// 异步得到分好页的文章类别清单
        /// </summary>
        /// <param name="request"></param>
        /// <returns>PageData类型数据，包含DataList和TotalCount</returns>
        Task<IEnumerable<ArticleClass>> GetPagedArticleClassListAsync(BaseRequest request);

        /// <summary>
        /// 得到分好页的文章类别清单,并返回文章总数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        List<ArticleClass> GetPagedArticleClassList(ArticleClassRequest request, out int totalCount);

        /// <summary>
        /// 异步得到分好页的文章类别清单,并返回文章总数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns>PageData类型数据，包含DataList和TotalCount</returns>
        Task<PageData<ArticleClass>> GetPagedArticleClassListAsync(ArticleClassRequest request);
    }
}
