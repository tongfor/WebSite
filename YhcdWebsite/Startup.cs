using BLL;
using IBLL;
using IDAL;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RepositoryPattern;
using Setting;
using YhcdWebsite.Config;

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
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
