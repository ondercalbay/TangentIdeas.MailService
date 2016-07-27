using log4net;
using System;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using TangentIdeas.Core.Common.Common;
using TangentIdeas.Core.Common.Exceptions;
using TangentIdeas.Core.Common.Exceptions.ExceptionCodes;
using TangentIdeas.Core.Entities;
using TangentIdeas.MailWindowsService.Interfaces;
using TangentIdeas.MailWindowsService.Model;

namespace TangentIdeas.MailWindowsService.Implementations
{
    public class MailService: IMailService
    {
        internal ILog Log { get; } = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly MailServiceModel _model;
        private readonly IMailSenderService _mailSenderService;

        public MailService(MailServiceModel model,IMailSenderService mailSenderService)
        {
            _model = model;
            _mailSenderService = mailSenderService;
        }

        public ServiceResult GetWaitingMail()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mail = _model.Mail.Select(m=>m.Status == Core.Entities.MailStatusType.Waiting);
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

        public ServiceResult Add(Mail mail)
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
                var mails = _model.Mail.Where(m => m.Status == MailStatusType.Waiting).ToList();
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

        private void SendMail(Mail item)
        {
              
            var resultMail = new ServiceResult();
            resultMail = _mailSenderService.SendMail(item.Subject, item.Message, item.To, item.MailSender);
            if ((bool)resultMail.Data)
            {
                item.Status = MailStatusType.Sent;
                item.SendTime = DateTime.Now;
            }
            else
            {
                item.Status = MailStatusType.Error;
                item.Exception = resultMail.Exception.Message;
            }
            item.UpdatedAt = DateTime.Now;
            _model.Entry(item).State = EntityState.Modified;
            _model.SaveChanges();
        }
    }
}
