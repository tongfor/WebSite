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
            foreach (string s in value.Split(','))
            {
                if (source.Contains(s))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
