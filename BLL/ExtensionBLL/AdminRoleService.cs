/** 
* AdminRoleService.cs
*
* 功 能： AdminRole逻辑层
* 类 名： AdminRoleService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Models;

namespace BLL
{
    public partial class AdminRoleService
    { 
        #region 删除数据(包括关联数据)
        /// <summary>
        ///  删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public void DelIncludeRelatedData(int id)
        {
            adminRoleDAL.DelIncludeRelatedData(id);
        }

        /// <summary>
        ///  异步删除数据(包括关联数据)
        /// </summary>
        /// <param name="id">ID</param>
        public async void DelIncludeRelatedDataAsync(int id)
        {
            await Task.Run(() => { adminRoleDAL.DelIncludeRelatedData(id); });
        }

        #endregion

        #region 批量删除数据(包括关联数据)

        /// <summary>
        ///  批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public void DelIncludeRelatedData(List<int> ids)
        {
            adminRoleDAL.DelIncludeRelatedData(ids);
        }

        /// <summary>
        ///  异步批量删除数据(包括关联数据)
        /// </summary>
        /// <param name="ids">ID列表</param>
        public async void DelIncludeRelatedDataAsync(List<int> ids)
        {
            await Task.Run(() => { adminRoleDAL.DelIncludeRelatedDataAsync(ids); });
        }

        #endregion

        #region 根据请求条件获取IPageList格式数据

        /// <summary>
        /// 根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public IEnumerable<AdminRoleView> GetAdminRoleList(RoleRequest request = null)
        {
            request = request ?? new RoleRequest();
            List<AdminRoleView> roleViewList = new List<AdminRoleView>();
            int totalCount = 0;

            var roleList = !string.IsNullOrEmpty(request.RoleName) && Utils.IsSafeSqlString(request.RoleName) ? adminRoleDAL.GetPageListBy(request.PageIndex, request.PageSize, f => f.RoleName.Contains(request.RoleName), o => o.Id, out totalCount, true)
              :
              adminRoleDAL.GetPageListBy(request.PageIndex, request.PageSize, f => true, o => o.Id, out totalCount, true);
            roleViewList = roleList.Select(s => new AdminRoleView(s)).ToList();

            //return roleViewList.OrderByDescending(u => u.Id).ToPagedList(request.PageIndex, request.PageSize, totalCount);
            return roleViewList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        /// <summary>
        /// 异步根据请求条件获取IPageList格式数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<IEnumerable<AdminRoleView>> GetAdminRoleListAsync(RoleRequest request = null)
        {
            request = request ?? new RoleRequest();
            List<AdminRoleView> roleViewList = new List<AdminRoleView>();
            int totalCount = 0;

            var roleList = !string.IsNullOrEmpty(request.RoleName) && Utils.IsSafeSqlString(request.RoleName)
                ? await adminRoleDAL.GetPageListByAsync(request.PageIndex, request.PageSize, f => f.RoleName.Contains(request.RoleName), o => o.Id, true)
                : adminRoleDAL.GetPageListBy(request.PageIndex, request.PageSize, f => true, o => o.Id, out totalCount, true);
            roleViewList = roleList.Select(s => new AdminRoleView(s)).ToList();

            //return roleViewList.OrderByDescending(u => u.Id).ToPagedList(request.PageIndex, request.PageSize, totalCount);
            return roleViewList.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        #endregion

        /// <summary>
        /// 根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public AdminRole GetRoleByUserId(int UserId)
        {
           return  adminRoleDAL.GetRoleByUserId(UserId);
        }

        /// <summary>
        /// 异步根据用户ID查询用户的角色信息
        /// </summary>
        /// <param name="UserId">用户ID</param>
        /// <returns></returns>
        public async  Task<AdminRole> GetRoleByUserIdAsync(int UserId)
        {
            return await adminRoleDAL.GetRoleByUserIdAsync(UserId);
        }
    }
}
