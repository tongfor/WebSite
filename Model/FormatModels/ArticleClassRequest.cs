/** 
* ArticleRequest.cs
*
* 功 能： 文章传入条件请求
* 类 名： ArticleRequest,ChannelRequest,TagRequest
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ArticleClassRequest : BaseRequest
    {
        /// <summary>
        /// 父类ID
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 排序字段，如Id desc,ParentId asc,其中asc可以省略
        /// </summary>
        public string OrderBy { get; set; }
    }
}
