/** 
* EmailHelper.cs
*
* 功 能： 邮件发送辅助类
* 类 名： EmailHelper
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2016/9/28 17:05:34   李庸    初版
*
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：成都中科创动科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    /// <summary>
    /// 邮件发送辅助类
    /// </summary>
    public static class EmailHelper
    {
        /// <summary>
        /// 发送激活邮件
        /// </summary>
        /// <param name="email"></param>
        /// <param name="activeCode"></param>
        /// <param name="userid"></param>
        public static void SendMail(string email, string activeCode, int userid)
        {
            MailConfig mailConfig = new MailConfig();
            try
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.From = new MailAddress(mailConfig.Email_Name,mailConfig.Email_DisplayName);
                mailMsg.To.Add(email);
                mailMsg.Subject = "请激活注册";
                StringBuilder contentBuilder = new StringBuilder();
                contentBuilder.Append("请单击以下链接完成激活：");
                contentBuilder.Append("<a href='http://localhost:15464/cheng.aspx?activecode=" + activeCode + "&id=" +
                                      userid + "'>激活</a>");
                mailMsg.Body = contentBuilder.ToString(); //拼接字符串 
                mailMsg.IsBodyHtml = true;
                SmtpClient client = new SmtpClient
                {
                    Host = mailConfig.SMTP,
                    Port = int.Parse(mailConfig.SMTP_POST)
                };
                //发件方服务器地址 
                //mailMsg.IsBodyHtml = true; 
                NetworkCredential credetial = new NetworkCredential
                {
                    UserName = mailConfig.Email_User,
                    Password = mailConfig.Email_Pwd
                };
                client.EnableSsl = mailConfig.UseSSL == "1";
                client.Credentials = credetial;
                client.Send(mailMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
