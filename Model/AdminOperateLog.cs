using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminOperateLog
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserIp { get; set; }
        public string OperateInfo { get; set; }
        public string Description { get; set; }
        public sbyte? IsSuccess { get; set; }
        public DateTime? OperateTime { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
