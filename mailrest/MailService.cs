using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceInterface;
using mailrest.Model;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.IO;
using System.Web.Configuration;
using System.ComponentModel;
using MailData;
using MailData.Model;
using System.Configuration;


namespace mailrest 
{
    public class MailService : Service
    {
        public object Any(MessageFile request)
        {
            var t = GetPath(request);
            return new MailMessageResponse { Result = "ok" };
        }

        private FileInfo GetPath(MessageFile request)
        {
            return new FileInfo(Path.Combine(@"C:\", request.Path));
        }

        public object Any(MailMessaging request)
        {
            string mailBody = string.Empty;
            if (!string.IsNullOrEmpty(request.Message))
            {
                var convertedBase64 = Convert.FromBase64String(request.Message);
                var decodedString = System.Text.Encoding.UTF8.GetString(convertedBase64);
                mailBody = decodedString;
            }

            using (var dbFactory = new MailData.DataFactory(ConfigurationManager.ConnectionStrings["filgiftsMail"].ConnectionString))
            {
                var mailDetails = new MailData.Model.MailDetails
                {
                    From = request.From,
                    To = request.To,
                    Cc = request.Cc,
                    IsHTML = request.IsHTML,
                    Message = mailBody,
                    Status = 1,
                    Subject = request.Subject
                };
                dbFactory.AddMailMessage(mailDetails);
            }

            //using (var message = new MailMessage(request.From, request.To))
            //{
            //    message.Subject = request.Subject;

            //    if (!string.IsNullOrEmpty(request.Message))
            //    {
            //        var convertedBase64 = Convert.FromBase64String(request.Message);
            //        var decodedString = System.Text.Encoding.UTF8.GetString(convertedBase64);
            //        message.Body = decodedString;
            //    }

            //    if (!string.IsNullOrEmpty(request.Cc))
            //    {
            //        message.CC.Add(new MailAddress(request.Cc));
            //    }
            //    if (!string.IsNullOrEmpty(request.Bcc))
            //    {
            //        if (request.Bcc.Contains(","))
            //        {
            //            var bccs = request.Bcc.Split(',');
            //            foreach (var item in bccs)
            //            {
            //                message.Bcc.Add(new MailAddress(item));
            //            }
            //        }
            //        else
            //        {
            //            message.Bcc.Add(new MailAddress(request.Bcc));
            //        }
            //    }
            //    if (request.IsHTML)
            //    {
            //        message.IsBodyHtml = true;
            //    }

            //    //using (var smtp = new SmtpClient())
            //    //{
            //    //    smtp.Host = WebConfigurationManager.AppSettings["smtphost"];
            //    //    smtp.Port = int.Parse(WebConfigurationManager.AppSettings["smtpport"]);
            //    //    smtp.UseDefaultCredentials = false;
            //    //    smtp.Credentials = new NetworkCredential(WebConfigurationManager.AppSettings["smtpusername"], WebConfigurationManager.AppSettings["smtppassword"]);
            //    //    smtp.EnableSsl = true;
            //    //    try
            //    //    {
            //    //        smtp.Send(message);
            //    //    }
            //    //    catch (SmtpException e)
            //    //    {
            //    //        var errormessage = e.StatusCode;
            //    //    }
            //    //}
            //}
            return new MailMessageResponse { Result = "ok" };
        }

    }

}