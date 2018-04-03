/** 
* Parameter.cs
*
* 功 能： 模型ParameterToPoco
* 类 名： Parameter
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
//----------Parameter Poco开始----------

namespace Models
{
	public partial class Parameter
    {
		public Parameter ToPOCO(bool isPOCO = true)
    		{
    			return new Parameter() 
				{
						Id = this.Id,
						ParName = this.ParName,
						ParExplain = this.ParExplain,
						ParKey = this.ParKey,
						ParValue = this.ParValue,
						ParSequence = this.ParSequence,
						ParParentID = this.ParParentID,
						ParHierarchy = this.ParHierarchy,
						ParPath = this.ParPath,
						ParVersion = this.ParVersion,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
				};
			}
    }
}

//----------Parameter Poco结束----------

    