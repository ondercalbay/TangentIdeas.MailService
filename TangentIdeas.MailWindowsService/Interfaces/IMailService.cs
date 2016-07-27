
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Entities.System;

namespace TangentIdeas.MailWindowsService.Interfaces
{
    public interface IMailService
    {
        ServiceResult Add(MailItem mail);
        ServiceResult GetWaitingMail();
        ServiceResult SendWaitingMails();
    }
}
