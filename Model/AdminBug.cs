using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminBug
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserIp { get; set; }
        public string BugInfo { get; set; }
        public string BugMessage { get; set; }
        public sbyte? IsShow { get; set; }
        public sbyte? IsSolve { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
