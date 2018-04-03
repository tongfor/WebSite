/** 
* AdminUser.cs
*
* 功 能： 模型AdminUserToPoco
* 类 名： AdminUser
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
						QQ = this.QQ,
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

    