using IDAL;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
   public partial class ParameterService
    {
       protected IParameterDAL IParameterListDAL = new DALSession().IParameterDAL;
    }
}
