using Common.AspNetCore.Extensions;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using YhcdWebsite.Config;
using YhcdWebsite.Models;

namespace YhcdWebsite.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        
        public ArticleController(IArticleService articleService, IArticleClassService articleClassService, IOptions<SiteConfig> options) : base(articleClassService, options)
        {
            _articleService = articleService;
        }

        // GET: Article
        public async Task<IActionResult> List(ArticleRequest request)
        {
            try
            {
                ViewBag.KeyWord = request.Title;
                ViewBag.CurrentPageIndex = request.PageIndex <= 1 ? 1 : request.PageIndex;
                ViewBag.TotalPageCount = 1;

                ViewBag.ClassId = request == null ? 0 : request.ClassId;

                if (request.PageSize <= 0)
                {
                    request.PageSize = SiteConfigSettings.DefaultPageCount;
                }
                IEnumerable<ArticleView> articleList = await _articleService.GetArticleListBySqlAsync(request);
                return View(articleList as PagedList<ArticleView>);
            }
            catch(Exception ex)
            {
                _logger.Error($"于{DateTime.Now}在IP:{HttpContext.GetUserIp()}访问文章列表页报\"{ex.Message}\"，错误详细信息：{ex.StackTrace.ToString()}.");
                return new LocalRedirectResult("/errors/500");
                //return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            try
            {
                var article = await _articleService.GetModelAsync(id.Value);
                if (article == null)
                {
                    return NotFound();
                }
                return View(article);
            }
            catch (Exception ex)
            {
                _logger.Warn($"于{DateTime.Now}在IP:{HttpContext.GetUserIp()}访问文章详细页报\"{ex.Message}\"，错误详细信息：{ex.StackTrace.ToString()}.");
                return new LocalRedirectResult("/errors/500");
            }
        }
        private bool ArticleExists(int id)
        {
            return _articleService.GetModel(id) == null;
        }
    }
}
