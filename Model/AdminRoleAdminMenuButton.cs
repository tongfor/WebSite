using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminRoleAdminMenuButton
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public int? MenuId { get; set; }
        public int? ButtonId { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
