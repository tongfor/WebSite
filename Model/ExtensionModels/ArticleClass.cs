/** 
* ArticleClass.cs
*
* 功 能： 模型ArticleClass
* 类 名： ArticleClass
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/4 11:41:36   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------ArticleClass开始----------
using System;

namespace Models
{
    public partial class ArticleClass
    {
        public ArticleClass()
        {
            AddTime = DateTime.Now;
            EditTime = DateTime.Now;
        }
    }
}

//----------ArticleClass Poco结束----------

    