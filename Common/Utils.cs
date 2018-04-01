/**********************************************************************************
 * ����˵����     ��������(�ַ������������ַ�����֤���ļ��������������)��
 * �������ڣ�     2009.10.20
 * ����������     agui(��������discuz!)
 * ��ϵ��ʽ��     mailto:354990393@qq.com  
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
    /// ��������(�ַ������������ַ�����֤���ļ��������������)��
    /// </summary>
    public abstract class Utils
    {
        
        private static DateTime UnixStartTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        private static System.Globalization.CultureInfo CultureInfoDate = System.Globalization.CultureInfo.CreateSpecificCulture("en-US");
        /// <summary>
        /// �õ���������������
        /// </summary>
        /// <returns>��������</returns>
        public static RegexOptions GetRegexCompiledOptions()
        {
            return RegexOptions.None;
        }
        /// <summary>
        /// �ж϶����Ƿ�ΪInt32���͵�����
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
        /// �Ƿ�ΪDouble����
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
        /// �жϸ������ַ�������(strNumber)�е������ǲ��Ƕ�Ϊ��ֵ��
        /// </summary>
        /// <param name="strNumber">Ҫȷ�ϵ��ַ�������</param>
        /// <returns>���򷵼�true �����򷵻� false</returns>
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
        /// �ж��ļ����Ƿ�Ϊ���������ֱ����ʾ��ͼƬ�ļ���
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <returns>�Ƿ����ֱ����ʾ</returns>
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
        /// ����Ƿ����email��ʽ
        /// </summary>
        /// <param name="strEmail">Ҫ�жϵ�email�ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsValidEmail(string strEmail)
        {
            return Regex.IsMatch(strEmail, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }
        /// <summary>
        /// ����Ƿ�����ȷ��Url
        /// </summary>
        /// <param name="strUrl">Ҫ��֤��Url</param>
        /// <returns>�жϽ��</returns>
        public static bool IsUrl(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }
        /// <summary>
        /// �ж��Ƿ�Ϊbase64�ַ���
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsBase64String(string str)
        {
            return Regex.IsMatch(str, @"[A-Za-z0-9\+\/\=]");    //A-Z, a-z, 0-9, +, /, =
        }
        /// <summary>
        /// ����Ƿ���SqlΣ���ַ�
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsSafeSqlString(string str)
        {
            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }
        /// <summary>
        /// ����Ƿ���Σ�յĿ����������ӵ��ַ���
        /// </summary>
        /// <param name="str">Ҫ�ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsSafeLinkString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|�ο�|^Guest");
        }
        /// <summary>
        /// �ֶδ��Ƿ�ΪNull��Ϊ""(��)
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
        /// �Ƿ�Ϊ��ֵ���б�����ֵ����","���
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
        /// �Ƿ�Ϊip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIP(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");
        }
        /// <summary>
        /// �Ƿ�ΪipƬ��
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPSect(string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){2}((2[0-4]\d|25[0-5]|[01]?\d\d?|\*)\.)(2[0-4]\d|25[0-5]|[01]?\d\d?|\*)$");
        }
        /// <summary>
        /// ����ָ��IP�Ƿ���ָ����IP�������޶��ķ�Χ��, IP�����ڵ�IP��ַ����ʹ��*��ʾ��IP������, ����192.168.1.*
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
        /// �ж��ļ����Ƿ�ΪUTF8�ַ���
        /// </summary>
        /// <param name="sbInputStream">�ļ���</param>
        /// <returns>�жϽ��</returns>
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
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
        public static bool ObjToBool(object objValue, bool defValue)
        {
            if (objValue != null)
            {
                return StrToBool(objValue.ToString(), defValue);
            }
            return defValue;
        }
        /// <summary>
        /// string��ת��Ϊbool��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����bool���ͽ��</returns>
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
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static int ObjToInt(object objValue, int defValue)
        {
            if (objValue != null)
            {
                return StrToInt(objValue.ToString(), defValue);
            }
            return defValue;
        }
        /// <summary>
        /// ������ת��ΪInt32����
        /// </summary>
        /// <param name="str">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
        public static float ObjToFloat(object objValue, float defValue)
        {
            if (objValue == null)
            {
                return defValue;
            }
            return StrToFloat(objValue.ToString(), defValue);
        }
        /// <summary>
        /// string��ת��Ϊfloat��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����int���ͽ��</returns>
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
        /// string��ת��Ϊdecimal��
        /// </summary>
        /// <param name="strValue">Ҫת�����ַ���</param>
        /// <param name="defValue">ȱʡֵ</param>
        /// <returns>ת�����decimal���ͽ��</returns>
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
        /// �滻�س����з�Ϊhtml���з�
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
        /// ��long����ֵת��ΪInt32����
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
        /// ��ȫ������ת��Ϊ����(SBCCasΪȫ�ǣ�DBCCaseΪ���)
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
        /// �����ַ�����ʵ����, 1�����ֳ���Ϊ2
        /// </summary>
        /// <returns>�ַ�����</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }
        /// <summary>
        /// ���ַ�����ָ��λ�ý�ȡָ�����ȵ����ַ���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <param name="length">���ַ����ĳ���</param>
        /// <returns>���ַ���</returns>
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
        /// ���ַ�����ָ��λ�ÿ�ʼ��ȡ���ַ�����β���˷���
        /// </summary>
        /// <param name="str">ԭ�ַ���</param>
        /// <param name="startIndex">���ַ�������ʼλ��</param>
        /// <returns>���ַ���</returns>
        public static string CutString(string str, int startIndex)
        {
            return CutString(str, startIndex, str.Length);
        }
        /// <summary>
        /// ��ȡȡ��ʵ�����ַ���(�������ĺ���ĸ)
        /// ��д���ڣ�2013-09-30
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
        /// ��ȡ�ַ�������
        /// </summary>
        /// <param name="Str">��Ҫ��ȡ���ַ���</param>
        /// <param name="Num">��ȡ�ַ����ĳ���</param>
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
        /// ��ȡ�ַ�������
        /// </summary>
        /// <param name="Str">��Ҫ��ȡ���ַ���</param>
        /// <param name="Num">��ȡ�ַ����ĳ���</param>
        /// <param name="LastStr">��ȡ�ַ�����ʡ�Բ��ֵ��ַ���</param>
        /// <returns></returns>
        public static string GetSubString(string Str, int Num, string LastStr)
        {
            return (Str.Length > Num) ? Str.Substring(0, Num) + LastStr : Str;
        }
        /// <summary>
        /// ����ո�
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string ClearSpace(string strIn)
        {
            return Regex.Replace(strIn, @"\s+", "");
        }
        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public static string ClearInput(string strIn)
        {
            return Regex.Replace(strIn.Trim(), @"[^\w\.@-]", "");
        }
        /// <summary>
        /// ɾ�����һ���ַ�
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
        /// ���ݰ��������ַ����·ݵ�����(�ɸ���Ϊĳ������)
        /// </summary>	
        public static string[] Monthes
        {
            get
            {
                return new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            }
        }
        /// <summary>
        /// ��ʽ���ֽ����ַ���
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
        /// Ϊ�ű��滻�����ַ���
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
        /// ����ָ�����ȵ��ַ���,������strLong��str�ַ���
        /// </summary>
        /// <param name="strLong">���ɵĳ���</param>
        /// <param name="str">��str�����ַ���</param>
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
        /// �ж��ַ������ָ���
        /// </summary>
        /// <param name="strSource">ԭ�ַ�</param>
        /// <param name="strArg">�ַ�����</param>
        /// <returns></returns>
        public static int GetCount(string strSource,string strArg)
        {
            return Regex.Matches(strSource, strArg).Count;
        }
        /// <summary>
        /// �ָ��ַ���
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
        /// ����ַ��������е��ظ���
        /// </summary>
        /// <param name="data">�ַ�������</param>
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
        /// ��ȡ�ٷֱ�(��ǰ��Ŀ��������Ŀ�򷵻�-1)
        /// </summary>
        /// <param name="num">��ǰ��Ŀ</param>
        /// <param name="count">����Ŀ</param>
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
        /// ���ر�׼���ڸ�ʽstring
        /// </summary>
        public static string GetDate()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// ������С�����ڸ�ʽstring
        /// </summary>
        /// <returns></returns>
        public static string GetMinDate()
        {
            return "1900-01-01";
        }
        /// <summary>
        /// ���ر�׼ʱ���ʽstring
        /// </summary>
        public static string GetTime()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
        /// <summary>
        /// ���ر�׼ʱ���ʽstring
        /// </summary>
        public static string GetDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// ��������ڵ�ǰʱ����������
        /// </summary>
        public static string GetDateTime(int relativeday)
        {
            return DateTime.Now.AddDays(relativeday).ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// ���ر�׼ʱ���ʽstring
        /// </summary>
        public static string GetStandardDateTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fffffff");
        }
        /// <summary>
        /// ��֤�Ƿ���ʱ���ʽstring
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }
        /// <summary>
        /// �ж��ַ����Ƿ���yyyy-mm-dd�ַ���
        /// </summary>
        /// <param name="str">���ж��ַ���</param>
        /// <returns>�жϽ��</returns>
        public static bool IsDateString(string str)
        {
            return Regex.IsMatch(str, @"(\d{4})-(\d{1,2})-(\d{1,2})");
        }
        /// <summary>
        /// ������������
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
        /// �������ķ�����
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
        /// ��������Сʱ��
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
        /// �������ھ�ȷ�ַ�����(yyyyMMddHHmmssffff)
        /// </summary>
        /// <returns></returns>
        public static string GetDateTimeCode()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssffff");
        }
        /// <summary>
        /// ��ȡjs�������͵�����ʱ��
        /// </summary>
        /// <returns></returns>
        public static string GetJsDateTimeString()
        {
            return DateTime.Now.ToString("ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy", CultureInfoDate);
        }
        /// <summary>
        /// ����js���������ַ����õ�DateTime
        /// </summary>
        /// <param name="jsDateString"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string jsDateString)
        {
            return DateTime.ParseExact(jsDateString, "ddd MMM d HH:mm:ss 'UTC'zz'00' yyyy", CultureInfoDate);
        }
        /// <summary>
        /// mysql����Unixʱ�����C# DateTimeʱ�����ͻ���
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ConvertDateTime(System.DateTime time)
        {
            return (long)(time - UnixStartTime).TotalSeconds;
        }

        /// <summary>
        /// ת��Ϊ��̬html
        /// </summary>
        /// <param name="path">ԭ·��</param>
        /// <param name="outpath">���·��</param>
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
        ///// ���� HTML �ַ����ı�����
        ///// </summary>
        ///// <param name="str">�ַ���</param>
        ///// <returns>������</returns>
        //public static string HtmlEncode(string str)
        //{
        //    return HttpUtility.HtmlEncode(str);
        //}
        ///// <summary>
        ///// ���� HTML �ַ����Ľ�����
        ///// </summary>
        ///// <param name="str">�ַ���</param>
        ///// <returns>������</returns>
        //public static string HtmlDecode(string str)
        //{
        //    return HttpUtility.HtmlDecode(str);
        //}
        ///// <summary>
        ///// ���� URL �ַ����ı�����
        ///// </summary>
        ///// <param name="str">�ַ���</param>
        ///// <returns>������</returns>
        //public static string UrlEncode(string str)
        //{
        //    return HttpUtility.UrlEncode(str);
        //}
        ///// <summary>
        ///// ���� URL �ַ����ı�����
        ///// </summary>
        ///// <param name="str">�ַ���</param>
        ///// <returns>������</returns>
        //public static string UrlDecode(string str)
        //{
        //    return HttpUtility.UrlDecode(str);
        //}
        /// <summary>
        /// ����ָ��������html�ո����
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
        /// �Ƴ�Html���
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string RemoveHtml(string content)
        {
            string regexstr = @"<[^>]*>";
            return Regex.Replace(content, regexstr, string.Empty, RegexOptions.IgnoreCase);
        }
        /// <summary>
        /// ����HTML�еĲ���ȫ��ǩ
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
        /// ����HTML�еĲ���ȫ��ǩ
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
        /// ����HTML�еĲ���ȫ��ǩ
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
        /// ��HTML�л�ȡ�ı�,����br,p,img
        /// </summary>
        /// <param name="Html"></param>
        /// <returns></returns>
        public static string GetTextFromHtml(string Html)
        {
            System.Text.RegularExpressions.Regex regEx = new System.Text.RegularExpressions.Regex(@"</?(?!br|/?p|img)[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return regEx.Replace(Html, "");
        }
        /// <summary>
        /// ��HTML�л�ȡclassȥ��style
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
        /// ѹ��Html�ļ�
        /// </summary>
        /// <param name="Html">Html�ļ�</param>
        /// <returns></returns>
        public static string ZipHtml(string Html)
        {
            Html = Regex.Replace(Html, @">\s+?<", "><");//ȥ��Html�еĿհ��ַ�.
            Html = Regex.Replace(Html, @"\r\n\s*", "");
            Html = Regex.Replace(Html, @"<body([\s|\S]*?)>([\s|\S]*?)</body>", @"<body$1>$2</body>", RegexOptions.IgnoreCase);
            return Html;
        }
       /// <summary>
        /// ƥ��ҳ�������
       /// </summary>
       /// <param name="HtmlCode">htmlԴ��</param>
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
        /// ƥ��ҳ���ͼƬ��ַ(�����'|'�ָ�)
        /// </summary>
        /// <param name="HtmlCode">htmlԴ��</param>
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
        /// �����ļ�(���ĺ���)
        /// </summary>
        /// <param name="filePath">·��</param>
        /// <param name="text">����</param>
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
        /// �����ļ�
        /// </summary>
        /// <param name="filePath">·��</param>
        /// <param name="text">����</param>
        public static bool CreateFile(string filePath, string text)
        {
            return CreateFile(filePath, text, Encoding.Default);
        }
        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="filePath">·��</param>
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
        /// ��ȡ�ⲿ�ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        public static string ReadFile(string path)
        {
            return ReadFile(path, System.Text.Encoding.Default);
        }
        /// <summary>
        /// ��ȡ�ⲿ�ļ�
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="encode">���뷽ʽ</param>
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
        /// ��ȡ�ⲿ�ļ�(ȥ���ļ�ǰ������)
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="tCount">ȥ���ַ�����</param>
        public static string ReadFile(string path, int tCount)
        {
            return ReadFile(path, tCount,System.Text.Encoding.Default);    // Encoding.GetEncoding("gb2312"));
        }
        /// <summary>
        /// ��ȡ�ⲿ�ļ�(ȥ���ļ�ǰ������)
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="tCount">ȥ���ַ�����</param>
        /// <param name="encode">���뷽ʽ</param>
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
        /// ��ȡ�ⲿ�ļ�(һ���Զ�ȡ)
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <returns></returns>
        public static string ReadFileToEnd(string path)
        {
            return ReadFileToEnd(path, System.Text.Encoding.Default);
        }
        /// <summary>
        /// ��ȡ�ⲿ�ļ�(һ���Զ�ȡ)
        /// </summary>
        /// <param name="path">�ļ�·��</param>
        /// <param name="encode">���뷽ʽ</param>
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
        /// �����ļ�
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <param name="overwrite">��Ŀ���ļ�����ʱ�Ƿ񸲸�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName, bool overwrite)
        {
            if (!System.IO.File.Exists(sourceFileName))
            {
                throw new FileNotFoundException(sourceFileName + "�ļ������ڣ�");
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
        /// �����ļ�,��Ŀ���ļ�����ʱ����
        /// </summary>
        /// <param name="sourceFileName">Դ�ļ���</param>
        /// <param name="destFileName">Ŀ���ļ���</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool BackupFile(string sourceFileName, string destFileName)
        {
            return BackupFile(sourceFileName, destFileName, true);
        }
        /// <summary>
        /// �ָ��ļ�
        /// </summary>
        /// <param name="backupFileName">�����ļ���</param>
        /// <param name="targetFileName">Ҫ�ָ����ļ���</param>
        /// <param name="backupTargetFileName">Ҫ�ָ��ļ��ٴα��ݵ�����,���Ϊnull,���ٱ��ݻָ��ļ�</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        public static bool RestoreFile(string backupFileName, string targetFileName, string backupTargetFileName)
        {
            try
            {
                if (!System.IO.File.Exists(backupFileName))
                {
                    throw new FileNotFoundException(backupFileName + "�ļ������ڣ�");
                }
                if (backupTargetFileName != null)
                {
                    if (!System.IO.File.Exists(targetFileName))
                    {
                        throw new FileNotFoundException(targetFileName + "�ļ������ڣ��޷����ݴ��ļ���");
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
        /// �����ļ��Ƿ����
        /// </summary>
        /// <param name="filename">�ļ���</param>
        /// <returns>�Ƿ����</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        /// <summary>
        /// �����ļ���
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return MakeSureDirectoryPathExists(name);
        }
        /// <summary>
        /// ����Ŀ¼
        /// </summary>
        /// <param name="name">����</param>
        /// <returns>�����Ƿ�ɹ�</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);
        /// <summary>
        /// ����·���е��ļ���
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
        /// ��ȡ�ļ���չ��
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
        /// ����ָ��Ŀ¼�µķ� UTF8 �ַ����ļ�
        /// </summary>
        /// <param name="Path">·��</param>
        /// <returns>�ļ������ַ�������</returns>
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
        /// ����URL�н�β���ļ���
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
        ///// (Url����)��õ�ǰ����·��
        ///// </summary>
        ///// <param name="strPath">ָ����·��</param>
        ///// <returns>����·��</returns>
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
        ///// (Url����)����Ӧ�ó����Ŀ¼��URL
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
        /// (Url����)��õ�ǰ����·��
        /// </summary>
        /// <param name="oldSrc">ԭ���·��</param>
        /// <param name="httpUrl">��ǰҲ·��</param>
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
        /// ��ȡUrl����ֵ
        /// </summary>
        /// <param name="strHref">���ӵ�ַ</param>
        /// <param name="strName">������</param>
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
        /// ���ַ���ת��ΪColor
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
        /// �����ɫֵ�Ƿ�Ϊ3/6λ�ĺϷ���ɫ
        /// </summary>
        /// <param name="color">��������ɫ</param>
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
        /// ����Url���Դ�ļ�����
        /// </summary>
        /// <param name="url">�Ϸ���Url��ַ</param>
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
        /// HTTP POST��ʽ��������
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="param">POST������</param>
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
        /// HTTP GET��ʽ��������.
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
        /// ����Ҫת���ĺ����ַ���-ƴ����д
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
        /// ����Ҫת���ĵ�������-ƴ����ĸ
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
        /// ���л������ļ�
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
        /// ���ļ������л�������
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
        ///  ��Ŀ��������л���XML�ĵ�(utf-16)
        ///  </summary>
        ///  <param  name="obj">����</param>
        ///  <param  name="type">��������</param>
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
        ///  ���ַ������ݷ����л��ɶ���(utf-16)
        ///  </summary>
        ///  <param  name="txt">�����л����ı�</param>
        ///  <param  name="type">��������</param>
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
        /// �������л��� xml string
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
        /// �����л��ɶ��� xml string 
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
        /// �������л���(������)
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
        ///  �����л��ɶ���(������)
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
        /// �����ַ�����ȡ����ֵ
        /// </summary>
        /// <param name="tmptype">������</param>
        /// <param name="proname">������</param>
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
        /// �����ַ�����ȡ����ֵ����
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
        /// �����ַ�����ȡ�ֶ�ֵ
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
        /// �����ַ�����ȡ�ֶ�ֵ����
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
        /// �õ��ý����ڴ�Ĵ�С(��λ��)
        /// </summary>
        /// <returns></returns>
        public static double GetMemorySize()
        {
            return (Double)System.Diagnostics.Process.GetCurrentProcess().WorkingSet64 / 1048576;
        }
        /// <summary>
        /// ���Assembly�汾��
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyVersion(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.FileVersion;
        }
        /// <summary>
        /// ���Assembly�汾��
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
        /// ���Assembly��Ʒ����
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyProductName(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.ProductName;
        }
        /// <summary>
        /// ���Assembly��Ʒ��Ȩ
        /// </summary>
        /// <param name="assemblyPath"></param>
        /// <returns></returns>
        public static string GetAssemblyCopyright(string assemblyPath)
        {
            FileVersionInfo AssemblyFileVersion = (assemblyPath != "") ? FileVersionInfo.GetVersionInfo(assemblyPath) : FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
            return AssemblyFileVersion.LegalCopyright;
        }
        /// <summary>
        /// ����õ�ʵ��
        /// </summary>
        /// <param name="assemblyName">��������</param>
        /// <param name="className">����</param>
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
        /// ������ò������(ExecAssemblyMethod("PlugNT.App.Bbs", "Section", "RemoveTopicCount", sysModelId, channelId , isToday ))
        /// </summary>
        /// <param name="assemblyName">��������</param>
        /// <param name="className">����</param>
        /// <param name="methodName">������</param>
        /// <param name="objParas">����</param>
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
        /// �򵥼����ַ���
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
        /// �򵥽����ַ���
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
            sr.WriteLine(DateTime.Now.ToString() + "��" + message);
            sr.Close();
        }
        public static string Help()
        {
            return @"mailto:354990393@qq.com";
        }

        #region �������±����ֶγ���2
        /// <summary>
        /// �������±����ֶγ��ȣ���...����
        /// </summary>
        /// <param name="str">��Ҫ�������������</param>
        /// <param name="maxstr">�����ʾ����</param>
        /// <param name="laststr">ʵ����ʾ����</param>
        /// <returns>�����ַ��� + "..."</returns>
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

        #region �������±����ֶγ���
        /// <summary>
        /// �������±����ֶγ���
        /// </summary>
        /// <param name="str">��Ҫ�������������</param>
        /// <param name="maxstr">�����ʾ����</param>
        /// <param name="laststr">ʵ����ʾ����</param>
        /// <returns>�����ַ��� + "..."</returns>
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

        #region ��ʽ���ı���������ݣ�
        /// <summary>
        /// ��ʽ���ı���������ݣ�
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

