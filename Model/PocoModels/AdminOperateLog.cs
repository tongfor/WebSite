/** 
* AdminOperateLog.cs
*
* 功 能： 模型AdminOperateLogToPoco
* 类 名： AdminOperateLog
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
//----------AdminOperateLog Poco开始----------

namespace Models
{
	public partial class AdminOperateLog
    {
		public AdminOperateLog ToPOCO(bool isPOCO = true)
    		{
    			return new AdminOperateLog() 
				{
						Id = this.Id,
						UserName = this.UserName,
						UserIp = this.UserIp,
						OperateInfo = this.OperateInfo,
						Description = this.Description,
						IsSuccess = this.IsSuccess,
						OperateTime = this.OperateTime,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------AdminOperateLog Poco结束----------

    