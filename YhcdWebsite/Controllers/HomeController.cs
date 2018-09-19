using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using YhcdWebsite.Config;
using YhcdWebsite.Models;

namespace YhcdWebsite.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IArticleClassService articleClassService, IOptions<SiteConfig> options) : base(articleClassService, options)
        {
        }

        public IActionResult Index()
        {
            return View();
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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
