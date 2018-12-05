/** 
* Builder.cs
*
* 功 能： 网站运行设置
* 类 名： Builder
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/27 15:47:24   N/A    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using BLL;
using IBLL;
using IDAL;
using RepositoryPattern;
using Setting.Mvc.Authorize;
using System;

namespace Setting
{
    public static class Builder
    {
        public static void UseYhcdSetting(this IServiceCollection services, IConfiguration configuration)
        {
            #region inject DB Setting

            services.AddDbContext<CdyhcdDBContext>();

            services.Configure<DatabaseOption>(configuration.GetSection("Database"));

            #endregion

            #region inject DAL Setting

            services.TryAddTransient<IAdminBugDAL, DALMySql.AdminBugDAL>();
            services.TryAddTransient<IAdminButtonDAL, DALMySql.AdminButtonDAL>();
            services.TryAddTransient<IAdminDepartmentDAL, DALMySql.AdminDepartmentDAL>();
            services.TryAddTransient<IAdminLoginLogDAL, DALMySql.AdminLoginLogDAL>();
            services.TryAddTransient<IAdminMenuDAL, DALMySql.AdminMenuDAL>();
            services.TryAddTransient<IAdminMenuAdminButtonDAL, DALMySql.AdminMenuAdminButtonDAL>();
            services.TryAddTransient<IAdminOperateLogDAL, DALMySql.AdminOperateLogDAL>();
            services.TryAddTransient<IAdminRoleDAL, DALMySql.AdminRoleDAL>();
            services.TryAddTransient<IAdminRoleAdminMenuButtonDAL, DALMySql.AdminRoleAdminMenuButtonDAL>();
            services.TryAddTransient<IAdminUserDAL, DALMySql.AdminUserDAL>();
            services.TryAddTransient<IAdminUserAdminDepartmentDAL, DALMySql.AdminUserAdminDepartmentDAL>();
            services.TryAddTransient<IAdminUserAdminRoleDAL, DALMySql.AdminUserAdminRoleDAL>();
            services.TryAddTransient<IArticleDAL, DALMySql.ArticleDAL>();
            services.TryAddTransient<IArticleClassDAL, DALMySql.ArticleClassDAL>();
            services.TryAddTransient<IBoardDAL, DALMySql.BoardDAL>();
            services.TryAddTransient<IParameterDAL, DALMySql.ParameterDAL>();

            #endregion

            #region inject BLL Setting

            services.TryAddTransient<IAdminBugService, AdminBugService>();
            services.TryAddTransient<IAdminButtonService, AdminButtonService>();
            services.TryAddTransient<IAdminDepartmentService, AdminDepartmentService>();
            services.TryAddTransient<IAdminLoginLogService, AdminLoginLogService>();
            services.TryAddTransient<IAdminMenuService, AdminMenuService>();
            services.TryAddTransient<IAdminMenuAdminButtonService, AdminMenuAdminButtonService>();
            services.TryAddTransient<IAdminOperateLogService, AdminOperateLogService>();
            services.TryAddTransient<IAdminRoleService, AdminRoleService>();
            services.TryAddTransient<IAdminRoleAdminMenuButtonService, AdminRoleAdminMenuButtonService>();
            services.TryAddTransient<IAdminUserService, AdminUserService>();
            services.TryAddTransient<IAdminUserAdminDepartmentService, AdminUserAdminDepartmentService>();
            services.TryAddTransient<IAdminUserAdminRoleService, AdminUserAdminRoleService>();
            services.TryAddTransient<IArticleService, ArticleService>();
            services.TryAddTransient<IArticleClassService, ArticleClassService>();
            services.TryAddTransient<IBoardService, BoardService>();
            services.TryAddTransient<IParameterService, ParameterService>();

            #endregion
        }

        public static void UseAdminSetting(this IServiceCollection services, IConfiguration configuration)
        {            
            var expiresHours = configuration.GetSection("SiteConfig").GetValue("DefaultLoginExpiresHours", 2.0);
            services.AddAuthentication(DefaultAuthorizeAttribute.DefaultAuthenticationScheme)
            .AddCookie(DefaultAuthorizeAttribute.DefaultAuthenticationScheme, o =>
            {
                o.LoginPath = new PathString("/Account/Login");
                if (expiresHours > 0)
                {
                    o.ExpireTimeSpan = TimeSpan.FromHours(expiresHours);
                }
            });
        }
    }
}




