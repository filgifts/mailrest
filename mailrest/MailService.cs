using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceInterface;
using mailrest.Model;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.IO;

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
            using (var message = new MailMessage(request.From, request.To))
            {
                message.Subject = request.Subject;

                if (!string.IsNullOrEmpty(request.Message))
                {
                    var convertedBase64 = Convert.FromBase64String(request.Message);
                    var decodedString = System.Text.Encoding.UTF8.GetString(convertedBase64);
                    message.Body = decodedString;
                }

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

                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "email-smtp.us-west-2.amazonaws.com";
                    smtp.Port = 587;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("AKIAIHZXXEETLVFJMLBQ", "Aj5giKnRAGdMQyLIUBgWoxjuG8vXTNy5a+KMBb7WAJpR");
                    smtp.EnableSsl = true;
                    smtp.Send(message);
                   
                }
            }
            return new MailMessageResponse { Result = "ok" };
        }
    }

}