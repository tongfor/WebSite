/** 
* Board.cs
*
* 功 能： Board类设置默认值
* 类 名： Board
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/5/23 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    [MetadataType(typeof(BoardMetadata))]
    public partial class Board
    {
        public Board()
        {
            AddTime = DateTime.Now;
            EditTime = DateTime.Now;
            IsChecked = 1;
            IsDel = 0;
        }
    }
}
