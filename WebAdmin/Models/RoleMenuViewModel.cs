using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdmin.Models
{
    /// <summary>
    /// 角色菜单类
    /// </summary>
    public class RoleMenuViewModel
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleId { get; set; }
        /// <summary>
        /// 角色已选可访问菜单ID集，以","分隔
        /// </summary>
        public string CheckedMenuIds { get; set; }
    }
}
