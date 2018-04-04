/** 
* ArticlesClassicView.cs
*
* 功 能： 所有用户前台展示的文章类别模型
* 类 名： ArticlesClassicView
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/25 17:05:34   李庸    初版
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
    //所有用户前台展示的文章类别模型
    /// <summary>
    /// 
    /// </summary>
    public class ArticlesClassicView
    {
    }

    /// <summary>
    /// 用于EASYUI TREE的模型
    /// </summary>
    public class ArticleClassTreeView
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 打开关闭状态
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 是否选择
        /// </summary>
        public bool @checked{get;set;}
        /// <summary>
        /// 子类
        /// </summary>
        public List<ArticleClassTreeView> chileren { get; set; }
        /// <summary>
        /// 自定义属性
        /// </summary>
        public CustomAttributes attributes { get; set; }
    }

    /// <summary>
    /// 用于EASYUI TREE的模型的自定义属性
    /// </summary>
    public class CustomAttributes
    {

    }
}
