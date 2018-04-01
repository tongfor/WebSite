using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminUserAdminDepartment
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? DepartmentId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
