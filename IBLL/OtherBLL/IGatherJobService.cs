/** 
* IGatherJobService.cs
*
* 功 能： 定时执行采集任务接口
* 类 名： IGatherJobService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019/2/20 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Text;

namespace IBLL
{
    public interface IGatherJobService
    {
        /// <summary>
        /// 定时执行任务
        /// </summary>
        void Run();
    }
}
