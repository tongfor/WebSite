/** 
* AdminButton.cs
*
* 功 能： 模型AdminButtonToPoco
* 类 名： AdminButton
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
//----------AdminButton Poco开始----------

namespace Models
{
	public partial class AdminButton
    {
		public AdminButton ToPOCO(bool isPOCO = true)
    		{
    			return new AdminButton() 
				{
						Id = this.Id,
						Name = this.Name,
						Code = this.Code,
						Icon = this.Icon,
						Sort = this.Sort,
						Description = this.Description,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------AdminButton Poco结束----------

    