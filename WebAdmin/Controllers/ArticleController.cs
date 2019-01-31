using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class ArticleController : BaseController
    {
        private readonly IArticleService _articleService;
        private readonly IArticleClassService _articleClassService;

        public ArticleController(IArticleService articleService, IArticleClassService articleClassService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, ILogger<ArticleController> logger, IOptionsSnapshot<SiteConfig> options, IOptionsSnapshot<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
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
                    MyOperateLogService.Add(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }

                ViewBag.ClassId = request == null ? 0 : request.ClassId;

                if (request.PageSize <= 0)
                {
                    request.PageSize = 15;
                }
                IEnumerable<ArticleView> articleList = await _articleService.GetArticleListBySqlAsync(request);

                await CreateLeftMenuAsync();
                ViewBag.DomainName = SiteConfigSettings.DefaultDomainName;
                return View(articleList as PagedList<ArticleView>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "文章列表页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Article/Create
        public async Task<IActionResult> Create()
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

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "新增文章页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Article/Create
        [HttpPost]
        public async Task<IActionResult> Create(ArticleView article)
        {
            try
            {
                if (string.IsNullOrEmpty(article.Title))
                {
                    ModelState.AddModelError("Title", "请填写文章标题!");
                }
                if (article.ClassId == null || article.ClassId <= 0)
                {
                    ModelState.AddModelError("ClassId", "请选择文章类别!");
                }
                if (string.IsNullOrEmpty(article.Content))
                {
                    ModelState.AddModelError("Content", "请填写文章内容!");
                }

                if (ModelState.IsValid)
                {
                    Article model = article.ToOriginal();
                    article.UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name;
                    model.AddTime = model.AddTime ?? DateTime.Now;
                    model.EditTime = model.EditTime ?? DateTime.Now;
                    ViewBag.ClassId = 0;

                    int addedCount = await _articleService.AddAsync(model);

                    return PackagingAjaxMsg(addedCount > 0 ? AjaxStatus.IsSuccess : AjaxStatus.Err, addedCount > 0 ? "添加成功！" : "添加失败！", null);
                    //return this.RefreshParent();
                }

                return View("Edit", article);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "新增文章功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
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
                    MyOperateLogService.Add(OperateLog);
                    return PackagingAjaxMsg(AjaxStatus.Err, "请传递正确参数!");
                }
                ArticleView model = new ArticleView(await  _articleService.GetModelAsync(id.Value));
                ViewBag.ClassId = model.ClassId ?? model.ClassId;

                return View(model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑文章页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
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
                    MyOperateLogService.Add(OperateLog);
                    ModelState.AddModelError("Title", "请传递文章ID!");
                }

                if (string.IsNullOrEmpty(article.Title))
                {
                    ModelState.AddModelError("Title", "请填写文章标题!");
                }
                if (article.ClassId == null || article.ClassId <= 0)
                {
                    ModelState.AddModelError("ClassId", "请选择文章类别!");
                }
                if (string.IsNullOrEmpty(article.Content))
                {
                    ModelState.AddModelError("Content", "请填写文章内容!");                    
                }
                article.UserName = string.IsNullOrEmpty(User.Identity.Name) ? "" : User.Identity.Name;

                if (ModelState.IsValid)
                {
                    Article modifyModel = article.ToOriginal();
                    ViewBag.ClassId = modifyModel.ClassId ?? modifyModel.ClassId;
                    modifyModel.EditTime = DateTime.Now;

                    int modifiedCount = await _articleService.ModifyAsync(modifyModel);

                    return PackagingAjaxMsg(modifiedCount > 0 ? AjaxStatus.IsSuccess : AjaxStatus.Err, modifiedCount > 0 ? "修改成功！" : "修改失败！", null);
                    //return this.RefreshParent();
                }

                return View(article);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑文章功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
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
                    MyOperateLogService.Add(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                await _articleService.DelByAsync(f => f.Id == id.Value);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "删除文章功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // GET: Article/Delete/5
        public async Task<IActionResult> DeleteAjax(int? id)
        {
            try
            {
                if (id == null)
                {
                    ViewBag.ErrMsg = "请传递正确参数!";
                    OperateLog.OperateInfo = "删除文章功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递文章ID";
                    MyOperateLogService.Add(OperateLog);
                    return PackagingAjaxMsg(AjaxStatus.Err, "请传递正确参数！", null);
                }
                var result = await _articleService.DelByAsync(f => f.Id == id.Value);
                return PackagingAjaxMsg(result > 0 ? AjaxStatus.IsSuccess : AjaxStatus.Err, result > 0 ? "删除成功！" : "删除失败！", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "删除文章功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
                await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
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

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "批量删除文章功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
    }
}