using System;
using System.Collections.Generic;

namespace Models
{
    public partial class Parameter
    {
        public int Id { get; set; }
        public string ParName { get; set; }
        public string ParExplain { get; set; }
        public string ParKey { get; set; }
        public string ParValue { get; set; }
        public int? ParSequence { get; set; }
        public int? ParParentId { get; set; }
        public int? ParHierarchy { get; set; }
        public string ParPath { get; set; }
        public int? ParVersion { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
