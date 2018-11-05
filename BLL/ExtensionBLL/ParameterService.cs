/** 
* ParameterService.cs
*
* 功 能： Parameter逻辑层
* 类 名： ParameterService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/11/5 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Common;
using Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLL
{
    public partial class ParameterService
    {
        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Parameter>> GetParameterListBySqlAsync(ParameterRequest request = null)
        {
            request = request ?? new ParameterRequest();
            List<Parameter> result = new List<Parameter>();
            Expression<Func<Parameter, bool>> queryWhere = null;
            string orderby = string.IsNullOrEmpty(request.OrderBy) ? " AddTime desc" : request.OrderBy;

            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                queryWhere = f => f.ParName.Contains(request.Title);
            }

            if (request.ParentId > 0)
            {
                if (!string.IsNullOrEmpty(request.ParPath) && Utils.IsSafeSqlString(request.ParPath))
                {
                    queryWhere.AndAlso(f => f.ParPath.StartsWith(request.ParPath));
                }
                else
                {
                    queryWhere.AndAlso(f => f.ParParentId == request.ParentId);
                }
            }

            var parameters = await GetPageListByAsync(request.PageIndex, request.PageSize, queryWhere, orderby);

            return parameters.DataList.ToPagedList(request.PageIndex, request.PageSize, parameters.TotalCount);
        }
    }
}
