/** 
* ApiResult.cs
*
* 功 能： 接口请求结果集合
* 类 名： ApiResult
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/5/25 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;

namespace Models.FormatModels
{
    /// <summary>
    /// 接口请求结果
    /// </summary>
    public class ApiResultList<T> : Pager
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 操作结果描述
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 返回结果集
        /// </summary>
        public List<T> ItemList { get; set; }
    }
}
