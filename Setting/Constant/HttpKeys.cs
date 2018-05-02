using System;
using System.Collections.Generic;
using System.Text;

namespace Setting.Constant
{
    /// <summary>
    /// Http请求中用到的常量
    /// </summary>
    public class HttpKeys
    {
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
    }
}
