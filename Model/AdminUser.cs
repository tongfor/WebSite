using System;
using System.Collections.Generic;

namespace Models
{
    public partial class AdminUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string UserPwd { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Qq { get; set; }
        public string Email { get; set; }
        public string Postcode { get; set; }
        public int? MemberLevel { get; set; }
        public sbyte? IsFromThird { get; set; }
        public string ThirdUrl { get; set; }
        public string ThirdToken { get; set; }
        public string ThirdType { get; set; }
        public sbyte? IsAble { get; set; }
        public sbyte? IsChangePwd { get; set; }
        public DateTime? AddTime { get; set; }
        public DateTime? EditTime { get; set; }
    }
}
