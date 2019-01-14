using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public static class StringExtension
    {
        /// <summary>
        /// 多字符串包含
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool MyContains(this string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            foreach (string s in value.Split(','))
            {
                if (source.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 对字符串进行多重替换
        /// </summary>
        /// <param name="source">可以,分隔，按顺序替换</param>
        /// <param name="strBeReplaced">被替换词，可以,分隔，按顺序替换</param>
        /// <param name="strReplacer">替换词，可以,分隔，按顺序替换</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string MyReplace(this string source, string strBeReplaced, string strReplacer, char separator)
        {
            var sourceArr = source.Split(separator);
            var beReplacedArr = strBeReplaced.Split(separator);
            var replacerArr = strReplacer.Split(separator);
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(strBeReplaced) || sourceArr == null || beReplacedArr ==null || replacerArr == null)
            {
                return source;
            }
            for(int i = 0; i < beReplacedArr.Length; i++)
            {
                //如果被替换与替换的个数不一致，替换为替换词中第一个
                source = beReplacedArr.Length == replacerArr.Length ? source.Replace(beReplacedArr[i], replacerArr[i])
                    : source.Replace(beReplacedArr[i], replacerArr[0]);
            }
            return source;
        }
    }
}
