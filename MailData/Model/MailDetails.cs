using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace MailData.Model
{
    public class MailDetails
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string MessageId { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public bool IsHTML { get; set; }
        public int Status { get; set; }
    }
}
