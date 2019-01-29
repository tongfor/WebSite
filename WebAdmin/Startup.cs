using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Extensions.Logging;
using RepositoryPattern;
using Setting;

namespace WebAdmin
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
            services.Configure<GatherConfig>(Configuration.GetSection("GatherConfig"));

            services.Configure<FormOptions>(x => {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue;
            });

            services.AddScoped<IOnDatabaseConfiguring, EntityFrameWorkConfigure>();

            services.UseYhcdSetting(Configuration);
            services.UseAdminSetting(Configuration);

            services.AddSession();
            //services.AddMvc(options=>options.Filters.Add<Filter.HttpGlobalExceptionFilter>())
            services.AddMvc()
              .AddJsonOptions(option => { option.SerializerSettings.DateFormatString = "yyyy-MM-dd"; });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            loggerFactory.AddNLog(); //添加NLog
            LogManager.LoadConfiguration("nlog.config");
            var logger = LogManager.GetCurrentClassLogger();

            app.UseExceptionHandler(errorApp => errorApp.Run(async context =>
            {
                var feature = context.Features.Get<IExceptionHandlerFeature>();
                var error = feature?.Error;
                logger.Error(error, error.Message, error.StackTrace);
                context.Response.StatusCode = 500;

                if (context.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
                {
                    context.Response.ContentType = "text/html";
                    await context.Response.SendFileAsync($@"{env.WebRootPath}/errors/500.html");
                }
            }));

            app.UseStaticFiles();
            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");
            });

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);//注册编码提供程序
        }
    }
}
