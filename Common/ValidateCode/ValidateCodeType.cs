/** 
* ValidateCodeType.cs
*
* 功 能： 验证码类型
* 类 名： ValidateCodeType
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/19 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace Common.ValidateCode
{
    /// <summary>
    /// 图片验证码抽象类
    /// </summary>
    public abstract class ValidateCodeType
    {
        protected ValidateCodeType()
        {
        }

        public abstract byte[] CreateImage(out string resultCode);

        public abstract string Name { get; }

        public virtual string Tip
        {
            get
            {
                return "请输入图片中的字符";
            }
        }

        public string Type
        {
            get
            {
                return base.GetType().Name;
            }
        }
    }
}
