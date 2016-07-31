
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Entities.System;

namespace TangetIdeas.MailService.Business.Interfaces
{
    public interface IMailService
    {
        ServiceResult Add(MailItem mail);
        ServiceResult GetWaitingMail();
        ServiceResult SendWaitingMails();
        ServiceResult Add(SendMailRequestDto sendMailRequest);
        ServiceResult SendMail(MailItem mailItem);
    }
}
