/** 
* BusinessException.cs
*
* 功 能：  业务异常，用于在后端抛出到前端做相应处理
* 类 名： BusinessException
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/21 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;

namespace Models
{
    /// <summary>
    /// 业务异常，用于在后端抛出到前端做相应处理
    /// </summary>
    public class BusinessException : Exception
    {
        public BusinessException()
            : this(string.Empty)
        {
        }

        public BusinessException(string message) :
            this("error", message)
        {
        }

        public BusinessException(string name, string message)
            : base(message)
        {
            this.Name = name;
        }

        public BusinessException(string message, Enum errorCode)
            : this("error", message, errorCode)
        {
        }

        public BusinessException(string name, string message, Enum errorCode)
            : base(message)
        {
            this.Name = name;
            this.ErrorCode = errorCode;
        }

        public string Name { get; set; }
        public Enum ErrorCode { get; set; }
    }
}
