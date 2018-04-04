

/** 
* IDal.cs
*
* 功 能： 数据层接口
* 类 名： IDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/4 11:52:44   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Models;

namespace IDAL
{

	public partial interface IAdminBugDal : IBaseDAL<AdminBug>
    {		
    }

	public partial interface IAdminButtonDal : IBaseDAL<AdminButton>
    {		
    }

	public partial interface IAdminDepartmentDal : IBaseDAL<AdminDepartment>
    {		
    }

	public partial interface IAdminLoginLogDal : IBaseDAL<AdminLoginLog>
    {		
    }

	public partial interface IAdminMenuDal : IBaseDAL<AdminMenu>
    {		
    }

	public partial interface IAdminMenuAdminButtonDal : IBaseDAL<AdminMenuAdminButton>
    {		
    }

	public partial interface IAdminOperateLogDal : IBaseDAL<AdminOperateLog>
    {		
    }

	public partial interface IAdminRoleDal : IBaseDAL<AdminRole>
    {		
    }

	public partial interface IAdminRoleAdminMenuButtonDal : IBaseDAL<AdminRoleAdminMenuButton>
    {		
    }

	public partial interface IAdminUserDal : IBaseDAL<AdminUser>
    {		
    }

	public partial interface IAdminUserAdminDepartmentDal : IBaseDAL<AdminUserAdminDepartment>
    {		
    }

	public partial interface IAdminUserAdminRoleDal : IBaseDAL<AdminUserAdminRole>
    {		
    }

	public partial interface IArticleDal : IBaseDAL<Article>
    {		
    }

	public partial interface IArticleClassDal : IBaseDAL<ArticleClass>
    {		
    }

	public partial interface IBoardDal : IBaseDAL<Board>
    {		
    }

	public partial interface IParameterDal : IBaseDAL<Parameter>
    {		
    }
}




