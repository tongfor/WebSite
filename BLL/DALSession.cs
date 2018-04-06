/** 
* DALSession.cs
*
* 功 能： 数据工厂
* 类 名： DALSession
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 18:09:29   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using IDAL;
using DALMySql;
using Models;

namespace BLL
{
	public class DALSession
    {
        //EF上下文
        protected readonly CdyhcdDBContext _db;

        public DALSession(CdyhcdDBContext db)
        {
            _db = db;
        }

        #region 01 数据接口 IAdminBugDAL
	    IAdminBugDAL _iAdminBugDAL;
		public IAdminBugDAL IAdminBugDAL
		{
			get { return _iAdminBugDAL ?? (_iAdminBugDAL = new AdminBugDAL(_db)); }
	        set { _iAdminBugDAL = value; }			
		}
        #endregion

        #region 02 数据接口 IAdminButtonDAL
	    IAdminButtonDAL _iAdminButtonDAL;
		public IAdminButtonDAL IAdminButtonDAL
		{
			get { return _iAdminButtonDAL ?? (_iAdminButtonDAL = new AdminButtonDAL(_db)); }
	        set { _iAdminButtonDAL = value; }			
		}
        #endregion

        #region 03 数据接口 IAdminDepartmentDAL
	    IAdminDepartmentDAL _iAdminDepartmentDAL;
		public IAdminDepartmentDAL IAdminDepartmentDAL
		{
			get { return _iAdminDepartmentDAL ?? (_iAdminDepartmentDAL = new AdminDepartmentDAL(_db)); }
	        set { _iAdminDepartmentDAL = value; }			
		}
        #endregion

        #region 04 数据接口 IAdminLoginLogDAL
	    IAdminLoginLogDAL _iAdminLoginLogDAL;
		public IAdminLoginLogDAL IAdminLoginLogDAL
		{
			get { return _iAdminLoginLogDAL ?? (_iAdminLoginLogDAL = new AdminLoginLogDAL(_db)); }
	        set { _iAdminLoginLogDAL = value; }			
		}
        #endregion

        #region 05 数据接口 IAdminMenuDAL
	    IAdminMenuDAL _iAdminMenuDAL;
		public IAdminMenuDAL IAdminMenuDAL
		{
			get { return _iAdminMenuDAL ?? (_iAdminMenuDAL = new AdminMenuDAL(_db)); }
	        set { _iAdminMenuDAL = value; }			
		}
        #endregion

        #region 06 数据接口 IAdminMenuAdminButtonDAL
	    IAdminMenuAdminButtonDAL _iAdminMenuAdminButtonDAL;
		public IAdminMenuAdminButtonDAL IAdminMenuAdminButtonDAL
		{
			get { return _iAdminMenuAdminButtonDAL ?? (_iAdminMenuAdminButtonDAL = new AdminMenuAdminButtonDAL(_db)); }
	        set { _iAdminMenuAdminButtonDAL = value; }			
		}
        #endregion

        #region 07 数据接口 IAdminOperateLogDAL
	    IAdminOperateLogDAL _iAdminOperateLogDAL;
		public IAdminOperateLogDAL IAdminOperateLogDAL
		{
			get { return _iAdminOperateLogDAL ?? (_iAdminOperateLogDAL = new AdminOperateLogDAL(_db)); }
	        set { _iAdminOperateLogDAL = value; }			
		}
        #endregion

        #region 08 数据接口 IAdminRoleDAL
	    IAdminRoleDAL _iAdminRoleDAL;
		public IAdminRoleDAL IAdminRoleDAL
		{
			get { return _iAdminRoleDAL ?? (_iAdminRoleDAL = new AdminRoleDAL(_db)); }
	        set { _iAdminRoleDAL = value; }			
		}
        #endregion

        #region 09 数据接口 IAdminRoleAdminMenuButtonDAL
	    IAdminRoleAdminMenuButtonDAL _iAdminRoleAdminMenuButtonDAL;
		public IAdminRoleAdminMenuButtonDAL IAdminRoleAdminMenuButtonDAL
		{
			get { return _iAdminRoleAdminMenuButtonDAL ?? (_iAdminRoleAdminMenuButtonDAL = new AdminRoleAdminMenuButtonDAL(_db)); }
	        set { _iAdminRoleAdminMenuButtonDAL = value; }			
		}
        #endregion

        #region 10 数据接口 IAdminUserDAL
	    IAdminUserDAL _iAdminUserDAL;
		public IAdminUserDAL IAdminUserDAL
		{
			get { return _iAdminUserDAL ?? (_iAdminUserDAL = new AdminUserDAL(_db)); }
	        set { _iAdminUserDAL = value; }			
		}
        #endregion

        #region 11 数据接口 IAdminUserAdminDepartmentDAL
	    IAdminUserAdminDepartmentDAL _iAdminUserAdminDepartmentDAL;
		public IAdminUserAdminDepartmentDAL IAdminUserAdminDepartmentDAL
		{
			get { return _iAdminUserAdminDepartmentDAL ?? (_iAdminUserAdminDepartmentDAL = new AdminUserAdminDepartmentDAL(_db)); }
	        set { _iAdminUserAdminDepartmentDAL = value; }			
		}
        #endregion

        #region 12 数据接口 IAdminUserAdminRoleDAL
	    IAdminUserAdminRoleDAL _iAdminUserAdminRoleDAL;
		public IAdminUserAdminRoleDAL IAdminUserAdminRoleDAL
		{
			get { return _iAdminUserAdminRoleDAL ?? (_iAdminUserAdminRoleDAL = new AdminUserAdminRoleDAL(_db)); }
	        set { _iAdminUserAdminRoleDAL = value; }			
		}
        #endregion

        #region 13 数据接口 IArticleDAL
	    IArticleDAL _iArticleDAL;
		public IArticleDAL IArticleDAL
		{
			get { return _iArticleDAL ?? (_iArticleDAL = new ArticleDAL(_db)); }
	        set { _iArticleDAL = value; }			
		}
        #endregion

        #region 14 数据接口 IArticleClassDAL
	    IArticleClassDAL _iArticleClassDAL;
		public IArticleClassDAL IArticleClassDAL
		{
			get { return _iArticleClassDAL ?? (_iArticleClassDAL = new ArticleClassDAL(_db)); }
	        set { _iArticleClassDAL = value; }			
		}
        #endregion

        #region 15 数据接口 IBoardDAL
	    IBoardDAL _iBoardDAL;
		public IBoardDAL IBoardDAL
		{
			get { return _iBoardDAL ?? (_iBoardDAL = new BoardDAL(_db)); }
	        set { _iBoardDAL = value; }			
		}
        #endregion

        #region 16 数据接口 IParameterDAL
	    IParameterDAL _iParameterDAL;
		public IParameterDAL IParameterDAL
		{
			get { return _iParameterDAL ?? (_iParameterDAL = new ParameterDAL(_db)); }
	        set { _iParameterDAL = value; }			
		}
        #endregion
    }
}




