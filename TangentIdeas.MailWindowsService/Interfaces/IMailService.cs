using TangentIdeas.Core.Common.Common;
using TangentIdeas.Core.Entities;

namespace TangentIdeas.MailWindowsService.Interfaces
{
    public interface IMailService
    {
        ServiceResult Add(Mail mail);
        ServiceResult GetWaitingMail();
        ServiceResult SendWaitingMails();
    }
}
