using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDAL
{
    public partial interface IAdminRoleDAL
    {
        AdminRole GetRoleByUserId(int UserId);
        
    }
}
