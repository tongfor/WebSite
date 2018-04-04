/** 
* Article.cs
*
* 功 能： 模型ArticleToPoco
* 类 名： Article
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
//----------Article Poco开始----------

namespace Models
{
	public partial class Article
    {
		public Article ToPOCO(bool isPOCO = true)
    		{
    			return new Article() 
				{
						Id = this.Id,
						ClassId = this.ClassId,
						Title = this.Title,
						TitleColor = this.TitleColor,
						Content = this.Content,
						Introduce = this.Introduce,
						IntroduceImg = this.IntroduceImg,
						Author = this.Author,
						Origin = this.Origin,
						UserName = this.UserName,
						LookCount = this.LookCount,
						AddHtmlurl = this.AddHtmlurl,
						IsTop = this.IsTop,
						IsMarquee = this.IsMarquee,
						Sort = this.Sort,
						AddTime = this.AddTime,
						EditTime = this.EditTime,
						IsDel = this.IsDel,
				};
			}
    }
}

//----------Article Poco结束----------

    