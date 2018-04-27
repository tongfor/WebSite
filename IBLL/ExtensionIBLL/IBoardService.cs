/** 
* IBoardService.cs
*
* 功 能： IBoard逻辑层接口
* 类 名： IBoardService
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
using System.Collections.Generic;
using System.Linq.Expressions;
using Common;
using Models;

namespace IBLL
{
    partial interface IBoardService
    {
        #region 根据请求条件获取IPageList格式数据
        IEnumerable<Board> GetBoardList(BoardRequest request = null);

        #endregion

        #region 根据请求条件获取Board分页列表，并返回总数
        List<Board> GetBoardList(out int totalCount, BoardRequest request = null);

        #endregion

        #region 按时间限制添加模型

        int LimitAdd(Board board, int limitHours, int limitCount);

        #endregion 按时间限制添加模型
    }
}
