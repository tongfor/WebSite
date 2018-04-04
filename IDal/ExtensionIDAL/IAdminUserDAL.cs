using System.Collections.Generic;
using Models;
/** 
* IAdminUserDAL.cs
*
* 功 能： AdminUser数据层接口
* 类 名： IAdminUserDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/17 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Linq.Expressions;
using System;

namespace IDAL
{
    public partial interface IAdminUserDAL
    {
        #region 根据用户名返回模型
        /// <summary>
        /// 根据用户名返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        AdminUser GetModelByUserName(string username);
        #endregion

        #region  根据用户名和密码返回模型
        /// <summary>
        /// 根据用户名和密码返回模型
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        AdminUser GetModelByUserNameAndPwd(string username, string password);
        #endregion

        #region 获取文章关联文章类别的数据（直接执行查询语句）

        /// <summary>
        /// 获取文章关联文章类别的数据（直接执行查询语句）
        /// </summary>
        /// <param name="strWhere">查询条件(article表用aco表示)</param>
        /// <returns></returns>
        List<AdminUserView> GetUserIncludeRole(string strWhere);

        #endregion

        #region 获取用户关联分组的数据（直接执行查询语句）

        /// <summary>
        /// 获取用户关联分组的数据（直接执行查询语句）
        /// </summary>
        /// <param name="groupId">组ID</param>
        /// <param name="strWhere">查询条件(ProjectBase表用pb表示,CreativeActivityProjectShip用ap表示，EntrepreneurshipActivity用ea表示)</param>
        /// <returns></returns>
        List<UserIncludeGroupView> GetUserIncludeGroup(int groupId, string strWhere);

        #endregion

        #region 分页获取用户关联分组的数据并排序（直接执行查询语句）

        /// <summary>
        /// 获取用户关联分组的数据（直接执行查询语句）
        /// </summary>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="groupId">组ID</param>
        /// <param name="strWhere">查询条件(article表用aco表示),必传</param>
        /// <param name="orderBy">排序(article表用aco表示,ArticleClass表用acl表示)</param>
        /// <param name="totalCount">总数</param>
        /// <returns></returns>
        List<UserIncludeGroupView> GetUserIncludeGroupByPage(int pageIndex, int pageSize, int groupId,
            string strWhere,
            string orderBy, out int totalCount);

        #endregion

        bool ExistUserByLoginName(string userName);
        List<AdminUser> GetAdminUserByPage<TKey>(int pageIndex, int pageSize, Expression<Func<AdminUser, bool>> queryWhere, Expression<Func<AdminUser, TKey>> orderBy, out int totalCount, bool isdesc = false);
        
    }
}