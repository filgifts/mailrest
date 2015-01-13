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
using System.Threading;

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
                    dbFactory.UpdateMailStatus(mail.Id, 2);
                }

                SendMail(mails);
            }
        }

        private static void SendMail(List<MailDetails> mails)
        {
            foreach (var mail in mails)
            {
                Send(mail);
            }
        }

        private static void Send(MailDetails request)
        {
            using (ManualResetEvent waitHandle = new ManualResetEvent(false)) // handle blocking of asynchronous sending
            {
                var message = new MailMessage(request.From, request.To);
                message.Subject = request.Subject;

                message.Body = request.Message;

                if (!string.IsNullOrEmpty(request.ReplyTo))
                {
                    message.ReplyToList.Add(new MailAddress(request.ReplyTo));
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

                var smtp = new SmtpClient();
                smtp.Host = ConfigurationManager.AppSettings["smtphost"];
                smtp.Port = int.Parse(ConfigurationManager.AppSettings["smtpport"]);
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["smtpusername"], ConfigurationManager.AppSettings["smtppassword"]);
                smtp.EnableSsl = ConfigurationManager.AppSettings["smtpusername"].ToLower() == "true";
                var token = request.Id;

                smtp.SendCompleted += (s, e) =>
                {
                    using (var dbFactory = new MailData.DataFactory(ConfigurationManager.ConnectionStrings["filgiftsMail"].ConnectionString))
                    {
                        var id = (int)e.UserState;
                        if (e.Error != null)
                        {
                            dbFactory.UpdateMailStatus(id, -1);
                            
                            var mailError = new MailError
                            {
                                Message = e.Error.Message,
                                Source = e.Error.Source,
                                StackTrace = e.Error.StackTrace,
                                EmailId = id
                            };
                            dbFactory.LogMailError(mailError); // log error
                        }
                        else
                        {
                            dbFactory.UpdateMailStatus(id, 0);
                        }
                    }
                    waitHandle.Set(); // tell handler that sending completed
                };

                smtp.SendCompleted += (s, e) =>
                {
                    message.Dispose();
                    smtp.Dispose();
                };

                try
                {
                    smtp.SendAsync(message, token);
                }
                catch (SmtpException e)
                {
                    var errormessage = e.StatusCode;
                }
                waitHandle.WaitOne(); // tell handler to wait until sending completed
            }
        }
    }

}
