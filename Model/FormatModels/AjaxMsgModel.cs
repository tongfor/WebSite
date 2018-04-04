/** 
* AjaxMsgModel.cs
*
* 功 能： 操作返回结果模型
* 类 名： AjaxMsgModel
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/6 1224:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace Models
{
    /// <summary>
    /// 操作返回结果模型
    /// </summary>
    public class AjaxMsgModel
    {
        /// <summary>
        /// 操作是否成功
        /// </summary>
        public AjaxStatus Status { get; set; }

        /// <summary>
        /// 操作结果提示
        /// </summary>
        public string Msg { get; set; }

        /// <summary>
        /// 结果集JSO串，可为空
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 要返回的Url
        /// </summary>
        public string ReturnUrl { get; set; }
    }

    /// <summary>
    /// 操作结果类型定义枚举
    /// </summary>
    public enum AjaxStatus
    {
        IsSuccess = 0,
        Err = 1,
        LoginFail = 2,
        NoLogin = 3,
        None = 4
    }
}
