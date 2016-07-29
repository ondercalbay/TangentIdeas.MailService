using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Entities.System;
using Tangent.CeviriDukkani.Domain.Exceptions;
using Tangent.CeviriDukkani.Domain.Exceptions.ExceptionCodes;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangetIdeas.MailService.Business.Implementations
{
    public class MailService : IMailService
    {
        //internal ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly CeviriDukkaniModel _model;
        private readonly IMailSenderService _mailSenderService;

        public MailService(CeviriDukkaniModel model, IMailSenderService mailSenderService)
        {
            _model = model;
            _mailSenderService = mailSenderService;
        }

        public ServiceResult GetWaitingMail()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mail = _model.Mail.Select(m => m.Status == MailStatusTypeEnum.Waiting);
                if (mail == null)
                {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = mail;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                //Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult Add(MailItem mail)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                _model.Mail.Add(mail);

                serviceResult.Data = _model.SaveChanges() > 0;
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
            }
            return serviceResult;
        }

        public ServiceResult SendWaitingMails()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mails = _model.Mail.Where(m => m.Status == MailStatusTypeEnum.Waiting).ToList();
                if (mails == null)
                {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                Parallel.ForEach(mails, i =>
                {
                    SendMail(i);
                }
                );

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = mails;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                //Log.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        private void SendMail(MailItem item)
        {

            var resultMail = new ServiceResult();
            resultMail = _mailSenderService.SendMail(item.Subject, item.Message, item.To, item.MailSender);
            if ((bool)resultMail.Data)
            {
                item.Status = MailStatusTypeEnum.Sent;
                item.SendTime = DateTime.Now;
            }
            else
            {
                item.Status = MailStatusTypeEnum.Error;
                item.Exception = resultMail.Exception.Message;
            }
            item.UpdatedAt = DateTime.Now;
            _model.Entry(item).State = EntityState.Modified;
            _model.SaveChanges();
        }

        public ServiceResult Add(SendMailRequestDto sendMailRequest)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                MailItem mailItem = new MailItem();
                mailItem.MailSender = sendMailRequest.MailSender;
                mailItem.Message = sendMailRequest.Message;
                mailItem.Subject = sendMailRequest.Subject;
                mailItem.To = sendMailRequest.To;
                serviceResult = Add(mailItem);
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
            }
            return serviceResult;
        }
    }
}
