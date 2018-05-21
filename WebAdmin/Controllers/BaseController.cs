/** 
* BaseController.cs
*
* 功 能： 父类控制器
* 类 名： BaseController
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2018/4/27 10:25:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Models;
using IBLL;
using Setting.Constant;
using Common.Config;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace WebAdmin.Controllers
{
    /// <summary>
    /// 父类控制器
    /// </summary>
    [Authorize]
    public class BaseController : Controller
    {
        protected IAdminOperateLogService MyIOperateLogService;        
        protected IAdminBugService MyIAdminBugService;
        protected IAdminMenuService MyIAdminMenuService;
        protected AdminOperateLog OperateLog = new AdminOperateLog();  //操作日志对象
        protected AdminBug Bug = new AdminBug(); //BUG记录对象
        protected Microsoft.Extensions.Logging.ILogger _logger;

        public SiteConfig SiteConfigSettings;//站点设置

        public BaseController(IAdminOperateLogService operateLogService, IAdminBugService adminBugService, IAdminMenuService adminMenuService, IOptions<SiteConfig> options)
        {
            MyIOperateLogService = operateLogService;
            MyIAdminBugService = adminBugService;
            MyIAdminMenuService = adminMenuService;
            SiteConfigSettings = options.Value;
            CreateLeftMenu();
        }

        public BaseController(IAdminOperateLogService operateLogService)
        {
            MyIOperateLogService = operateLogService;
        }

        #region 把ajax值封闭成json格式返回
        /// <summary>
        /// 把ajax值封闭成json格式返回
        /// </summary>
        /// <param name="status">ajax状态</param>
        /// <param name="msg">ajax信息</param>
        /// <param name="data">ajax数据</param>
        /// <param name="returnurl">调用后的链接</param>
        /// <returns>json格式的ajax数据</returns>
        public ActionResult PackagingAjaxMsg(AjaxStatus status, string msg, object data = null, string returnurl = "")
        {
            AjaxMsgModel amm = new AjaxMsgModel
            {
                Status = status,
                Msg = msg,
                Data = data,
                ReturnUrl = returnurl
            };
            JsonResult ajaxRes = new JsonResult(amm)
            {
                Value = amm
            };
            return ajaxRes;
        }
        #endregion

        #region 重定向方法

        /// <summary>
        /// 重定向方法
        /// </summary>
        /// <param name="url">重定向url</param>
        /// <param name="action">请求的Action</param>
        /// <returns></returns>
        public ActionResult Redirect(string url, ActionDescriptor action)
        {
            //如果是ajax请求则返回Json格式数据
            //if (action.IsDefined(typeof(AjaxRequestAttribute), false) || action.ControllerDescriptor.IsDefined(typeof(AjaxRequestAttribute), false))
            //{
            //    return PackagingAjaxMsg(AjaxStatus.NoLogin, "您尚未登录或无权限访问此页面！", null, url);
            //}
            //else//如果是超链或表单
            {
                return new RedirectResult(url);
            }
        }

        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <returns></returns>
        public ContentResult Alert(string alert)
        {
            var script = string.Format("<script>{0}; </script>", "alert('" + alert + "')");
            return this.Content(script);
        }

        /// <summary>
        /// 当弹出DIV弹窗时，需要刷新浏览器整个页面
        /// </summary>
        /// <returns></returns>
        public ContentResult RefreshParent(string alert = null)
        {
            var script = string.Format("<script>{0}; parent.location.reload(1)</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        public ContentResult RefreshParentTab(string alert = null)
        {
            var script = string.Format("<script>{0}; if (window.opener != null) {{ window.opener.location.reload(); window.opener = null;window.open('', '_self', '');  window.close()}} else {{parent.location.reload(1)}}</script>", string.IsNullOrEmpty(alert) ? string.Empty : "alert('" + alert + "')");
            return this.Content(script);
        }

        /// <summary>
        /// 用JS关闭弹窗
        /// </summary>
        /// <returns></returns>
        public ContentResult CloseThickbox()
        {
            return this.Content("<script>top.tb_remove()</script>");
        }

        /// <summary>
        ///  警告并且历史返回
        /// </summary>
        /// <param name="notice"></param>
        /// <returns></returns>
        public ContentResult Back(string notice)
        {
            var content = new StringBuilder("<script>");
            if (!string.IsNullOrEmpty(notice))
                content.AppendFormat("alert('{0}');", notice);
            content.Append("history.go(-1)</script>");
            return this.Content(content.ToString());
        }

        /// <summary>
        /// 弹出提示并返回页面
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public ContentResult PageReturn(string msg, string url = null)
        {
            var content = new StringBuilder("<script type='text/javascript'>");
            if (!string.IsNullOrEmpty(msg))
                content.AppendFormat("alert('{0}');", msg);
            if (string.IsNullOrWhiteSpace(url))
                url = Request.GetAbsoluteUri();
            content.Append("window.location.href='" + url + "'</script>");
            return this.Content(content.ToString());
        }

        #endregion

        #region session操作

        #region 当前Session中保存的后台用户验证码

        /// <summary>
        /// 当前Session中保存的后台用户验证码
        /// </summary>
        public string CurrentAdminVerificationCode
        {
            get
            {
                return HttpContext.Session.GetString(HttpKeys.AdminVerificationCodeSessionName);
            }
            set
            {
                HttpContext.Session.SetString(HttpKeys.AdminVerificationCodeSessionName, value);
            }
        }

        #endregion

        #region 当前Session中保存的前台用户验证码

        /// <summary>
        /// 当前Session中保存的前台用户验证码
        /// </summary>
        public string CurrentVerificationCode
        {
            get
            {
                return HttpContext.Session.GetString(HttpKeys.VerificationCodeSessionName);
            }
            set
            {
                HttpContext.Session.SetString(HttpKeys.VerificationCodeSessionName, value);
            }
        }

        #endregion

        #endregion session操作
       
        #region 获取文章关联文章类别的数据(Ajax)

        /// <summary>
        /// 获取文章关联文章类别的数据(Ajax)
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public List<AdminUserMenuView> GetAdminUserMenuTree(int userId)
        {
            List<AdminUserMenuView> menuList = new List<AdminUserMenuView>();
            List<AdminUserMenuView> menuTreeList = new List<AdminUserMenuView>();
            menuList = MyIAdminMenuService.GetAdminUserMenu(userId);
            menuTreeList = MyIAdminMenuService.GetAdminUserMenuTree(menuList, 0);
            return menuTreeList;
        }

        #endregion

        #region 分部视图

        public PartialViewResult CreateLeftMenu()
        {
            var model = GetAdminUserMenuTree(int.Parse(User.Claims.ToList().FirstOrDefault().Value));
            return PartialView("Menu", model);
        }

        #endregion 分部视图
    }
}