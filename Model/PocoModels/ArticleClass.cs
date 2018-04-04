/** 
* ArticleClass.cs
*
* 功 能： 模型ArticleClassToPoco
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
//----------ArticleClass Poco开始----------

namespace Models
{
	public partial class ArticleClass
    {
		public ArticleClass ToPOCO(bool isPOCO = true)
    		{
    			return new ArticleClass() 
				{
						Id = this.Id,
						ParentId = this.ParentId,
						Name = this.Name,
						Tier = this.Tier,
						Path = this.Path,
						IsActive = this.IsActive,
						Sort = this.Sort,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
						IsDel = this.IsDel,
				};
			}
    }
}

//----------ArticleClass Poco结束----------

    