using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace MailData.Model
{
    public class AccessLogs
    {
        [AutoIncrement]
        public int Id { get; set; }
        public string RemoteIP { get; set; }
    }
}
