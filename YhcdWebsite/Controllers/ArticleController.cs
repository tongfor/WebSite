using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using BLL;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using RepositoryPattern;
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
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }            
        }

        // GET: Article/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article =await _articleService.GetModelAsync(id.Value);            
            var a2 = _articleService.GetOrderArticleIncludeClassByPageAsync(1, 3, "", "");
            var a3 = _articleService.GetPageDataAsync(1, 3, f => true, o => o.Id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClassId,Title,TitleColor,Content,Introduce,IntroduceImg,Author,Origin,UserName,LookCount,AddHtmlurl,IsTop,IsMarquee,Sort,AddTime,EditTime,IsDel")] Article article)
        {
            if (ModelState.IsValid)
            {
                await _articleService.AddAsync(article);
                return RedirectToAction(nameof(List));
            }
            return View(article);
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var article = await _articleService.GetModelAsync(id.Value);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClassId,Title,TitleColor,Content,Introduce,IntroduceImg,Author,Origin,UserName,LookCount,AddHtmlurl,IsTop,IsMarquee,Sort,AddTime,EditTime,IsDel")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //_context.Update(article);
                    //await _context.SaveChangesAsync();
                    await _articleService.ModifyAsync(article);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(List));
            }
            return View(article);
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var article = await _context.Article
            //    .SingleOrDefaultAsync(m => m.Id == id);
            var article = await _articleService.GetModelAsync(id.Value);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var article = await _context.Article.SingleOrDefaultAsync(m => m.Id == id);
            //_context.Article.Remove(article);
            //await _context.SaveChangesAsync();
            await _articleService.DelByAsync(f => f.Id == id);
            return RedirectToAction(nameof(List));
        }

        private bool ArticleExists(int id)
        {
            return _articleService.GetModel(id) == null;
        }
    }
}
