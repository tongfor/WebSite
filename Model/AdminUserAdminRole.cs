using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminUserAdminRole
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int? RoleId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
