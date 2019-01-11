using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public partial class Article
    {
        public Article()
        {
            this.AddTime = DateTime.Now;
            this.EditTime = DateTime.Now;
            this.LookCount = 1;
            this.Introduce = "";
            this.IntroduceImg = "";
            this.IsTop = 0;
            this.IsMarquee = 0;
            this.AddHtmlurl = "";
            this.Gatherurl = "";
            this.TitleColor = "";
            this.Sort = 0;
            this.Author = "";
            this.Origin = "";
            this.IsDel = 0;
        }
    }
}
