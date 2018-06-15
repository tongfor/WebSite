using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;

namespace WebAdmin.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;

        public ArticleController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, ILogger<ArticleController> logger, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            _articleService = articleService;
            _articleClassService = articleClassService;
            _logger = logger;            
        }

        // GET: Article
        public async Task<IActionResult> Index(ArticleRequest request)
        {
            try
            {
                ViewBag.KeyWord = request.Title;
                ViewBag.CurrentPageIndex = request.PageIndex <= 1 ? 1 : request.PageIndex;
                ViewBag.TotalPageCount = 1;

                if (request == null)
                {
                    ViewBag.ErrMsg = "未正确传递参数";
                    OperateLog.OperateInfo = "文章列表页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未正确传递文章参数";
                    MyIOperateLogService.Add(OperateLog);
                    return View("Error");
                }

                ViewBag.ClassId = request == null ? 0 : request.ClassId;

                if (request.PageSize <= 0)
                {
                    request.PageSize = 15;
                }
                IEnumerable<ArticleView> articleList = await _articleService.GetArticleListBySqlAsync(request);

                CreateLeftMenu();
                ViewBag.DomainName = SiteConfigSettings.DefaultDomainName;
                return View(articleList as PagedList<ArticleView>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "文章列表页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        // GET: Article/Create
        public IActionResult Create()
        {
            try
            {
                ViewBag.ClassId = 0;

                var model = new ArticleView();
                return View("Edit", model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "新增文章页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        // POST: Article/Create
        [HttpPost]
        public async Task<IActionResult> Create(ArticleView article)
        {
            try
            {
                if (article.ClassId == null || article.ClassId <= 0)
                {
                    ModelState.AddModelError("ClassId", "请选择文章类别!");
                    return View("Edit", article);
                }
                if (article.Content == null)
                {
                    ModelState.AddModelError("Content", "请填写文章内容!");
                    return View("Edit", article);
                }

                Article model = article.ToOriginal();
                model.UserName = User.Identity.Name;
                ViewBag.ClassId = 0;                

                await _articleService.AddAsync(model);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "添加成功！", null);
                //return this.RefreshParent();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "新增文章功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        // GET: Article/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "编辑文章页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递文章ID";
                    MyIOperateLogService.Add(OperateLog);
                    return View("Error");
                }
                ArticleView model = new ArticleView(await  _articleService.GetModelAsync(id.Value));
                ViewBag.ClassId = model.ClassId ?? model.ClassId;

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "编辑文章页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        // POST: Article/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleView article)
        {
            try
            {
                if (article.Id <= 0)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "编辑文章功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递文章ID";
                    MyIOperateLogService.Add(OperateLog);
                    return View("Error");
                }
                if (article.ClassId == null || article.ClassId <= 0)
                {
                    ModelState.AddModelError("ClassId", "请选择文章类别!");
                    return View("Edit", article);
                }
                if (article.Content == null)
                {
                    ModelState.AddModelError("Content", "请填写文章内容!");
                    return View("article", article);
                }

                Article modifyModel = article.ToOriginal();
                ViewBag.ClassId = modifyModel.ClassId ?? modifyModel.ClassId;                
                modifyModel.EditTime = DateTime.Now;

                await _articleService.ModifyAsync(modifyModel);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "修改成功！", null);
                //return this.RefreshParent();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "编辑文章功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "删除文章功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递文章ID";
                    MyIOperateLogService.Add(OperateLog);
                    return View("Error");
                }
                await _articleService.DelByAsync(f => f.Id == id.Value);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "删除文章功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }

        //批量删除
        [HttpPost]
        public async Task<IActionResult> Delete(List<int> ids)
        {
            try
            {
                await _articleService.DelByAsync(f => ids.Contains(f.Id));
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "批量删除文章功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyIOperateLogService.Add(OperateLog);
                return View("Error");
            }
        }
    }
}