using System.Collections.Generic;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Entities.System;

namespace TangetIdeas.MailService.Business.Interfaces
{
    public interface IMailSenderService
    {
        ServiceResult SendMail(string subject, string message, List<MailTarget> to, MailSenderTypeEnum mailSender);
    }
}
