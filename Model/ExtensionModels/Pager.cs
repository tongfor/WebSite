/** 
* Pager.cs
*
* 功 能： 分页控制类
* 类 名： Pager
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/2 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace Models
{
    /// <summary>
    /// 分页控制类
    /// </summary>
    public class Pager
    {
        /// <summary>
        /// 页码
        /// </summary>
        //public int PageIndex { get; set; } = 1;
        private int _pageIndex = 1;
        /// <summary>
        /// 页码
        /// </summary>
        public int PageIndex
        {
            get { return _pageIndex; }
            set { _pageIndex = value; }
        }

        /// <summary>
        /// 每页显示条数
        /// </summary>
        //public int PageSize { get; set; } = 1;
        private int _pageSize;
        /// <summary>
        /// 每页显示条数
        /// </summary>
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }

        /// <summary>
        /// 数据总量
        /// </summary>
        //public int TotalCount { get; set; } = 1;
        private int _totalCount;
        /// <summary>
        /// 数据总量
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }
    }
}
