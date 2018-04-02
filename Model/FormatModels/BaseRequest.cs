using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class BaseRequest : Pager
    {
        public BaseRequest()
        {
        }

        /// <summary>
        /// 检索标题
        /// </summary>
        public string Title { get; set; }
    }
}
