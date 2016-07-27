using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Entities.System;

namespace TangentIdeas.MailWindowsService.Interfaces
{
    public interface IMailSenderService
    {           
        ServiceResult SendMail(string subject, string message, List<MailTarget> to, MailSenderTypeEnum mailSender);
    }
}
