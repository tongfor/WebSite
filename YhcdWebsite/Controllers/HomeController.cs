using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common.AspNetCore.Extensions;
using IBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Models;
using YhcdWebsite.Config;
using YhcdWebsite.Models;

namespace YhcdWebsite.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IArticleService articleService, IArticleClassService articleClassService, IOptions<SiteConfig> options, IHostingEnvironment hostingEnvironment) : base(articleClassService, options)
        {
            _articleService = articleService;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                string picArticleWhere = $"ClassId in ({SiteConfigSettings.IndustryInformationClassIds}) and (aco.IsDel = 0 or aco.IsDel is NULL)";
                string textArticleWhere = $"ClassId in ({SiteConfigSettings.IndustryInformationClassIds}) and (aco.IsDel = 0 or aco.IsDel is NULL) and (IntroduceImg is NULL || IntroduceImg = '')";
                var picArticleList = await _articleService.GetOrderArticleIncludeClassByPageAsync(1, 3, picArticleWhere, "(case when IntroduceImg is not null then 1 else 0 end) desc, AddTime DESC");
                var textArticleList = await _articleService.GetOrderArticleIncludeClassByPageAsync(1, 9, textArticleWhere, "AddTime DESC");

                HomeArticleList articleList = new HomeArticleList
                {
                    PicArticleList = picArticleList.DataList,
                    TextArticleList = textArticleList.DataList
                };
                ViewBag.WebRootPath = _hostingEnvironment.WebRootPath;
                return View(articleList);
            }
            catch(Exception ex)
            {
                _logger.Error($"于{DateTime.Now}在IP:{HttpContext.GetUserIp()}访问首页\"{ex.Message}\"，错误详细信息：{ex.StackTrace.ToString()}.");
                return new LocalRedirectResult("/errors/500");
            }
        }

        public IActionResult About()
        { 
            return View();
        }

        public IActionResult Patent()
        {
            return View();
        }

        public IActionResult Trademark()
        {
            return View();
        }

        public IActionResult Qualification()
        {
            return View();
        }

        public IActionResult Consulting()
        {
            return View();
        }

        public IActionResult Classic()
        {
            return View();
        }

        public IActionResult Website()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
