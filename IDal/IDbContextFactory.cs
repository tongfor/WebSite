/** 
* IDbContextFactory.cs
*
* 功 能： 数据层上下文接口
* 类 名： IDbContextFactory
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/8/29 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using Models;

namespace IDAL
{
    public interface IDbContextFactory
    {
        /// <summary>
        /// 获取EF上下文
        /// </summary>
        /// <returns></returns>
        CdyhcdDBContext GetDbContext();
    }
}
