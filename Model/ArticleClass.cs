using System;
using System.Collections.Generic;

namespace Models
{
    public partial class ArticleClass
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public int? Tier { get; set; }
        public string Path { get; set; }
        public sbyte? IsActive { get; set; }
        public int? Sort { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
        public sbyte? IsDel { get; set; }
    }
}
