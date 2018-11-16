/** 
* ParameterService.cs
*
* 功 能： Parameter逻辑层接口
* 类 名： ParameterService接口
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

using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IBLL
{
    public partial interface IParameterService
    {
        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<IEnumerable<Models.Parameter>> GetParameterListBySqlAsync(ParameterRequest request = null);
    }
}
