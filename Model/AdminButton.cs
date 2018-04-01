using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminButton
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Icon { get; set; }
        public int? Sort { get; set; }
        public string Description { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
