/** 
* AdminRole.cs
*
* 功 能： 模型AdminRoleToPoco
* 类 名： AdminRole
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/3 16:31:31   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都成信创通网络信息有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
//----------AdminRole Poco开始----------

namespace Models
{
	public partial class AdminRole
    {
		public AdminRole ToPOCO(bool isPOCO = true)
    		{
    			return new AdminRole() 
				{
						Id = this.Id,
						RoleName = this.RoleName,
						Description = this.Description,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------AdminRole Poco结束----------

    