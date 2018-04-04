/** 
* IAdminLoginLogDAL.cs
*
* 功 能： AdminLoginLog数据层接口
* 类 名： IAdminLoginLogDAL
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/20 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace IDAL
{
    /// <summary>
    /// 账号登录日志DAL
    /// </summary>
    public partial interface IAdminLoginLogDAL
    {
        bool CheckLoginErrorCount(int maxErrorCount, int tyrMinutes, string ip, out DateTime? lastLoginTime);
    }
}