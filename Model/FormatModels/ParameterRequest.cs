/** 
* ParameterRequest.cs
*
* 功 能： 参数传入条件请求
* 类 名： ParameterRequest
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/11/15 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class ParameterRequest : BaseRequest
    {
        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// 参数路径
        /// </summary>
        public string ParPath { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string OrderBy { get; set; }
    }
}
