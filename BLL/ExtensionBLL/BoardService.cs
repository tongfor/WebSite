/** 
* BoardService.cs
*
* 功 能： Board逻辑层
* 类 名： BoardService
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
using IDAL;
using Models;

namespace BLL
{
    public partial class BoardService
    {
        #region 根据请求条件获取IPageList格式数据
        public IEnumerable<Board> GetBoardList(BoardRequest request = null)
        {
            request = request ?? new BoardRequest();
            Expression<Func<Board, bool>> queryWhere = f => true;

            if (!string.IsNullOrEmpty(request.Title) && Utils.IsSafeSqlString(request.Title))
            {
                queryWhere = f => f.Title.Contains(request.Title);
            }

            int totalCount;
            var boards = GetPageListBy(request.PageIndex, request.PageSize, queryWhere, o => o.AddTime, out totalCount, true);
            
            return boards.ToPagedList(request.PageIndex, request.PageSize, totalCount);
        }

        #endregion

        #region 根据请求条件获取Board分页列表，并返回总数
        public List<Board> GetBoardList(out int totalCount, BoardRequest request = null)
        {
            request = request ?? new BoardRequest();
            Expression<Func<Board, bool>> queryWhere = f => f.IsDel == 0;

            if (!string.IsNullOrEmpty(request.Title))
            {
                queryWhere = f => f.Title.Contains(request.Title);
            }
            if (string.IsNullOrEmpty(request.OrderBy) || !CanOrdered(request.OrderBy))
            {
                request.OrderBy = "AddTime desc";
            }
            var boards = GetPageListBy(request.PageIndex, request.PageSize, queryWhere, request.OrderBy, out totalCount);

            return boards;
        }

        #endregion

        #region 按时间限制添加模型

        public int LimitAdd(Board board, int limitHours, int limitCount)
        {
            DateTime limitDateTime=DateTime.Now.AddHours(-limitHours);
            if (board.AddTime != null)
            {
                limitDateTime = board.AddTime.Value.AddHours(-limitHours);
            }
            var boardList = GetListBy(f => f.Ip == board.Ip && f.AddTime >= limitDateTime);
            return boardList.Count >= limitCount ? 0 : Add(board);
        }

        #endregion 按时间限制添加模型
    }
}
