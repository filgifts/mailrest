using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.ServiceHost;
using System.IO;


namespace mailrest.Model
{
    [Route("/mailmessage")]
    [Route("/mailmessage/{Name}")]
    public class MailMessaging
    {
        public string From {get; set;}
        public string To { get; set; }
        public string ReplyTo { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public string Cc { get; set; }
        public string Bcc { get; set; }
        public bool IsHTML { get; set; }
    }

public class MailMessageResponse
{
    public string Result { get; set; }
}

public class MessageFile
{
    public string Path { get; set; }
    public string TextContents { get; set; }
}


}