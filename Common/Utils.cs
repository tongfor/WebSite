/**********************************************************************************
 * 程序说明：     公用助手(字符操作、类型字符串验证、文件操作、对象操作)类
 * 创建日期：     2009.10.20
 * 程序制作：     agui(部分引用discuz!)
 * 联系方式：     mailto:354990393@qq.com  
 * ********************************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace Common
{
    /// <summary>
    /// 公用助手(字符操作、类型字符串验证、文件操作、对象操作)类
    /// </summary>
    public abstract class Utils
    {
        
        private static DateTime UnixStartTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        private static System.Globalization.CultureInfo CultureInfoDate = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        /// <summary>
        /// 得到正则编译参数设置
        /// </summary>
        /// <returns>参数设置</returns>
        public static RegexOptions GetRegexCompiledOptions()
        {
            return RegexOptions.None;
        }
        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool IsNumeric(string expression)
        {
            if (!IsNullOrEmpty(expression))
            {
                string str = expression;
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// 是否为Double类型
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsDouble(string expression)
        {
            if (!IsNullOrEmpty(expression))
            {
                return Regex.IsMatch(expression, @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool IsNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!IsNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImgFileName(string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".") == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".") + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }
        /// <summary>
        /// 检测是否符合email格式
        /// </summary>
        /// <param name="strEmail">要判断的email字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");    //A-Z, a-z, 0-9, +, /, =
        }
        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeLinkString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }
        /// <summary>
        /// 字段串是否为Null或为""(空)
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string str)
        {
            if (str == null || str.Trim() == "")
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 是否为数值串列表，各数值间用","间隔
        /// </summary>
        /// <param name="numList"></param>
        /// <returns></returns>
        public static bool IsNumericList(string numList)
        {
            if (numList == "")
                return false;
            foreach (string num in numList.Split(','))
            {
                if (!IsNumeric(num))
                    return false;
            }
            return true;
        }
        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// 是否为ip片段
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// 返回指定IP是否在指定的IP数组所限定的范围内, IP数组内的IP地址可以使用*表示该IP段任意, 例如192.168.1.*
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="iparray"></param>
        /// <returns></returns>
        public static bool InIPArray(string ip, string[] iparray)
        {
            string[] userip = Utils.SplitString(ip, @".");
            for (int ipIndex = 0; ipIndex < iparray.Length; ipIndex++)
            {
                string[] tmpip = Utils.SplitString(iparray[ipIndex], @".");
                int r = 0;
                for (int i = 0; i < tmpip.Length; i++)
                {
                    if (tmpip[i] == "*")
                    {
                        return true;
                    }
                    if (userip.Length > i)
                    {
                        if (tmpip[i] == userip[i])
                        {
                            r++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                if (r == 4)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 判断文件流是否为UTF8字符集
        /// </summary>
        /// <param name="sbInputStream">文件流</param>
        /// <returns>判断结果</returns>
        private static bool IsUTF8(FileStream sbInputStream)
        {
            int i;
            byte cOctets;  
            byte chr;
            bool bAllAscii = true;
            long iLen = sbInputStream.Length;
            cOctets = 0;
            for (i = 0; i < iLen; i++)
            {
                chr = (byte)sbInputStream.ReadByte();
                if ((chr & 0x80) != 0) bAllAscii = false;
                if (cOctets == 0)
                {
                    if (chr >= 0x80)
                    {
                        do
                        {
                            chr <<= 1;
                            cOctets++;
                        }
                        while ((chr & 0x80) != 0);
                        cOctets--;
                        if (cOctets == 0) return false;
                    }
                }
                else
                {
                    if ((chr & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    cOctets--;
                }
            }
            if (cOctets > 0)
            {
                return false;
            }
            if (bAllAscii)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool ObjToBool(object objValue, bool defValue)
        {
            if (objValue != null)
            {
                return StrToBool(objValue.ToString(), defValue);
            }
            return defValue;
        }
        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StrToBool(string strValue, bool defValue)
        {
            if (strValue != null)
            {
                if (string.Compare(strValue, "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(strValue, "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int ObjToInt(object objValue, int defValue)
        {
            if (objValue != null)
            {
                return StrToInt(objValue.ToString(), defValue);
            }
            return defValue;
        }
        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="str">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StrToInt(string strValue, int defValue)
        {
            if (strValue == null)
                return defValue;
            if (strValue.Length > 0 && strValue.Length <= 11 && Regex.IsMatch(strValue, @"^[-]?[0-9]*$"))
            {
                if ((strValue.Length < 10) || (strValue.Length == 10 && strValue[0] == '1') || (strValue.Length == 11 && strValue[0] == '-' && strValue[1] == '1'))
                {
                    return Convert.ToInt32(strValue);
                }
            }
            return defValue;
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float ObjToFloat(object objValue, float defValue)
        {
            if (objValue == null)
            {
                return defValue;
            }
            return StrToFloat(objValue.ToString(), defValue);
        }
        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StrToFloat(string strValue, float defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
            {
                return defValue;
            }
            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }
        public static decimal ObjToDecimal(object objValue, decimal defValue)
        {
            if (objValue != null)
                return StrToDecimal(objValue.ToString(), defValue);
            return defValue;
        }
        /// <summary>
        /// string型转换为decimal型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的decimal类型结果</returns>
        public static decimal StrToDecimal(string strValue, decimal defValue)
        {
            if ((strValue == null) || (strValue.Length > 10))
                return defValue;
            decimal intValue = defValue;
            if (strValue != null)
            {
                bool IsDecimal = Regex.IsMatch(strValue, @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsDecimal)
                    decimal.TryParse(strValue, out intValue);
            }
            return intValue;
        }
        /// <summary>
        /// 替换回车换行符为html换行符
        /// </summary>
        public static string StrToHtmlNewLines(string str)
        {
            string str2;
            if (str == null)
            {
                str2 = "";
            }
            else
            {
                str = str.Replace("\r\n", "<br />");
                str = str.Replace("\n", "<br />");
                str2 = str;
            }
            return str2;
        }
        /// <summary>
        /// 将long型数值转换为Int32类型
        /// </summary>
        /// <param name="objNum"></param>
        /// <returns></returns>
        public static int LongToInt(object objNum)
        {
            if (objNum == null)
            {
                return 0;
            }
            string strNum = objNum.ToString();
            if (IsNumeric(strNum))
            {
                if (strNum.ToString().Length > 9)
                {
                    if (strNum.StartsWith("-"))
                    {
                        return int.MinValue;
                    }
                    else
                    {
                        return int.MaxValue;
                    }
                }
                return int.Parse(strNum);
            }
            else
            {
                return 0;
            }
        }
        /// <summary>
        /// 将全角数字转换为数字(SBCCas为全角，DBCCase为半角)
        /// </summary>
        /// <param name="SBCCase"></param>
        /// <returns></returns>
        public static string SBCCaseToNumberic(string SBCCase)
        {
            char[] c = SBCCase.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                byte[] b = System.Text.Encoding.Unicode.GetBytes(c, i, 1);
                if (b.Length == 2)
                {
                    if (b[1] == 255)
                    {
                        b[0] = (byte)(b[0] + 32);
                        b[1] = 0;
                        c[i] = System.Text.Encoding.Unicode.GetChars(b)[0];
                    }
                }
            }
            return new string(c);
        }
        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>字符长度</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// 从字符串的指定位置截取指定长度的子字符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <param name="length">子字符串的长度</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex, int length)
        {
            if (startIndex >= 0)
            {
                if (length < 0)
                {
                    length = length * -1;
                    if (startIndex - length < 0)
                    {
                        length = startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        startIndex = startIndex - length;
                    }
                }
                if (startIndex > str.Length)
                {
                    return "";
                }
            }
            else
            {
                if (length < 0)
                {
                    return "";
                }
                else
                {
                    if (length + startIndex > 0)
                    {
                        length = length + startIndex;
                        startIndex = 0;
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            if (str.Length - startIndex < length)
            {
                length = str.Length - startIndex;
            }
            return str.Substring(startIndex, length);
        }
        /// <summary>
        /// 从字符串的指定位置开始截取到字符串结尾的了符串
        /// </summary>
        /// <param name="str">原字符串</param>
        /// <param name="startIndex">子字符串的起始位置</param>
        /// <returns>子字符串</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
        /// <summary>
        /// 截取取真实长度字符串(包括中文和字母)
        /// 编写日期：2013-09-30
        /// </summary>
        /// <param name="str"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CutLenString(string str, int length)
        {
            if (length <= 0)
            {
                return "";
            }
            int strLength = GetStringLength(str);
            if (length > strLength)
            {
                return str;
            }
            int lenCount=0;
            int lenIndex=0;
            for (int i = 0; i < str.Length; i++)
            {
                byte[] bt = Encoding.Default.GetBytes(str.Substring(i, 1));
                if (bt.Length == 2)
                {
                    lenCount = lenCount + 2;
                }
                else
                {
                    lenCount++;
                }
                if (lenCount >= length)
                {
                    lenIndex=i;
                    break;
                }
            }
            if (lenCount > strLength)
            {
                return str.Substring(0, lenIndex);
            }
            else
            {
                return str.Substring(0, lenIndex + 1);
            }
        }
        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num)
        {
            if (IsNullOrEmpty(Str))
                return "";
            string outstr = "";
            int n = 0;
            foreach (char ch in Str)
            {
                n += System.Text.Encoding.Default.GetByteCount(ch.ToString());
                if (n > Num)
                    break;
                else
                    outstr += ch;
            }
            return outstr;
        }
        /// <summary>
        /// 截取字符串函数
        /// </summary>
        /// <param name="Str">所要截取的字符串</param>
        /// <param name="Num">截取字符串的长度</param>
        /// <param name="LastStr">截取字符串后省略部分的字符串</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }
        /// <summary>
        /// 清理空格
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string ClearSpace(string strIn)
        {
            return Regex.Replace(strIn, @"\s+", "");
        }
        /// <summary>
        /// 清理字符串
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string ClearInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ClearLastChar(string str)
        {
            if (str == "")
                return "";
            else
                return str.Substring(0, str.Length - 1);
        }
        /// <summary>
        /// 根据阿拉伯数字返回月份的名称(可更改为某种语言)
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }
        /// <summary>
        /// 格式化字节数字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string FormatBytesStr(int bytes)
        {
            if (bytes > 1073741824)
            {
                return ((double)(bytes / 1073741824)).ToString("0") + "G";
            }
            if (bytes > 1048576)
            {
                return ((double)(bytes / 1048576)).ToString("0") + "M";
            }
            if (bytes > 1024)
            {
                return ((double)(bytes / 1024)).ToString("0") + "K";
            }
            return bytes.ToString() + "Bytes";
        }
        /// <summary>
        /// 为脚本替换特殊字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceStrToScript(string str)
        {
            str = str.Replace("\\", "\\\\");
            str = str.Replace("'", "\\'");
            str = str.Replace("\"", "\\\"");
            return str;
        }
        /// <summary>
        /// 生成指定长度的字符串,即生成strLong个str字符串
        /// </summary>
        /// <param name="strLong">生成的长度</param>
        /// <param name="str">以str生成字符串</param>
        /// <returns></returns>
        public static string StringOfChar(int strLong, string str)
        {
            string ReturnStr = "";
            for (int i = 0; i < strLong; i++)
            {
                ReturnStr += str;
            }
            return ReturnStr;
        }
        /// <summary>
        /// 判断字符串出现个数
        /// </summary>
        /// <param name="strSource">原字符</param>
        /// <param name="strArg">字符参数</param>
        /// <returns></returns>
        public static int GetCount(string strSource,string strArg)
        {
            return Regex.Matches(strSource, strArg).Count;
        }
        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (!Utils.IsNullOrEmpty(strContent))
            {
                if (strContent.IndexOf(strSplit) < 0)
                {
                    string[] tmp = { strContent };
                    return tmp;
                }
                return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
            }
            else
            {
                return new string[0] { };
            }
        }
        /// <summary>
        /// 清除字符串数组中的重复项
        /// </summary>
        /// <param name="data">字符串数组</param>
        /// <returns></returns>
        public static string[] DistinctStringArray(string[] data)
        {
            if (data.Length > 0)
            {
                Array.Sort(data);     
                int size = 1;           
                for (int i = 1; i < data.Length; i++)
                    if (data[i] != data[i - 1])
                        size++;
                String[] tempData = new String[size];
                int j = 0;
                tempData[j++] = data[0];
                for (int i = 1; i < data.Length; i++)
                    if (data[i] != data[i - 1])
                        tempData[j++] = data[i];
                return tempData;
            }
            return data;
        }
        /// <summary>
        /// 获取百分比(当前数目大于总数目则返回-1)
        /// </summary>
        /// <param name="num">当前数目</param>
        /// <param name="count">总数目</param>
        /// <returns></returns>
        public static int GetPercent(int num,int count)
        {
            if (num > count)
            {
                return -1;
            }
            return (int)(((double)num / count) * 100);
        }
        /// <summary>
        /// 返回标准日期格式string
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// 返回最小的日期格式string
        /// </summary>
        /// <returns></returns>
        public static string GetMinDate()
        {
            return "1900-01-01";
        }
        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 返回相对于当前时间的相对天数
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// 返回标准时间格式string
        /// </summary>
        public static string GetStandardDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
        /// <summary>
        /// 验证是否是时间格式string
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        /// <summary>
        /// 判断字符串是否是yyyy-mm-dd字符串
        /// </summary>
        /// <param name="str">待判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }
        /// <summary>
        /// 返回相差的秒数
        /// </summary>
        /// <param name="Time"></param>
        /// <param name="Sec"></param>
        /// <returns></returns>
        public static int StrDateDiffSeconds(string Time, int Sec)
        {
            TimeSpan ts = DateTime.Now - DateTime.Parse(Time).AddSeconds(Sec);
            if (ts.TotalSeconds > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalSeconds < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalSeconds;
        }
        /// <summary>
        /// 返回相差的分钟数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static int StrDateDiffMinutes(string time, int minutes)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddMinutes(minutes);
            if (ts.TotalMinutes > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalMinutes < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalMinutes;
        }
        /// <summary>
        /// 返回相差的小时数
        /// </summary>
        /// <param name="time"></param>
        /// <param name="hours"></param>
        /// <returns></returns>
        public static int StrDateDiffHours(string time, int hours)
        {
            if (time == "" || time == null)
                return 1;
            TimeSpan ts = DateTime.Now - DateTime.Parse(time).AddHours(hours);
            if (ts.TotalHours > int.MaxValue)
            {
                return int.MaxValue;
            }
            else if (ts.TotalHours < int.MinValue)
            {
                return int.MinValue;
            }
            return (int)ts.TotalHours;
        }
        /// <summary>
        /// 生成日期精确字符串码(yyyyMMddHHmmssffff)
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        /// <summary>
        /// 获取js数据类型的日期时间
        /// </summary>
        /// <returns></returns>
        public static string GetJsDateTimeString()
        {
            return DateTime.Now.ToString("ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy", CultureInfoDate);
        }
        /// <summary>
        /// 根据js数据类型字符串得到DateTime
        /// </summary>
        /// <param name="jsDateString"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string jsDateString)
        {
            return DateTime.ParseExact(jsDateString, "ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy", CultureInfoDate);
        }
        /// <summary>
        /// mysql数据Unix时间戳与C# DateTime时间类型互换
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTime(System.DateTime time)
        {
            return (long)(time - UnixStartTime).TotalSeconds;
        }

        /// <summary>
        /// 转换为静态html
        /// </summary>
        /// <param name="path">原路径</param>
        /// <param name="outpath">输出路径</param>
        public void TransHtml(string path, string outpath)
        {
            //Page page = new Page();
            //StringWriter writer = new StringWriter();
            //page.Server.Execute(path, writer);
            //FileStream fs;
            //if (File.Exists(page.Server.MapPath("") + "\\" + outpath))
            //{
            //    File.Delete(page.Server.MapPath("") + "\\" + outpath);
            //    fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            //}
            //else
            //{
            //    fs = File.Create(page.Server.MapPath("") + "\\" + outpath);
            //}
            //byte[] bt = Encoding.Default.GetBytes(writer.ToString());
            //fs.Write(bt, 0, bt.Length);
            //fs.Close();
        }
        ///// <summary>
        ///// 返回 HTML 字符串的编码结果
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <returns>编码结果</returns>
        //public static string HtmlEncode(string str)
        //{
        //    return HttpUtility.HtmlEncode(str);
        //}
        ///// <summary>
        ///// 返回 HTML 字符串的解码结果
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <returns>解码结果</returns>
        //public static string HtmlDecode(string str)
        //{
        //    return HttpUtility.HtmlDecode(str);
        //}
        ///// <summary>
        ///// 返回 URL 字符串的编码结果
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <returns>编码结果</returns>
        //public static string UrlEncode(string str)
        //{
        //    return HttpUtility.UrlEncode(str);
        //}
        ///// <summary>
        ///// 返回 URL 字符串的编码结果
        ///// </summary>
        ///// <param name="str">字符串</param>
        ///// <returns>解码结果</returns>
        //public static string UrlDecode(string str)
        //{
        //    return HttpUtility.UrlDecode(str);
        //}
        /// <summary>
        /// 生成指定数量的html空格符号
        /// </summary>
        public static string GetSpacesString(int spacesCount)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < spacesCount; i++)
            {
                sb.Append(" &nbsp;&nbsp;");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 移除Html标记
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(script|frame|form|meta|behavior|style)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <param name="tags">applet|body|embed|frame|script|frameset|html|iframe|object|meta|style|layer|link|ilayer|img</param>
        /// <returns></returns>
        public static string RemoveUnsafeHtml(string content,string tags)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, "(" + tags + @")([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 过滤HTML中的不安全标签
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveAllUnsafeHtml(string content)
        {
            content = Regex.Replace(content, @"(\<|\s+)o([a-z]+\s?=)", "$1$2", RegexOptions.IgnoreCase);
            content = Regex.Replace(content, @"(applet|body|embed|frame|script|frameset|html|iframe|object|meta|style|layer|link|ilayer|img)([\s|:|>])+", "$1.$2", RegexOptions.IgnoreCase);
            return content;
        }
        /// <summary>
        /// 从HTML中获取文本,保留br,p,img
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string GetTextFromHtml(string Html)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regEx.Replace(Html, "");
        }
        /// <summary>
        /// 从HTML中获取class去除style
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string GetCssHtml(string Html)
        {
            StringBuilder sbStyle = new StringBuilder();
            string regstr = @"(<\S+\s+(?<attributes>[^\]]*?)>)";
            Regex rt = new Regex(regstr, RegexOptions.None);
            Dictionary<string, string> dicStyle = new Dictionary<string, string>();
            MatchCollection matches = rt.Matches(Html);
            if (matches != null)
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    string tag = matches[i].Groups[0].Value.ToString();
                    rt = new Regex(@"(?<Key>\w+)\s*=\s*(?<Value>" + "\"[^\"]*\"|'[^']*')", RegexOptions.None);
                    MatchCollection mts = rt.Matches(tag);
                    if (mts != null)
                    {
                        string sStyle = "";
                        string sClass = "";
                        for (int j = 0; j < mts.Count; j++)
                        {
                            Match mt = mts[j];
                            if (mt.Groups["Key"].ToString().Trim().ToLower() == "style")
                            {
                                sStyle = mt.Groups["Value"].ToString().Trim('\"');
                            }
                            if (mt.Groups["Key"].ToString().Trim().ToLower() == "class")
                            {
                                string tClass = mt.Groups["Value"].ToString().Trim('\"').Trim('\'').Trim();
                                if (tClass.IndexOf(' ') > 0)
                                {
                                    break;
                                }
                                sClass = "." + tClass;
                            }
                        }
                        if (sClass != "" && sStyle != "")
                        {
                            if (!dicStyle.ContainsKey(sClass))
                            {
                                dicStyle.Add(sClass, sStyle);
                            }
                        }
                    }
                }
                sbStyle.AppendLine("<style type=\"text/css\">");
                foreach (System.Collections.Generic.KeyValuePair<string, string> kv in dicStyle)
                {
                    sbStyle.AppendLine(kv.Key + "{" + kv.Value + "}");
                }
                sbStyle.AppendLine("</style>");
                rt = new Regex(@"\s*style\s*=\s*(" + "\"[^\"]*\"" + @"|'[^']*')\s*", RegexOptions.None);
                return sbStyle.ToString() + rt.Replace(Html, "");
            }
            return Html;
        }
        /// <summary>
        /// 压缩Html文件
        /// </summary>
        /// <param name="Html">Html文件</param>
        /// <returns></returns>
        public static string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");//去除Html中的空白字符.
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }
       /// <summary>
        /// 匹配页面的链接
       /// </summary>
       /// <param name="HtmlCode">html源码</param>
       /// <returns></returns>
        public static string GetHref(string HtmlCode)
        {
            string MatchVale = "";
            string Reg = @"(h|H)(r|R)(e|E)(f|F) *= *('|"")?((\w|\\|\/|\.|:|-|_)+)[\S]*";
            foreach (Match m in Regex.Matches(HtmlCode, Reg))
            {
                MatchVale += (m.Value).ToLower().Replace("href=", "").Trim() + "|";
            }
            return MatchVale;
        }
        /// <summary>
        /// 匹配页面的图片地址(多个以'|'分割)
        /// </summary>
        /// <param name="HtmlCode">html源码</param>
        /// <returns></returns>
        public static string GetImgSrc(string HtmlCode)
        {
            string MatchVale = "";
            string Reg = @"<img.+?>";
            foreach (Match m in Regex.Matches(HtmlCode.ToLower(), Reg))
            {
                MatchVale += GetImg((m.Value).ToLower().Trim()) + "|";
            }
            return MatchVale;
        }
        private static string GetImg(string ImgString)
        {
            string MatchVale = "";
            string Reg = @"src=.+\.(bmp|jpg|gif|png|)";
            foreach (Match m in Regex.Matches(ImgString.ToLower(), Reg))
            {
                MatchVale += (m.Value).ToLower().Trim().Replace("src=", "").Trim('\'').Trim('\"');
            }
            return (MatchVale);
        }

        /// <summary>
        /// 创建文件(核心函数)
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="text">内容</param>
        public static bool CreateFile(string filePath, string text,Encoding tmpEncoding)
        {
            try
            {
                StreamWriter sw = new StreamWriter(filePath, false, tmpEncoding);
                sw.WriteLine(text);
                sw.Flush();
                sw.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <param name="text">内容</param>
        public static bool CreateFile(string filePath, string text)
        {
            return CreateFile(filePath, text, Encoding.Default);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath">路径</param>
        public static bool DeleteFile(string filePath)
        {
            try
            {
                File.Delete(filePath);
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 读取外部文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            return ReadFile(path, System.Text.Encoding.Default);
        }
        /// <summary>
        /// 读取外部文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string ReadFile(string path, System.Text.Encoding encode)
        {
            StringBuilder html = new StringBuilder();
            try
            {
                using (StreamReader reader = new StreamReader(path, encode))
                {
                    while (reader.Peek() >= 0)
                    {
                        html.Append(((char)reader.Read()).ToString());
                    }
                }
            }
            catch { return null; }
            return html.ToString();
        }
        /// <summary>
        /// 读取外部文件(去除文件前面数字)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="tCount">去除字符长度</param>
        public static string ReadFile(string path, int tCount)
        {
            return ReadFile(path, tCount,System.Text.Encoding.Default);    // Encoding.GetEncoding("gb2312"));
        }
        /// <summary>
        /// 读取外部文件(去除文件前面数字)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="tCount">去除字符长度</param>
        /// <param name="encode">编码方式</param>
        public static string ReadFile(string path, int tCount, System.Text.Encoding encode)
        {
            Encoding code = encode;
            StreamReader sr = null;
            StringBuilder sb = new StringBuilder();
            string temppath = path;    
            try
            {
                sr = new StreamReader(temppath, code);
                sb.Append("<pre>");
                while (sr.Peek() >= 0)
                {
                    string strMbContect = sr.ReadLine();
                    if (strMbContect.Length > tCount)
                    {
                        strMbContect = strMbContect.Remove(0, tCount);
                    }
                    else
                    {
                        strMbContect = "";
                    }
                    sb.Append("<br>" + strMbContect);
                }
                sb.Append("</pre>");
            }
            catch
            {
                if (sr != null)
                {
                    sr.Close();
                }
                sb.Remove(0, sb.Length);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 读取外部文件(一次性读取)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static string ReadFileToEnd(string path)
        {
            return ReadFileToEnd(path, System.Text.Encoding.Default);
        }
        /// <summary>
        /// 读取外部文件(一次性读取)
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encode">编码方式</param>
        /// <returns></returns>
        public static string ReadFileToEnd(string path, System.Text.Encoding encode)
        {
            try
            {
                StreamReader reader = new StreamReader(path, encode);
                string str = reader.ReadToEnd();
                reader.Close();
                return str;
            }
            catch { return null; }
        }
        /// <summary>
        /// 备份文件
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <param name="overwrite">当目标文件存在时是否覆盖</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "文件不存在！");
            }
            if (!overwrite && System.IO.File.Exists(destFileName))
            {
                return false;
            }
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, true);
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// 备份文件,当目标文件存在时覆盖
        /// </summary>
        /// <param name="sourceFileName">源文件名</param>
        /// <param name="destFileName">目标文件名</param>
        /// <returns>操作是否成功</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }
        /// <summary>
        /// 恢复文件
        /// </summary>
        /// <param name="backupFileName">备份文件名</param>
        /// <param name="targetFileName">要恢复的文件名</param>
        /// <param name="backupTargetFileName">要恢复文件再次备份的名称,如果为null,则不再备份恢复文件</param>
        /// <returns>操作是否成功</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "文件不存在！");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "文件不存在！无法备份此文件！");
                    }
                    else
                    {
                        System.IO.File.Copy(targetFileName, backupTargetFileName, true);
                    }
                }
                System.IO.File.Delete(targetFileName);
                System.IO.File.Copy(backupFileName, targetFileName);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
        public static bool RestoreFile(string backupFileName, string targetFileName)
        {
            return RestoreFile(backupFileName, targetFileName, null);
        }
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return MakeSureDirectoryPathExists(name);
        }
        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);
        /// <summary>
        /// 返回路径中的文件名
        /// </summary>		
        public static string GetFileName(string filename)
        {
            if (IsNullOrEmpty(filename))
            {
                return "";
            }
            string reName="";
            if (filename.IndexOf("/")>=0)
            {
                string []str = filename.Split(new char[] { '/' });
                reName=str[str.Length-1];
            }
            else
            {
                if (filename.IndexOf("\\")>=0)
                {
                    string []str = filename.Split(new char[] { '\\' });
                    reName=str[str.Length-1];
                }
            }
            return reName;
        } 
        /// <summary>
        /// 获取文件扩展名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string GetFileExtName(string filename)
        {
            string[] array = filename.Trim().Split('.');
            Array.Reverse(array);
            return array[0].ToString();
        }
        /// <summary>
        /// 返回指定目录下的非 UTF8 字符集文件
        /// </summary>
        /// <param name="Path">路径</param>
        /// <returns>文件名的字符串数组</returns>
        public static string[] FindNoUTF8File(string Path)
        {
            StringBuilder filelist = new StringBuilder();
            DirectoryInfo Folder = new DirectoryInfo(Path);
            FileInfo[] subFiles = Folder.GetFiles();
            for (int j = 0; j < subFiles.Length; j++)
            {
                if (subFiles[j].Extension.ToLower().Equals(".htm"))
                {
                    FileStream fs = new FileStream(subFiles[j].FullName, FileMode.Open, FileAccess.Read);
                    bool bUtf8 = IsUTF8(fs);
                    fs.Close();
                    if (!bUtf8)
                    {
                        filelist.Append(subFiles[j].FullName);
                        filelist.Append("\r\n");
                    }
                }
            }
            return Utils.SplitString(filelist.ToString(), "\r\n");
        }
        /// <summary>
        /// 返回URL中结尾的文件名
        /// </summary>		
        public static string GetUrlFileName(string url)
        {
            if (IsNullOrEmpty(url))
            {
                return "";
            }
            string[] strs1 = url.Split(new char[] { '/' });
            return strs1[strs1.Length - 1].Split(new char[] { '?' })[0];
        }        
        ///// <summary>
        ///// (Url操作)获得当前绝对路径
        ///// </summary>
        ///// <param name="strPath">指定的路径</param>
        ///// <returns>绝对路径</returns>
        //public static string GetMapPath(string strPath)
        //{
        //    if (HttpContext.Current != null)
        //    {
        //        return HttpContext.Current.Server.MapPath(strPath);
        //    }
        //    else 
        //    {
        //        strPath = strPath.Replace("/", "\\");
        //        if (strPath.StartsWith("\\"))
        //        {
        //            strPath = strPath.Substring(strPath.IndexOf('\\', 1)).TrimStart('\\');
        //        }
        //        return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, strPath);
        //    }
        //}
        ///// <summary>
        ///// (Url操作)返回应用程序根目录的URL
        ///// </summary>
        ///// <returns></returns>
        //public static string GetAppPath()
        //{
        //    string AppPath = System.Web.HttpContext.Current.Request.ApplicationPath.Trim();
        //    if (AppPath.Length > 1)
        //    {
        //        AppPath = AppPath + "/";
        //    }
        //    return ("http://" + System.Web.HttpContext.Current.Request.Url.Authority + AppPath);
        //}
        /// <summary>
        /// (Url操作)获得当前绝对路径
        /// </summary>
        /// <param name="oldSrc">原相对路径</param>
        /// <param name="httpUrl">当前也路径</param>
        /// <returns></returns>
        public static string GetMapSrc(string oldSrc, string httpUrl)
        {
            oldSrc = oldSrc.Trim().ToLower();
            httpUrl = httpUrl.Trim().ToLower();
            string http = "http://";
            string rtUrl = "";
            int tFlag = httpUrl.LastIndexOf('/');
            if (tFlag > 0)
            {
                httpUrl = httpUrl.Substring(0, tFlag);
            }
            if (oldSrc.StartsWith(http))
            {
                rtUrl = oldSrc;
            }
            else
            {
                if (oldSrc.StartsWith("/"))
                {
                    if (httpUrl.StartsWith(http))
                    {
                        httpUrl = httpUrl.Replace(http, "");
                    }
                    int iFlag = httpUrl.IndexOf('/');
                    if (iFlag > 0)
                    {
                        httpUrl = httpUrl.Substring(0, iFlag);
                    }
                    rtUrl = http + httpUrl + oldSrc;
                }
                else if (oldSrc.StartsWith("./"))
                {
                    rtUrl = httpUrl + oldSrc.Trim('.');
                }
                else
                {
                    while (oldSrc.StartsWith(".."))
                    {
                        oldSrc = oldSrc.Trim('.').Trim('/');
                        int tCount = httpUrl.LastIndexOf('/');
                        if (tCount > 0)
                        {
                            httpUrl = httpUrl.Substring(0, tCount);
                        }
                    }
                    rtUrl = httpUrl + "/" + oldSrc;
                }
            }
            return rtUrl;
        }
        /// <summary>
        /// 获取Url参数值
        /// </summary>
        /// <param name="strHref">链接地址</param>
        /// <param name="strName">参数名</param>
        /// <returns></returns>
        public static string GetParam(string strHref, string strName)
        {
            int intPos = strHref.IndexOf("?");
            if (intPos < 0)
            {
                return "";
            }
            string strRight = strHref.Substring(intPos + 1);
            string[] arrPram = Utils.SplitString(strRight, "&");
            for (int i = 0; i < arrPram.Length; i++)
            {
                string[] arrPramName = Utils.SplitString(arrPram[i], "=");
                if (arrPramName[0].ToLower() == strName.ToLower())
                {
                    return arrPramName[1];
                }
            }
            return "";
        }
        /// <summary>
        /// 将字符串转换为Color
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Color ToColor(string color)
        {
            int red, green, blue = 0;
            char[] rgb;
            color = color.TrimStart('#');
            color = Regex.Replace(color.ToLower(), "[g-zG-Z]", "");
            switch (color.Length)
            {
                case 3:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[0].ToString(), 16);
                    green = Convert.ToInt32(rgb[1].ToString() + rgb[1].ToString(), 16);
                    blue = Convert.ToInt32(rgb[2].ToString() + rgb[2].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                case 6:
                    rgb = color.ToCharArray();
                    red = Convert.ToInt32(rgb[0].ToString() + rgb[1].ToString(), 16);
                    green = Convert.ToInt32(rgb[2].ToString() + rgb[3].ToString(), 16);
                    blue = Convert.ToInt32(rgb[4].ToString() + rgb[5].ToString(), 16);
                    return Color.FromArgb(red, green, blue);
                default:
                    return Color.FromName(color);
            }
        }
        /// <summary>
        /// 检查颜色值是否为3/6位的合法颜色
        /// </summary>
        /// <param name="color">待检查的颜色</param>
        /// <returns></returns>
        public static bool CheckColorValue(string color)
        {
            if (IsNullOrEmpty(color))
            {
                return false;
            }
            color = color.Trim().Trim('#');
            if (color.Length != 3 && color.Length != 6)
            {
                return false;
            }
            if (!Regex.IsMatch(color, "[^0-9a-f]", RegexOptions.IgnoreCase))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 根据Url获得源文件内容
        /// </summary>
        /// <param name="url">合法的Url地址</param>
        /// <returns></returns>
        public static string GetTextByUrl(string url)
        {
            string responseStr = "";
            WebRequest request = null;
            WebResponse response = null;
            StreamReader reader = null;
            try
            {
                request = WebRequest.Create(url);
                request.Timeout = 20000;
                response = request.GetResponse();
                reader = new StreamReader(response.GetResponseStream());
                responseStr = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                request = null;
                reader = null;
                response = null;
            }
            return responseStr;
        }
        /// <summary>
        /// HTTP POST方式请求数据
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST的数据</param>
        /// <returns></returns>
        public static string HttpPost(string url, string param)
        {
            string responseStr = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            StreamWriter requestStream = null;
            StreamReader reader = null;
            WebResponse response = null;
            
            try
            {
                requestStream = new StreamWriter(request.GetRequestStream());
                requestStream.Write(param);
                requestStream.Close();
                response = request.GetResponse();
                if (response != null)
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch 
            {
                throw;
            }
            finally
            {
                request = null;
                requestStream = null;
                reader = null;
                response = null;
            }
            return responseStr;
        }
        /// <summary>
        /// HTTP GET方式请求数据.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <returns></returns>
        public static string HttpGet(string url)
        {
            string responseStr = "";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "*/*";
            request.Timeout = 20000;
            request.AllowAutoRedirect = false;
            StreamReader reader = null;
            WebResponse response = null;
            
            try
            {
                response = request.GetResponse();
                if (response != null)
                {
                    reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    responseStr = reader.ReadToEnd();
                    reader.Close();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                request = null;
                reader = null;
                response = null;
            }
            return responseStr;
        }
        
        
        /// <summary>
        /// 返回要转换的汉字字符串-拼音缩写
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetPinYinString(string str)
        {
            string tempStr = "";
            foreach (char c in str)
            {
                if ((int)c >= 33 && (int)c <= 126)
                {
                    tempStr += c.ToString();
                }
                else
                {
                    tempStr += GetPinYinChar(c.ToString());
                }
            }
            return tempStr;
        }
        /// <summary>
        /// 返回要转换的单个汉字-拼音声母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private static string GetPinYinChar(string c)
        {
            byte[] array = new byte[2];
            array = System.Text.Encoding.Default.GetBytes(c);
            int i = (short)(array[0] - '\0') * 256 + ((short)(array[1] - '\0'));
            if (i < 0xB0A1) return "*";
            if (i < 0xB0C5) return "a";
            if (i < 0xB2C1) return "b";
            if (i < 0xB4EE) return "c";
            if (i < 0xB6EA) return "d";
            if (i < 0xB7A2) return "e";
            if (i < 0xB8C1) return "f";
            if (i < 0xB9FE) return "g";
            if (i < 0xBBF7) return "h";
            if (i < 0xBFA6) return "g";
            if (i < 0xC0AC) return "k";
            if (i < 0xC2E8) return "l";
            if (i < 0xC4C3) return "m";
            if (i < 0xC5B6) return "n";
            if (i < 0xC5BE) return "o";
            if (i < 0xC6DA) return "p";
            if (i < 0xC8BB) return "q";
            if (i < 0xC8F6) return "r";
            if (i < 0xCBFA) return "s";
            if (i < 0xCDDA) return "t";
            if (i < 0xCEF4) return "w";
            if (i < 0xD1B9) return "x";
            if (i < 0xD4D1) return "y";
            if (i < 0xD7FA) return "z";
            return "*";
        }        
        /// <summary>
        /// 序列化对象到文件
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        public static void Serialiaze(string filepath,object obj, Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            Stream stream = new FileStream(filepath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            xs.Serialize(stream,obj);
            stream.Close();
        }
        /// <summary>
        /// 从文件反序列化到对象
        /// </summary>
        /// <param name="filepath"></param>
        /// <param name="type"></param>
        public static object Deserialize(string filepath,Type type)
        {
            XmlSerializer xs = new XmlSerializer(type);
            Stream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            object obj = xs.Deserialize(stream);
            stream.Close();
            return obj;
        }
        ///  <summary>
        ///  将目标对象序列化成XML文档(utf-16)
        ///  </summary>
        ///  <param  name="obj">对象</param>
        ///  <param  name="type">对象类型</param>
        ///  <returns></returns>
        public static string SerialiazeText(object obj, Type type)
        {
            StringWriter writer = new StringWriter();
            new XmlSerializer(type).Serialize((TextWriter)writer, obj);
            StringBuilder sb = writer.GetStringBuilder();
            writer.Close();
            writer.Dispose();
            return sb.ToString();
        }
        ///  <summary>
        ///  将字符串内容反序列化成对象(utf-16)
        ///  </summary>
        ///  <param  name="txt">被序列化的文本</param>
        ///  <param  name="type">对象类型</param>
        ///  <returns></returns>
        public static object DeserializeText(string txt, Type type)
        {
            string tmpstr = "<?xml version=\"1.0\" encoding=\"utf-16\"?>";
            object obj = null;
            txt = txt.Trim();
            if (txt.StartsWith(tmpstr) && txt != tmpstr)
            {
                StringReader reader = new StringReader(txt);
                obj = new XmlSerializer(type).Deserialize(reader);
                reader.Close();
                reader.Dispose();
            }
            return obj;
        }
        /// <summary>
        /// 对象序列化成 xml string
        /// </summary>
        public static string Serialiaze<T>(T obj)
        {
            string xmlString = string.Empty;
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                xmlserializer.Serialize(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
            }
            return xmlString;
        }
        /// <summary>
        /// 反序列化成对象 xml string 
        /// </summary>
        public static T Deserialize<T>(string xmlString)
        {
            T type = default(T);
            XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
            using (Stream xmlstream = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                using (XmlReader xmlreader = XmlReader.Create(xmlstream))
                {
                    object obj = xmlserializer.Deserialize(xmlreader);
                    type = (T)obj;
                }
            }
            return type;
        }
        /// <summary>
        /// 对象序列化成(二进制)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeText<T>(T obj)
        {
            System.Runtime.Serialization.IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            string result = string.Empty;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bf.Serialize(ms, obj);
                byte[] byt = new byte[ms.Length];
                byt = ms.ToArray();
                result = System.Convert.ToBase64String(byt);
                ms.Flush();
            }
            return result;
        }
        /// <summary>
        ///  反序列化成对象(二进制)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeText<T>(string str)
        {
            T obj;
            System.Runtime.Serialization.IFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            byte[] byt = Convert.FromBase64String(str);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(byt, 0, byt.Length))
            {
                obj = (T)bf.Deserialize(ms);
            }
            return obj;
        } 
        /// <summary>
        /// 根据字符串获取属性值
        /// </summary>
        /// <param name="tmptype">类型名</param>
        /// <param name="proname">属性名</param>
        /// <returns></returns>
        public static string GetPropertyValue(Type tmptype, string proname)
        {
            try
            {
                return tmptype.GetProperty(proname, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic).GetValue(null, null).ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 根据字符串获取属性值集合
        /// </summary>
        /// <param name="tmptype"></param>
        /// <returns></returns>
        public static string[] GetPropertyValues(Type tmptype)
        {
            string[] prostrings = null;
            try
            {
                PropertyInfo[] proinfos = tmptype.GetProperties(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic);
                if (proinfos.Length > 0)
                {
                    int fcount = proinfos.Length;
                    prostrings = new string[fcount];
                    for (int i = 0; i < fcount; i++)
                    {
                        prostrings[i] = proinfos[i].GetValue(null,null).ToString();
                    }
                }
            }
            catch { }
            return prostrings;
        }
        /// <summary>
        /// 根据字符串获取字段值
        /// </summary>
        /// <param name="tmptype"></param>
        /// <param name="fiename"></param>
        /// <returns></returns>
        public static string GetFieldValue(Type tmptype, string fiename)
        {
            try
            {
                return tmptype.GetField(fiename, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic).GetValue(null).ToString();
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// 根据字符串获取字段值集合
        /// </summary>
        /// <param name="tmptype"></param>
        /// <returns></returns>
        public static string[] GetFieldValues(Type tmptype)
        {
            string[] fieldstrings = null;
            try
            {
                FieldInfo[] fieldinfos = tmptype.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.GetField | BindingFlags.NonPublic);
                if (fieldinfos.Length > 0)
                {
                    int fcount = fieldinfos.Length;
                    fieldstrings = new string[fcount];
                    for (int i = 0; i < fcount; i++)
                    {
                        fieldstrings[i] = fieldinfos[i].GetValue(null).ToString();
                    }
                }
            }
            catch { }
            return fieldstrings;
        }
        /// <summary>
        /// 得到该进程内存的大小(单位兆)
        /// </summary>
        /// <returns></returns>
        public static double GetMemorySize()
        {
            return (Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576;
        }
        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyVersion(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.FileVersion;
        }
        /// <summary>
        /// 获得Assembly版本号
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string GetAssemblyVersion(string assemblyPath, int count)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            switch (count)
            {
                case 1:
                    return AssemblyFileVersion.FileMajorPart.ToString();
                case 2:
                    return string.Format("{0}.{1}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart);
                case 3:
                    return string.Format("{0}.{1}.{2}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart);
                default:
                    return string.Format("{0}.{1}.{2}.{3}", AssemblyFileVersion.FileMajorPart, AssemblyFileVersion.FileMinorPart, AssemblyFileVersion.FileBuildPart, AssemblyFileVersion.FilePrivatePart);
            }
        }
        /// <summary>
        /// 获得Assembly产品名称
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyProductName(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.ProductName;
        }
        /// <summary>
        /// 获得Assembly产品版权
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyCopyright(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.LegalCopyright;
        }
        /// <summary>
        /// 反射得到实例
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类名</param>
        /// <returns></returns>
        public static object GetAssemblyInstance(string assemblyName, string className)
        {
            try
            {
                Assembly assembly = Assembly.Load(assemblyName);
                return assembly.CreateInstance(assemblyName + "." + className);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 反射调用插件方法(ExecAssemblyMethod("PlugNT.App.Bbs", "Section", "RemoveTopicCount", sysModelId, channelId , isToday ))
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <param name="className">类名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="objParas">参数</param>
        /// <returns>object</returns>
        public static object ExecAssemblyMethod(string assemblyName, string className, string methodName, params object[] objParas)
        {
            try
            {
                Assembly assembly = Assembly.Load(assemblyName);
                object obj = assembly.CreateInstance(assemblyName + "." + className);
                MethodInfo methodInfo = obj.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public);
                FastInvokeHandler fastInvoker = FastInvoke.GetMethodInvoker(methodInfo);
                return fastInvoker(obj, objParas);
            }
            catch
            {
                return null;
            }
        }
        /// <summary>
        /// 简单加密字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EnStr(string s)
        {
            Random rnd = new Random();
            string z = "abcdefghigklmnopqrstuvwxyz";
            byte[] b = System.Text.Encoding.Default.GetBytes(s);
            StringBuilder sb = new StringBuilder();
            foreach (byte tb in b)
            {
                sb.Append(tb.ToString() +  z[rnd.Next(0, 25)].ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 简单解密字符串
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DeStr(string s)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                if (Char.IsNumber(c))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('-');
                }
            }
            string[] ts = sb.ToString().Split('-');
            int tCount = ts.Length;
            byte[] b = new byte[tCount];
            for (int i = 0; i < tCount - 1; i++)
            {
                b[i] = Byte.Parse(ts[i]);
            }
            return System.Text.Encoding.Default.GetString(b);
        }
        public static void DebugWrite(string filepath, string message)
        {
            StreamWriter sr;
            if (File.Exists(filepath))
            {
                sr = File.AppendText(filepath);
            }
            else
            {
                sr = File.CreateText(filepath);
            }
            sr.WriteLine("\n");
            sr.WriteLine(DateTime.Now.ToString() + "：" + message);
            sr.Close();
        }
        public static string Help()
        {
            return @"mailto:354990393@qq.com";
        }

        #region 定义文章标题字段长度2
        /// <summary>
        /// 定义文章标题字段长度，无...符号
        /// </summary>
        /// <param name="str">需要定义的文字内容</param>
        /// <param name="maxstr">最大显示长度</param>
        /// <param name="laststr">实际显示长度</param>
        /// <returns>返回字符串 + "..."</returns>
        public static string FormatLeft2(string str, int maxstr, int laststr)
        {
            if (str.Length > maxstr)
            {
                return str.Substring(0, laststr);
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region 定义文章标题字段长度
        /// <summary>
        /// 定义文章标题字段长度
        /// </summary>
        /// <param name="str">需要定义的文字内容</param>
        /// <param name="maxstr">最大显示长度</param>
        /// <param name="laststr">实际显示长度</param>
        /// <returns>返回字符串 + "..."</returns>
        public static string FormatLeft(string str, int maxstr, int laststr)
        {
            if (str.Length > maxstr)
            {
                return str.Substring(0, laststr) + "...";
            }
            else
            {
                return str;
            }
        }
        #endregion

        #region 格式化文本（输出内容）
        /// <summary>
        /// 格式化文本（输出内容）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string MyFormatDestr(string str)
        {
            str = str.Replace("  ", " &nbsp;");
            str = str.Replace("\"", "&quot;");
            str = str.Replace("\'", "&#39;");
            str = str.Replace("\n", "<br/> ");
            str = str.Replace("\r\n", "<br/> ");
            return str;
        }
        #endregion
    } 
}

