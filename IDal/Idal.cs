

/** 
* IDal.cs
*
* 功 能： 数据层接口
* 类 名： IDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/6 11:50:48   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Models;

namespace IDAL
{

	public partial interface IAdminBugDAL : IBaseDAL<AdminBug>
    {		
    }

	public partial interface IAdminButtonDAL : IBaseDAL<AdminButton>
    {		
    }

	public partial interface IAdminDepartmentDAL : IBaseDAL<AdminDepartment>
    {		
    }

	public partial interface IAdminLoginLogDAL : IBaseDAL<AdminLoginLog>
    {		
    }

	public partial interface IAdminMenuDAL : IBaseDAL<AdminMenu>
    {		
    }

	public partial interface IAdminMenuAdminButtonDAL : IBaseDAL<AdminMenuAdminButton>
    {		
    }

	public partial interface IAdminOperateLogDAL : IBaseDAL<AdminOperateLog>
    {		
    }

	public partial interface IAdminRoleDAL : IBaseDAL<AdminRole>
    {		
    }

	public partial interface IAdminRoleAdminMenuButtonDAL : IBaseDAL<AdminRoleAdminMenuButton>
    {		
    }

	public partial interface IAdminUserDAL : IBaseDAL<AdminUser>
    {		
    }

	public partial interface IAdminUserAdminDepartmentDAL : IBaseDAL<AdminUserAdminDepartment>
    {		
    }

	public partial interface IAdminUserAdminRoleDAL : IBaseDAL<AdminUserAdminRole>
    {		
    }

	public partial interface IArticleDAL : IBaseDAL<Article>
    {		
    }

	public partial interface IArticleClassDAL : IBaseDAL<ArticleClass>
    {		
    }

	public partial interface IBoardDAL : IBaseDAL<Board>
    {		
    }

	public partial interface IParameterDAL : IBaseDAL<Parameter>
    {		
    }
}




