using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Article
    {
        public int Id { get; set; }
        public int? ClassId { get; set; }
        public string Title { get; set; }
        public string TitleColor { get; set; }
        public string Content { get; set; }
        public string Keyword { get; set; }
        public string Introduce { get; set; }
        public string IntroduceImg { get; set; }
        public string Author { get; set; }
        public string Origin { get; set; }
        public string UserName { get; set; }
        public int? LookCount { get; set; }
        public string AddHtmlurl { get; set; }
        public sbyte? IsTop { get; set; }
        public sbyte? IsMarquee { get; set; }
        public int? Sort { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
        public sbyte? IsDel { get; set; }
    }
}
