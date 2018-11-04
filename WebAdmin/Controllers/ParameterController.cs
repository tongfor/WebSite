using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Config;
using IBLL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Models;
using WebAdmin.Models;

namespace WebAdmin.Controllers
{
    public class ParameterController : BaseController
    {
        private readonly IParameterService _parameterClassService;

        public ParameterController(IParameterService parameterService, IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, ILogger<ArticleClassController> logger, IOptions<SiteConfig> options) : base(operateLogService, adminBugService, adminMenuService, options)
        {
            _parameterClassService = parameterService;
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
                IEnumerable<Parameter> parameterList = await _parameterClassService.GetListByAsync(f => 1 == 1);

                await CreateLeftMenuAsync();
                return View(parameterList as PagedList<ArticleClass>);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "参数设置列表页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        #region 新增
        public ActionResult Create()
        {
            try
            {
                ViewBag.ParentId = 0;
                var model = new Parameter();
                return View("Edit", model);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "创建参数设置页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }

        // POST: Activity/Create
        [HttpPost]
        public async Task<IActionResult> Create(Parameter parameter)
        {
            try
            {
                if ("父级分类".Equals(parameter.ParParentId))
                {
                    parameter.ParParentId = 0;
                }
                int resultNum = await _parameterClassService.AddAsync(parameter);

                return resultNum > 0 ? PackagingAjaxMsg(AjaxStatus.IsSuccess, "添加成功！", null) : PackagingAjaxMsg(AjaxStatus.Err, "添加失败！", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "创建参数设置功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                OperateLog.OperateInfo = "编辑参数设置页面异常";
                OperateLog.IsSuccess = 0;
                OperateLog.Description = "未传递类别ID";
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
            try
            {
                Parameter model = await _parameterClassService.GetModelAsync(id.Value);
                if (model == null)
                {
                    ViewBag.ErrMsg = "页面错误";
                    OperateLog.OperateInfo = "编辑参数设置页面异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未找到参数设置";
                    MyOperateLogService.Add(OperateLog);
                    return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
                }
                Parameter pocoModel = model.ToPOCO();
                ViewBag.ParParentId = pocoModel.ParParentId ?? pocoModel.ParParentId;
                return View(pocoModel);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "编辑参数设置页面异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                    OperateLog.OperateInfo = "编辑参数设置功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递类别ID";
                    MyOperateLogService.Add(OperateLog);

                    return PackagingAjaxMsg(AjaxStatus.Err, "未传递参数！", null);
                }

                Parameter model = _parameterClassService.GetModelBy(f => f.Id == articleClass.Id);
                await TryUpdateModelAsync<Parameter>(model);
                int resultNum = await _parameterClassService.ModifyAsync(model);

                return resultNum > 0 ? PackagingAjaxMsg(AjaxStatus.IsSuccess, "修改成功！", null) : PackagingAjaxMsg(AjaxStatus.Err, "修改失败！", null);
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "编辑参数设置功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                    OperateLog.OperateInfo = "删除参数设置功能异常";
                    OperateLog.IsSuccess = 0;
                    OperateLog.Description = "未传递类别ID";
                    MyOperateLogService.Add(OperateLog);
                }
                //查询要删除的单个
                Parameter model = await _parameterClassService.GetModelAsync(id.Value);//根据Id查询单个
                //根据单个的路径，查询他子目录
                List<Parameter> parameterList = await _parameterClassService.GetListByAsync(a => a.ParPath.StartsWith(model.ParPath));

                List<int?> ids = new List<int?>();
                foreach (var item in parameterList)
                {
                    ids.Add(item.Id);
                }
                if (ids.Count > 0)
                {
                    int deleteArticleCount = _parameterClassService.DelBy(ac => ids.Contains(ac.ClassId));
                    int resultCount = _parameterClassService.DelBy(f => ids.Contains(f.Id));

                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrMsg = ex.Message;
                OperateLog.OperateInfo = "删除参数设置功能异常-" + ex.Message;
                OperateLog.IsSuccess = 0;
                OperateLog.Description = JsonUtil.StringFilter(ex.StackTrace.ToString());
                MyOperateLogService.Add(OperateLog);
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
        #endregion
    }
}