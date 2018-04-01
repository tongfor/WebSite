using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminDepartment
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public int? ParentId { get; set; }
        public int? Tier { get; set; }
        public string Path { get; set; }
        public int? Sort { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
