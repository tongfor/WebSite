/** 
* ArticleClassService.cs
*
* 功 能： ArticleClassService逻辑层
* 类 名： ArticleClassService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/22 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Text;
using Models;
using System.Linq.Expressions;
using Common;
using System.Threading.Tasks;

namespace BLL
{
    /// <summary>
    /// ArticleClassService逻辑层
    /// </summary>
    public partial class ArticleClassService
    {
        #region 添加文章类别

        /// <summary>
        /// 添加文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int AddArticleClass(ArticleClass model)
        {
            if (model.ParentId == 0)
            {
                model.Tier = 1;
                model.Path = "0";
            }
            else
            {
                ArticleClass parentModel = GetModelBy(f => f.Id == model.ParentId);
                if (parentModel != null)
                {
                    model.Tier = parentModel.Tier + 1;
                    model.Path = parentModel.Path + "," + model.ParentId;
                }
            }
            return IBaseDal.Add(model);
        }

        /// <summary>
        /// 异步添加文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<int> AddArticleClassAsync(ArticleClass model)
        {
            
                if (model.ParentId == 0)
                {
                    model.Tier = 1;
                    model.Path = "0";
                }
                else
                {
                    ArticleClass parentModel = await GetModelBy(f => f.Id == model.ParentId);
                    if (parentModel != null)
                    {
                        model.Tier = parentModel.Tier + 1;
                        model.Path = parentModel.Path + "," + model.ParentId;
                    }
                }
                return IBaseDal.Add(model);
        }

        #endregion 添加文章类别

        #region 修改文章类别

        /// <summary>
        /// 修改文章类别
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public int ModifyArticleClass(ArticleClass model)
        {
            if (model.ParentId == 0)
            {
                model.Tier = 1;
                model.Path = "0";
            }
            else
            {
                ArticleClass parentModel = GetModelBy(f => f.Id == model.ParentId);
                if (parentModel != null)
                {
                    model.Tier = parentModel.Tier + 1;
                    model.Path = parentModel.Path + "," + model.ParentId;
                }
            }
            return IBaseDal.Modify(model);
        }

        #endregion 编辑文章类别

        #region 得到频道清单

        /// <summary>
        /// 得到频道清单,即父ID为0的文章类别
        /// </summary>
        /// <returns></returns>
        public List<ArticleClass> GetChannelList()
        {
            List<ArticleClass> result = ArticleClassDAL.GetOrderListBy(f => f.ParentId == 0, o => o.Sort);
            return result;
        }

        #endregion

        #region 查询所有文章类别树

        /// <summary>
        /// 查询所有文章类别树
        /// </summary>
        public List<ArticleClassTreeView> GetAllArticleClassTree(int classid)
        {
            List<ArticleClassTreeView> resultList = new List<ArticleClassTreeView>();
            List<ArticleClass> modelList = ArticleClassDAL.GetListBy(f => f.ParentId == classid);

            foreach (ArticleClass ac in modelList)
            {
                ArticleClassTreeView treeModel = new ArticleClassTreeView();
                treeModel.id = ac.Id;
                treeModel.text = ac.Name;
                treeModel.chileren = GetAllArticleClassTree(ac.Id);
                resultList.Add(treeModel);
            }

            return resultList;
        }

        #endregion

        #region 查询所有文章类别树并返回JSON

        /// <summary>
        /// 查询所有文章类别树并返回JSON
        /// </summary>
        public string GetAllArticleClassTreeJson(int classid)
        {
            List<ArticleClass> modelList = ArticleClassDAL.GetListBy(f => f.ParentId == classid);
            StringBuilder jsonResult = new StringBuilder();

            jsonResult.Append("[");
            foreach (ArticleClass ac in modelList)
            {
                jsonResult.Append("{\"id\":\"" + ac.Id + "\",\"text\":\"" + ac.Name + "\"");
                List<ArticleClass> cModelList = ArticleClassDAL.GetListBy(f => f.ParentId == ac.Id);
                if (cModelList.Count > 0) //根节点下有子节点
                {
                    jsonResult.Append(",");
                    jsonResult.Append("\"children\":" + GetAllArticleClassTreeJson(ac.Id));
                    jsonResult.Append("},");
                }
                else //根节点下没有子节点
                {
                    jsonResult.Append("},");
                }
            }
            string tmpstr = jsonResult.ToString().TrimEnd(',');//去掉最后多余的逗号
            jsonResult.Clear().Append(tmpstr);
            jsonResult.Append("]");

            return jsonResult.ToString();
        }

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
        public List<ArticleClass> GetArticleClassByPage<TKey>(int pageIndex, int pageSize, Expression<Func<ArticleClass, bool>> queryWhere, Expression<Func<ArticleClass, TKey>> orderBy, out int totalCount, bool isdesc = false)
        {
            return ArticleClassDAL.GetArticleClassByPage<TKey>(pageIndex, pageSize, queryWhere, orderBy, out totalCount, isdesc);
        }

        /// <summary>
        /// 得到分好页的文章类别清单
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<ArticleClass> GetPagedArticleClassList(BaseRequest request)
        {
            List<ArticleClass> actClassList = null;
            int totalCount;
            Expression<Func<ArticleClass, bool>> queryWhere = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                queryWhere = a => a.Name.Contains(request.Title);
            }
            else
            {
                queryWhere = a => true;
            }
            actClassList = GetArticleClassByPage<int>(request.PageIndex, request.PageSize, queryWhere, p => p.Id, out totalCount, false);
            return actClassList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        /// <summary>
        /// 得到分好页的文章类别清单,并返回文章总数
        /// </summary>
        /// <param name="request"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        public List<ArticleClass> GetPagedArticleClassList(ArticleClassRequest request, out int totalCount)
        {
            List<ArticleClass> actClassList = null;
            Expression<Func<ArticleClass, bool>> queryWhere = null;
            if (!string.IsNullOrEmpty(request.Title))
            {
                queryWhere = f => f.Name.Contains(request.Title);
            }
            else
            {
                queryWhere = f => true;
            }
            if (request.ParentId != null)
            {
                queryWhere = queryWhere.AndAlso(f => f.ParentId == request.ParentId);
            }
            if (string.IsNullOrEmpty(request.OrderBy) || !CanOrdered(request.OrderBy))
            {
                request.OrderBy = "Id desc";
            }

            actClassList = GetPageListBy<int>(request.PageIndex, request.PageSize, queryWhere, request.OrderBy, out totalCount);
            return actClassList;
        }
    }
}
