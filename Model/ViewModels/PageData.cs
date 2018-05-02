/** 
* PageData.cs
*
* 功 能： 用于分页查询的数据查询结果，包括数据总数
* 类 名： PageData
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 10:24:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;

namespace Models
{
    /// <summary>
    /// 用于分页查询的数据查询结果，包括数据总数
    /// </summary>
    public class PageData<T>
    {
        /// <summary>
        /// 分页数据集合
        /// </summary>
        public IEnumerable<T> DataList { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
