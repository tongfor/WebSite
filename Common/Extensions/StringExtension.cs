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
        public static bool ContainsAny(this string source, string value)
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
        /// 对包含多个字符串的字符串进行分隔替换
        /// </summary>
        /// <param name="source">可以,分隔，按顺序替换</param>
        /// <param name="strBeReplaced">被替换词，可以,分隔，按顺序替换</param>
        /// <param name="strReplacer">替换词，可以,分隔，按顺序替换</param>
        /// <param name="separator">分隔符</param>
        /// <returns></returns>
        public static string ReplaceEvery(this string source, string strBeReplaced, string strReplacer, char separator)
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

        /// <summary>
        /// 多字符串排除，不包含任一字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ExcludeAll(this string source, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }
            foreach (string s in value.Split(','))
            {
                if (source.Contains(s))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToTitleCase(this string source)
        {
            source = source.Substring(0, 1).ToUpper() + source.Substring(1);
            return source;
        }
    }
}
