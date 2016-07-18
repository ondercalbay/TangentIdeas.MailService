using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangentIdeas.Core.Entities
{
    [Table("Mail", Schema = "System")]
    public class Mail : BaseEntity
    {
        public List<MailTargets> To { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }        
        public MailStatusType Status { get; set; }                
        public MailSenderType MailSender { get; set; }
        public DateTime SendTime { get; set; }
        public string Exception { get; set; }
    }

    public enum MailStatusType
    {
        Waiting = 0,
        Sent = 1,
        Error = 2
    }

    public enum MailSenderType
    {
        System = 1,        
        User = 2,
        Advertisement = 3
    }

    [Table("MailTargets", Schema = "System")]
    public class MailTargets : BaseEntity
    {
        public string MailAddres { get; set; }
    }    
}
