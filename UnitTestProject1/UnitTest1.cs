using DALMySql;
using Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
using BLL;
using YhcdWebSite.Service;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var db = new DbContextFactory().GetDbContext();
            //var bll = new ArticleService(db);
            //var bll = new AdminUserService(db);
            var bll = new AdminMenuService(db);

            //var dal = new ArticleDAL(db);
            //var menuDal = new AdminMenuDAL(db);
            //var userDal = new AdminUserDAL(db);
            //var roleDal = new AdminRoleDAL(db);

            //var cc = db.Set<Article>().Find(1);
            //var aa = db.Article.Join(db.ArticleClass, a => a.ClassId, ac => ac.Id, (a, ac) => new ArticleView() { }).ToList();
            //var model = dal.GetArticleById("1");
            //var articles = dal.GetArticleIncludeClass(" classid = 3;insert into Board (title,Author) values('dfsdf','34234');");
            //var userMenus = menuDal.GetAdminUserMenu(1);
            //var um2 = menuDal.GetMenuListIncludeRoleAndButton(0, 1);

            //var r1 = roleDal.GetRoleByUserId(1);
            //var a1 = dal.GetPageListByAsync(1, 3, f => f.ClassId == 2, " LookCount desc, AddTime desc");

            //var b1 = db.Set<PageData<Article>>().FromSql("select * from Article as DataList;Select count(1) as TotalCount from Article as TotalCount;");

            //var a1 = bll.GetModelAsync(1);            
            //int totalCount;
            //var a4 = bll.GetOrderArticleIncludeClassByPage(1, 3, "", "", out totalCount);
            //var a2 = bll.GetOrderArticleIncludeClassByPageAsync(1, 3, "", "");
            //var a3 = bll.GetPageDataAsync(1, 3, f => true, o => o.Id);
            //var a5 = bll.GetArticleIncludeClassAsync("");
            //var a6 = bll.GetArticleViewNearIdAsync(2);
            //var a7 = bll.GetOrderArticleIncludeClassByPageAsync(1, 3, "", "Id desc");
            //var a8 = bll.GetPageListByAsync(1, 3, f => true, "");

            //var au1 = bll.GetPageDataAsync(1, 4, f => true, o => o.Id);
            //var au2 = bll.GetAdminUserByPageAsync(1, 4, f => true, o => o.Id);
            //var au3 = bll.UserCanRegisterAsync("admin");
            //UserRequest userRequest = new UserRequest()
            //{
            //    UserName = "admin",
            //    RoleId = 1
            //};
            //var au4 = bll.GetUserByRequestAsync(null);

            //var d1 = bll.DelIncludeRelatedDataAsync(3);
            var menuList = bll.GetAdminUserMenu(1);
            var am1 = bll.GetAdminUserMenuTreeAsync(menuList, 0);
            var am2 = bll.GetMenuTreeJsonByRoleIdAsync(1, 1);
            var am3 = bll.GetAllMenuTreeJsonAsync(0);
            var am4 = bll.GetAdminMenuListAsync(null);
            var am5 = bll.GetAllMenuOrderListAsync();
            var am6 = bll.DelIncludeRelatedDataAsync(3);
            var am7 = bll.DelIncludeRelatedDataAsync(new List<int>() { 1, 3 });

            List<int> l1 = new List<int>() { 1, 2 ,3};
            string sl1 = l1.ToString();

            //var u1 = userDal.GetModelByUserName("admin");
            //var u2 = userDal.GetModelByUserNameAndPwd("super", "9CE853EB7EE8E362E1D121EB4DF2DC91");

            //var u4 = userDal.UserExistedByLoginName("admin");
            //var u3 = userDal.GetUserIncludeRole("");
            //int total = 0;
            //var u5 = userDal.GetAdminUserByPage(1, 2, f => true, o => o.AddTime, out total);

            var aa1 = 1;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var configuration = builder.Build();

            services.AddDbContext<CdyhcdDBContext>(options => options.UseMySQL(configuration.GetConnectionString("YhcdConnection")));
        }
    }
}
