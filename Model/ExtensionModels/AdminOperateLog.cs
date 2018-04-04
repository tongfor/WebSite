/** 
* AdminOperateLog.cs
*
* 功 能： AdminOperateLog类设置默认值
* 类 名： AdminOperateLog
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/5/26 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都盈辉创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public partial class AdminOperateLog
    {
        public AdminOperateLog()
        {
            OperateTime = DateTime.Now;
            AddTime = DateTime.Now;
            EditTime = DateTime.Now;
        }
    }
}
