/** 
* SkipAttribute.cs
*
* 功 能： 特性，用它标识的不过滤
* 类 名： SkipAttribute
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/17 10:25:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;

namespace Common.Atrributes
{
    /// <summary>
    /// 特性，用它标识的不过滤
    /// </summary>
    public class SkipLoginAttribute : Attribute
    {
    }
}
