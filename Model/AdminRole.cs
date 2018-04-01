using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminRole
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
