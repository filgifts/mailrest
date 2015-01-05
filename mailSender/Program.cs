using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailData;
using MailData.Model;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Configuration;
using System.ComponentModel;

namespace mailSender
{
    public class Program
    {
        static void Main(string[] args)
        {
            using (var dbFactory = new MailData.DataFactory(ConfigurationManager.ConnectionStrings["filgiftsMail"].ConnectionString))
            {
                var mails = dbFactory.GetMailByStatus(1);
                foreach (var mail in mails)
                {
                    dbFactory.UpdateMailStatus(mail.Id, 2)
                    SendMail(mail);
                }
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            using (var dbFactory = new MailData.DataFactory(ConfigurationManager.ConnectionStrings["filgiftsMail"].ConnectionString))
            {
                var id = (int)e.UserState;
                if (e.Error != null)
                {
                    dbFactory.UpdateMailStatus(id, -1);
                }
                else
                {
                    dbFactory.UpdateMailStatus(id, 0);
                }
            }
        }

        private static void SendMail(MailDetails request)
        {
            var message = new MailMessage(request.From, request.To);
            message.Subject = request.Subject;

            message.Body = request.Message;

            if (!string.IsNullOrEmpty(request.Cc))
            {
                message.CC.Add(new MailAddress(request.Cc));
            }
            if (!string.IsNullOrEmpty(request.Bcc))
            {
                if (request.Bcc.Contains(","))
                {
                    var bccs = request.Bcc.Split(',');
                    foreach (var item in bccs)
                    {
                        message.Bcc.Add(new MailAddress(item));
                    }
                }
                else
                {
                    message.Bcc.Add(new MailAddress(request.Bcc));
                }
            }
            if (request.IsHTML)
            {
                message.IsBodyHtml = true;
            }

            var smtp = new SmtpClient();
            smtp.Host = ConfigurationManager.AppSettings["smtphost"];
            smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtpport"]);
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpusername"], ConfigurationManager.AppSettings["smtppassword"]);
            smtp.EnableSsl = true;
            smtp.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            smtp.SendCompleted += (s, e) =>
            {
                message.Dispose();
                smtp.Dispose();
            };
            var token = request.Id;
            try
            {
                smtp.SendAsync(message, token);
            }
            catch (SmtpException e)
            {
                var errormessage = e.StatusCode;
            }
            Console.ReadLine();
        }
    }
}
