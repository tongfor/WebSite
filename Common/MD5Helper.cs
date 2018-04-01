/**********************************************************************************
 * ����˵����     MD5������
 * �������ڣ�     2015.6.23
 * ����������     liyong
 * ��ϵ��ʽ��     mailto:379359173@qq.com  
 * ��Ȩ������     http:
 * ********************************************************************************/
/** 
* MD5Helper.cs
*
* �� �ܣ� MD5������
* �� ���� MD5Helper
*
* Ver    �������             ������  �������
* ����������������������������������������������������������������������
* V0.01  2016/9/5 17:05:34   ��ӹ    ����
*
*������������������������������������������������������������������������
*�����˼�����ϢΪ����˾������Ϣ��δ������˾����ͬ���ֹ���������¶������
*������Ȩ���У��ɶ��пƴ����Ƽ����޹�˾������������������������������
*������������������������������������������������������������������������
*/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// MD5Helper(ͨ��MD5��)
    /// </summary>
    public static class MD5Helper
    {
        #region MD5����
        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="input">��Ҫ���ܵ��ַ���</param>
        /// <returns></returns>
        public static string MD5Encrypt(this string input)
        {
            return MD5Encrypt(input, new UTF8Encoding());
        }

        /// <summary>
        /// MD5����
        /// </summary>
        /// <param name="input">��Ҫ���ܵ��ַ���</param>
        /// <param name="encode">�ַ��ı���</param>
        /// <returns></returns>
        public static string MD5Encrypt(string input, Encoding encode)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] t = md5.ComputeHash(encode.GetBytes(input));
            StringBuilder sb = new StringBuilder(32);
            for (int i = 0; i < t.Length; i++)
                sb.Append(t[i].ToString("x").PadLeft(2, '0'));
            return sb.ToString();
        }

        /// <summary>
        /// MD5���ļ�������
        /// </summary>
        /// <param name="sr"></param>
        /// <returns></returns>
        public static string MD5Encrypt(Stream stream)
        {
            MD5 md5serv = MD5.Create();
            byte[] buffer = md5serv.ComputeHash(stream);
            StringBuilder sb = new StringBuilder();
            foreach (byte var in buffer)
                sb.Append(var.ToString("x2"));
            return sb.ToString();
        }

        /// <summary>
        /// MD5����(����16λ���ܴ�)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static string MD5Encrypt16(string input, Encoding encode)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            string result = BitConverter.ToString(md5.ComputeHash(encode.GetBytes(input)), 4, 8);
            result = result.Replace("-", "");
            return result;
        }
        #endregion
    }
}

