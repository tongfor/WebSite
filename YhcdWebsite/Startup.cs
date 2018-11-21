using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryPattern;
using Setting;
using System.IO;
using NLog.Extensions.Logging;
using NLog.Web;
using YhcdWebsite.Config;
using Microsoft.Extensions.Logging;
using NLog;

namespace YhcdWebsite
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SiteConfig>(Configuration.GetSection("SiteConfig"));

            services.AddScoped<IOnDatabaseConfiguring, EntityFrameWorkConfigure>();

            services.UseYhcdSetting(Configuration);

            services.AddMvc().AddJsonOptions(option => { option.SerializerSettings.DateFormatString = "yyyy-MM-dd"; }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                //app.UseExceptionHandler("/Home/Error");
                app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
                  {
                      context.Response.StatusCode = 500;
                      if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                      {
                          context.Response.ContentType = "text/html";
                          await context.Response.SendFileAsync($@"{env.WebRootPath}/errors/500.html");
                      }
                  }));
                app.UseStatusCodePagesWithReExecute("/errors/{0}");
            }

            loggerFactory.AddNLog(); //添加NLog
            LogManager.LoadConfiguration("nlog.config");

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                //ID参数在action后静态路由
                routes.MapRoute(
                   name: "-id.html",
                   template: "{controller=Home}/{action=Index}-{id}.html/{*others}");
                ////ClassID参数在action后静态路由
                //routes.MapRoute(
                //   name: "-classid.html",
                //   template: "{controller=Home}/{action=Index}-{classid}.html/{*others}");
                //静态路由
                routes.MapRoute(
                   name: "html",
                   template: "{controller=Home}/{action=Index}.html/{id?}");
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
