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
            var bll = BLLSession.ArticleService;
            BLLSession.Db = db;
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

            var a1 = bll.GetModelAsync(1);            
            int totalCount;
            var a4 = bll.GetOrderArticleIncludeClassByPage(1, 3, "", "", out totalCount);
            var a2 = bll.GetOrderArticleIncludeClassByPageAsync(1, 3, "", "");
            var a3 = bll.GetPageDataAsync(1, 3, f => true, o => o.Id);

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
