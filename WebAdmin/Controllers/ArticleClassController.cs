using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.AspNetCore.Extensions;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using Newtonsoft.Json;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class ArticleClassController : BaseController
    {
        private readonly IArticleClassService _articleClassService;
        private readonly IArticleService _articleService;

        public ArticleClassController(IArticleClassService articleClassService, IArticleService articleService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, ILogger<ArticleClassController> logger, IOptions<SiteConfig> options, IOptions<GatherConfig> gatherOptions) : base(operateLogService, adminBugService, adminMenuService, options, gatherOptions)
        {
            _articleClassService = articleClassService;
            _articleService = articleService;
            _logger = logger;
        }

        // GET: ArticleClass
        public async Task<IActionResult> Index(BaseRequest request)
        {
            try
            {
                if (request.PageSize == 0)
                {
                    request.PageSize = 10;
                }
                IEnumerable<ArticleClass> articleClassList = await _articleClassService.GetPagedArticleClassListAsync(request);

                await CreateLeftMenuAsync();
                return View(articleClassList as PagedList<ArticleClass>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "文章类别列表页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        #region 新增
        public async Task<ActionResult> Create()
        {
            try
            {
                ViewBag.ParentId = 0;
                var model = new ArticleClass();
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
                    BugInfo = "创建文章类别页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: Activity/Create
        [HttpPost]
        public async Task<IActionResult> Create(ArticleClass articleClass)
        {
            try
            {
                if ("根目录".Equals(articleClass.ParentId) || articleClass.ParentId == null)
                {
                    articleClass.ParentId = 0;
                }
                await _articleClassService.AddArticleClassAsync(articleClass);

                //return this.RefreshParent();
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "添加成功！", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "创建文章类别功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        #region 编辑
        // GET: AdminUser/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                ViewBag.ErrMsg = "请传递参数";
                OperateLog.OperateInfo = "编辑文章类别页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未传递类别ID";
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                ArticleClass model = await _articleClassService.GetModelAsync(id.Value);
                if (model == null)
                {
                    ViewBag.ErrMsg = "页面错误";
                    OperateLog.OperateInfo = "编辑文章类别页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未找到文章类别";
                    MyOperateLogService.Add(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                ArticleClass entActivity = model.ToPOCO();
                ViewBag.ParentId = entActivity.ParentId ?? entActivity.ParentId;
                return View(entActivity);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑文章类别页面异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        // POST: AdminUser/Edit/5
        [HttpPost]
        public async Task<IActionResult> Edit(ArticleClass articleClass)
        {
            try
            {
                if (articleClass == null || articleClass.Id <= 0)
                {
                    OperateLog.OperateInfo = "编辑文章类别功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递类别ID";
                    MyOperateLogService.Add(OperateLog);

                    return PackagingAjaxMsg(AjaxStatus.Err, "未传递参数！", null);
                }

                ArticleClass model = _articleClassService.GetModelBy(f => f.Id == articleClass.Id);
                await TryUpdateModelAsync<ArticleClass>(model);
                await _articleClassService.ModifyArticleClassAsync(model);

                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "修改成功！", null);
                //return this.RefreshParent();
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "编辑文章类别功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        #region 删除
        // GET: AdminUser/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    OperateLog.OperateInfo = "删除文章类别功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递类别ID";
                    MyOperateLogService.Add(OperateLog);
                }
                //查询要删除的单个
                ArticleClass act = await _articleClassService.GetModelAsync(id.Value);//根据Id查询单个
                //根据单个的路径，查询他子目录
                List<ArticleClass> actClassList = await _articleClassService.GetListByAsync(a => a.Path.StartsWith(act.Path + ","));

                List<int?> ids = new List<int?>();
                foreach (var item in actClassList)
                {
                    ids.Add(item.Id);
                }
                ids.Add(id.Value);
                if (ids.Count > 0)
                {
                    int deleteArticleCount = _articleService.DelBy(ac => ids.Contains(ac.ClassId));
                    int resultCount = _articleClassService.DelBy(f => ids.Contains(f.Id));

                    return RedirectToAction("Index", "ArticleClass", Request.QueryString);
                }
                else
                {
                    return  PackagingAjaxMsg(AjaxStatus.Err, "删除失败！", null);
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "删除文章类别功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }
        #endregion

        /// <summary>
        /// 查询所有文章类别树,并以ajax模式返回
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticleClassTree(int classid)
        {
            try
            {
                List<ArticleClassTreeView> data = await _articleClassService.GetAllArticleClassTreeAsync(classid);
                return PackagingAjaxMsg(AjaxStatus.IsSuccess, "查询成功 ", data);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "AJAX获取文章类别功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 查询所有文章类别树,并以JSON字符串返回
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticleClassTreeJson(int classid)
        {
            try
            {
                string data = await _articleClassService.GetAllArticleClassTreeJsonAsync(classid);
                return Content(data);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "获取文章类别Json功能异常" + ex.Message,
                    BugMessage = JsonUtil.StringFilter(ex.StackTrace.ToString()),
                    UserName = User != null && User.Identity != null ? User.Identity.Name : "",
                    AddTime = DateTime.Now,
                    EditTime = DateTime.Now
                };
               await MyAdminBugService.AddAsync(Bug);
                return PackagingAjaxMsg(AjaxStatus.Err, Bug.BugInfo);
            }
        }

        /// <summary>
        /// 查询所有文章类别树,并以JSON字符串返回(zTree使用)
        /// </summary>
        /// <param name="classid"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllArticleClassTreeJsonForzTree(int? classid)
        {
            try
            {
                if (classid == null)
                {
                    return PackagingAjaxMsg(AjaxStatus.Err, "请输入正确的文章类别！");
                }
                string strJson = await _articleClassService.GetAllArticleClassTreeJsonForzTreeAsync(classid.Value);
                var jsonResult = JsonConvert.DeserializeObject(strJson);
                
                return Json(jsonResult);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;

                Bug = new AdminBug
                {
                    UserIp = HttpContext.GetUserIp(),
                    IsShow = 1,
                    IsSolve = 0,
                    BugInfo = "为zTree获取文章类别Json功能异常" + ex.Message,
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