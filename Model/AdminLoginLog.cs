using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminLoginLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserIp { get; set; }
        public string City { get; set; }
        public sbyte? IsSuccess { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
