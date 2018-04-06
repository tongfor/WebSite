 /** 
* BLLSession.cs
*
* 功 能： 逻辑层工厂
* 类 名： BLLSession
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 19:10:17   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using BLL;
using Models;

namespace YhcdWebSite.Service
{
	public static class BLLSession
	{
        //EF上下文
        public static CdyhcdDBContext Db { get; set; }

        #region 01 逻辑层缓存 IAdminBugService

	    static AdminBugService _adminBugService;
		public static AdminBugService AdminBugService
		{
			get { return _adminBugService ?? (_adminBugService = new AdminBugService(Db)); }
	        set { _adminBugService = value; }			
		}
        #endregion

        #region 02 逻辑层缓存 IAdminButtonService

	    static AdminButtonService _adminButtonService;
		public static AdminButtonService AdminButtonService
		{
			get { return _adminButtonService ?? (_adminButtonService = new AdminButtonService(Db)); }
	        set { _adminButtonService = value; }			
		}
        #endregion

        #region 03 逻辑层缓存 IAdminDepartmentService

	    static AdminDepartmentService _adminDepartmentService;
		public static AdminDepartmentService AdminDepartmentService
		{
			get { return _adminDepartmentService ?? (_adminDepartmentService = new AdminDepartmentService(Db)); }
	        set { _adminDepartmentService = value; }			
		}
        #endregion

        #region 04 逻辑层缓存 IAdminLoginLogService

	    static AdminLoginLogService _adminLoginLogService;
		public static AdminLoginLogService AdminLoginLogService
		{
			get { return _adminLoginLogService ?? (_adminLoginLogService = new AdminLoginLogService(Db)); }
	        set { _adminLoginLogService = value; }			
		}
        #endregion

        #region 05 逻辑层缓存 IAdminMenuService

	    static AdminMenuService _adminMenuService;
		public static AdminMenuService AdminMenuService
		{
			get { return _adminMenuService ?? (_adminMenuService = new AdminMenuService(Db)); }
	        set { _adminMenuService = value; }			
		}
        #endregion

        #region 06 逻辑层缓存 IAdminMenuAdminButtonService

	    static AdminMenuAdminButtonService _adminMenuAdminButtonService;
		public static AdminMenuAdminButtonService AdminMenuAdminButtonService
		{
			get { return _adminMenuAdminButtonService ?? (_adminMenuAdminButtonService = new AdminMenuAdminButtonService(Db)); }
	        set { _adminMenuAdminButtonService = value; }			
		}
        #endregion

        #region 07 逻辑层缓存 IAdminOperateLogService

	    static AdminOperateLogService _adminOperateLogService;
		public static AdminOperateLogService AdminOperateLogService
		{
			get { return _adminOperateLogService ?? (_adminOperateLogService = new AdminOperateLogService(Db)); }
	        set { _adminOperateLogService = value; }			
		}
        #endregion

        #region 08 逻辑层缓存 IAdminRoleService

	    static AdminRoleService _adminRoleService;
		public static AdminRoleService AdminRoleService
		{
			get { return _adminRoleService ?? (_adminRoleService = new AdminRoleService(Db)); }
	        set { _adminRoleService = value; }			
		}
        #endregion

        #region 09 逻辑层缓存 IAdminRoleAdminMenuButtonService

	    static AdminRoleAdminMenuButtonService _adminRoleAdminMenuButtonService;
		public static AdminRoleAdminMenuButtonService AdminRoleAdminMenuButtonService
		{
			get { return _adminRoleAdminMenuButtonService ?? (_adminRoleAdminMenuButtonService = new AdminRoleAdminMenuButtonService(Db)); }
	        set { _adminRoleAdminMenuButtonService = value; }			
		}
        #endregion

        #region 10 逻辑层缓存 IAdminUserService

	    static AdminUserService _adminUserService;
		public static AdminUserService AdminUserService
		{
			get { return _adminUserService ?? (_adminUserService = new AdminUserService(Db)); }
	        set { _adminUserService = value; }			
		}
        #endregion

        #region 11 逻辑层缓存 IAdminUserAdminDepartmentService

	    static AdminUserAdminDepartmentService _adminUserAdminDepartmentService;
		public static AdminUserAdminDepartmentService AdminUserAdminDepartmentService
		{
			get { return _adminUserAdminDepartmentService ?? (_adminUserAdminDepartmentService = new AdminUserAdminDepartmentService(Db)); }
	        set { _adminUserAdminDepartmentService = value; }			
		}
        #endregion

        #region 12 逻辑层缓存 IAdminUserAdminRoleService

	    static AdminUserAdminRoleService _adminUserAdminRoleService;
		public static AdminUserAdminRoleService AdminUserAdminRoleService
		{
			get { return _adminUserAdminRoleService ?? (_adminUserAdminRoleService = new AdminUserAdminRoleService(Db)); }
	        set { _adminUserAdminRoleService = value; }			
		}
        #endregion

        #region 13 逻辑层缓存 IArticleService

	    static ArticleService _articleService;
		public static ArticleService ArticleService
		{
			get { return _articleService ?? (_articleService = new ArticleService(Db)); }
	        set { _articleService = value; }			
		}
        #endregion

        #region 14 逻辑层缓存 IArticleClassService

	    static ArticleClassService _articleClassService;
		public static ArticleClassService ArticleClassService
		{
			get { return _articleClassService ?? (_articleClassService = new ArticleClassService(Db)); }
	        set { _articleClassService = value; }			
		}
        #endregion

        #region 15 逻辑层缓存 IBoardService

	    static BoardService _boardService;
		public static BoardService BoardService
		{
			get { return _boardService ?? (_boardService = new BoardService(Db)); }
	        set { _boardService = value; }			
		}
        #endregion

        #region 16 逻辑层缓存 IParameterService

	    static ParameterService _parameterService;
		public static ParameterService ParameterService
		{
			get { return _parameterService ?? (_parameterService = new ParameterService(Db)); }
	        set { _parameterService = value; }			
		}
        #endregion
    }
}




