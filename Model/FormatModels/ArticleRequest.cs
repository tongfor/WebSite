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
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ArticleRequest : BaseRequest
    {
        public int ClassId { get; set; }
        public string ClassPath { get; set; }
        public string OrderBy { get; set; }
    }

    public class ChannelRequest : BaseRequest
    {
        public string ArticleClassName { get; set; }
        public int ParentId { get; set; }
        public bool IsActive { get; set; }
    }

    public class TagRequest : BaseRequest
    {
        public Orderby Orderby { get; set; }
    }

    public enum Orderby
    {
        ID = 0,
        Hits = 1
    }
}
