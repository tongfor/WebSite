/** 
* Board.cs
*
* 功 能： 模型BoardToPoco
* 类 名： Board
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
//----------Board Poco开始----------

namespace Models
{
	public partial class Board
    {
		public Board ToPOCO(bool isPOCO = true)
    		{
    			return new Board() 
				{
						Id = this.Id,
						Title = this.Title,
						Content = this.Content,
						Author = this.Author,
						IP = this.IP,
						QQ = this.QQ,
						Email = this.Email,
						HomePage = this.HomePage,
						IsChecked = this.IsChecked,
						IsDel = this.IsDel,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------Board Poco结束----------

    