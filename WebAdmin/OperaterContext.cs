/** 
* OperaterContext.cs
*
* 功 能： 上下文操作封装
* 类 名： OperaterContext
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/19 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using Common;
using Microsoft.AspNetCore.Http;
using Models;

namespace WebAdmin
{
    /// <summary>
    /// 上下文操作封装
    /// </summary>
    public class OperaterContext
    {
        private readonly string _cacheKey = "operater_Cache";

        #region 当前上下文操作
        /// <summary>
        /// 放入线程中当前上下文操作
        /// </summary>
        public static OperaterContext CurrentContext
        {
            get
            {
                OperaterContext operaterContext = CacheHelper.GetCacheValue("_cacheKey") as OperaterContext;
                if (operaterContext==null)
                {
                    operaterContext = new OperaterContext();
                }
                return operaterContext;
            }
        }
        #endregion

        #region 名称常量

        /// <summary>
        /// 用于在Session中保存的后台登录验证码名
        /// </summary>
        public const string AdminVerificationCodeSessionName = "VerificationCode";

        /// <summary>
        /// 用于在Session中保存的前台登录验证码名
        /// </summary>
        public const string VerificationCodeSessionName = "VerificationCode";

        /// <summary>
        /// 用于在Session中保存的后台用户信息名
        /// </summary>
        public const string AdminUserSessionName = "AdminUserInfo";

        /// <summary>
        /// 用于在Session中保存的前台用户信息名
        /// </summary>
        public const string UserInfoSessionName = "UserInfo";

        /// <summary>
        /// 用于在Cookie中保存后台用户名的名称
        /// </summary>
        public const string AdminUserNameCookieName = "AdminUserName";

        /// <summary>
        /// 用于在Session中保存的前台用户信息名
        /// </summary>
        public const string UserNameCookieName = "UserName";

        #endregion        

        #region 封装http对象

        #region http上下文

        HttpContext ContextHttp
        {
            get { return HttpContext.Current; }
        }

        #endregion

        #region response对象

        public HttpResponse Response
        {
            get
            {
                return ContextHttp.Response;
            }
        }

        #endregion

        #region Request对象

        public HttpRequest Request
        {
            get
            {
                return ContextHttp.Request;
            }
        }

        #endregion

        #region Session对象

        public HttpSessionState Session
        {
            get
            {
                return ContextHttp.Session;
            }
        }

        #endregion



        #endregion

        #region 当前Session中保存的后台用户验证码

        /// <summary>
        /// 当前Session中保存的后台用户验证码
        /// </summary>
        public string CurrentAdminVerificationCode
        {
            get
            {
                return Session[AdminVerificationCodeSessionName] as string;
            }
            set
            {
                Session[AdminVerificationCodeSessionName] = value;
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
                return Session[VerificationCodeSessionName] as string;
            }
            set
            {
                Session[VerificationCodeSessionName] = value;
            }
        }

        #endregion

        #region 当前保存的后台用户信息

        /// <summary>
        /// 当前保存的后台用户信息
        /// </summary>
        public AdminUser CurrentAdminUser
        {
            get
            {
                return Session[AdminUserSessionName] as AdminUser;
            }
            set
            {
                Session[AdminUserSessionName] = value;
            }
        }        

        #endregion

        #region 后台信息存入Cookie

        /// <summary>
        /// 后台信息存入Cookie
        /// </summary>
        /// <param name="msg"></param>
        public void SetAdminCoookie(string msg, int saveDays)
        {
            string encrypeMsg = msg.DESEncrypt();
            HttpCookie cookie = new HttpCookie(AdminUserNameCookieName, encrypeMsg);
            cookie.Expires = DateTime.Now.AddDays(saveDays);
            Response.Cookies.Add(cookie);
        }

        #endregion

        #region 取出存入Cookie的后台信息

        /// <summary>
        /// 取出存入Cookie的后台信息
        /// </summary>
        /// <param name="msg"></param>
        public string GetAdminUserNameFromCoookie()
        {
            string adminUserName = Request.Cookies[AdminUserNameCookieName] == null ? string.Empty : Request.Cookies[AdminUserNameCookieName].Value;
            return string.IsNullOrEmpty(adminUserName) ? adminUserName : adminUserName.DESDecrypt();
        }

        #endregion

        #region 后台信息清除Cookie

        /// <summary>
        /// 后台信息清除Cookie
        /// </summary>
        public void ClearAdminCoookie()
        {
            HttpCookie CookieMsg;
            CookieMsg= new HttpCookie(AdminUserNameCookieName) { Expires = DateTime.Now.AddDays(-1) };
            Response.Cookies.Add(CookieMsg);
        }

        #endregion
    }
}