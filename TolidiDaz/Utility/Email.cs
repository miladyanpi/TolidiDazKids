using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Utility
{
    public class Email
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="smtp">smtp</param>
        /// <param name="from">ایمیل فرستنده</param>
        /// <param name="password">رمز ایمیل</param>
        /// <param name="To">ایمیل گیرنده</param>
        /// <param name="subject">موضوع ایمیل</param>
        /// <param name="body">متن ایمیل</param>
        public void SendEmail(string smtp, string from, string password, string To, string subject, string body)
        {
            MailMessage myEmail = new MailMessage();
            myEmail.From = new MailAddress(from);
            myEmail.To.Add(To);
            myEmail.Subject = subject;
            myEmail.Body = body;
            myEmail.IsBodyHtml = true;
            myEmail.Priority = MailPriority.High;
            SmtpClient mysmtp = new SmtpClient(smtp);
            mysmtp.UseDefaultCredentials = false;
            mysmtp.EnableSsl = false;
            mysmtp.Port = 25;
            mysmtp.Credentials = new NetworkCredential(from, password);
            mysmtp.Send(myEmail);



        }
    }
}