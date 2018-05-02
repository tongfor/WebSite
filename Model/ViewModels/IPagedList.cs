/** 
* IPagedList.cs
*
* 功 能： 分页接口
* 类 名： IPagedList
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 10:24:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace Models
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedList
    {
        int CurrentPageIndex { get; set; }
        int PageSize { get; set; }
        int TotalItemCount { get; set; }
    }
}
