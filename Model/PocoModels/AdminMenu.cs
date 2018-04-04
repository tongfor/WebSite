/** 
* AdminMenu.cs
*
* 功 能： 模型AdminMenuToPoco
* 类 名： AdminMenu
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
//----------AdminMenu Poco开始----------

namespace Models
{
	public partial class AdminMenu
    {
		public AdminMenu ToPOCO(bool isPOCO = true)
    		{
    			return new AdminMenu() 
				{
						Id = this.Id,
						Name = this.Name,
						ParentId = this.ParentId,
						Code = this.Code,
						LinkAddress = this.LinkAddress,
						Icon = this.Icon,
						Sort = this.Sort,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------AdminMenu Poco结束----------

    