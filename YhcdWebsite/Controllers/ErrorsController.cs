using IBLL;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using YhcdWebsite.Config;

namespace YhcdWebsite.Controllers
{
    public class ErrorsController : BaseController
    {
        private IHostingEnvironment _env;
        public ErrorsController(IArticleClassService articleClassService, IOptions<SiteConfig> options, IHostingEnvironment hostingEnvironment) : base(articleClassService, options)
        {
            _env = hostingEnvironment;
        }

        //public IActionResult Show(int? statusCode)
        //{
        //    //if (statusCode == 404)
        //    //{
        //    //    return View("~/Views/Errors/404.cshtml");
        //    //}
        //    //return View("~/Views/Errors/505.cshtml");
        //    return View();
        //}

        [Route("errors/{statusCode}")]
        public IActionResult CustomError(int statusCode)
        {
            var filePath = $"{_env.WebRootPath}/errors/{(statusCode == 404 ? 404 : 500)}.html";
            return new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
        }
    }
}