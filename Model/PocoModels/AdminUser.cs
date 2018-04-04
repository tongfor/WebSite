/** 
* AdminUser.cs
*
* 功 能： 模型AdminUserToPoco
* 类 名： AdminUser
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
//----------AdminUser Poco开始----------

namespace Models
{
	public partial class AdminUser
    {
		public AdminUser ToPOCO(bool isPOCO = true)
    		{
    			return new AdminUser() 
				{
						Id = this.Id,
						UserName = this.UserName,
						UserPwd = this.UserPwd,
						Name = this.Name,
						Mobile = this.Mobile,
						Qq = this.Qq,
						Email = this.Email,
						Postcode = this.Postcode,
						MemberLevel = this.MemberLevel,
						IsFromThird = this.IsFromThird,
						ThirdUrl = this.ThirdUrl,
						ThirdToken = this.ThirdToken,
						ThirdType = this.ThirdType,
						IsAble = this.IsAble,
						IsChangePwd = this.IsChangePwd,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------AdminUser Poco结束----------

    