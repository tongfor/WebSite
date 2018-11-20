using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{ 
    public partial class AdminMenu
    {
        public AdminMenu()
        {
            AddTime = DateTime.Now;
            EditTime = DateTime.Now;
            Name = string.Empty;
            ParentId = 0;
            Code = string.Empty;
            LinkAddress = string.Empty;
            Icon = string.Empty;
            Sort = 0;
        }
    }
}
