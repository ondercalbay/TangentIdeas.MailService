using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TangentIdeas.Core.Common.Common;
using TangentIdeas.Core.Entities;

namespace TangentIdeas.MailWindowsService.Interfaces
{
    public interface IMailSenderService
    {           
        ServiceResult SendMail(string subject, string message, List<MailTargets> to, MailSenderType mailSender);
    }
}
