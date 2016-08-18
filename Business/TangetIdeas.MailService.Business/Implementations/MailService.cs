using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Dto.System;
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
        private readonly ILog _logger;

        public MailService(CeviriDukkaniModel model, IMailSenderService mailSenderService, ILog logger)
        {
            _model = model;
            _mailSenderService = mailSenderService;
            _logger = logger;
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
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult SendWaitingMails()
        {
            var serviceResult = new ServiceResult();
            try
            {
                var mails = _model.Mail.Include(a => a.To).Where(m => m.Status == MailStatusTypeEnum.Waiting).ToList();
                if (mails == null)
                {
                    throw new DbOperationException(ExceptionCodes.NoRelatedData);
                }

                //Task.Run(() =>
                //{
                Parallel.ForEach(mails, i => { SendMail(i); });
                //});

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = mails;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult SendMail(MailItem item)
        {
            var serviceResult = new ServiceResult();
            try
            {

                serviceResult = _mailSenderService.SendMail(item.Subject, item.Message, item.To, item.MailSender);
                if (serviceResult.ServiceResultType == ServiceResultType.Success)
                {
                    item.Status = MailStatusTypeEnum.Sent;
                    item.SendTime = DateTime.Now;
                }
                else
                {
                    item.Status = MailStatusTypeEnum.Error;
                    item.Exception = serviceResult.Exception.Message;
                }
                item.UpdatedAt = DateTime.Now;
                _model.Entry(item).State = EntityState.Modified;
                _model.SaveChanges();
                serviceResult.ServiceResultType = ServiceResultType.Success;
            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }

        public ServiceResult Add(SendMailRequestDto sendMailRequest)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                MailItem mailItem = new MailItem();

                StringBuilder mailContent = new StringBuilder();

                mailContent.Append(File.ReadAllText("\\MailTemplates\\" + sendMailRequest.MailType.ToString() + "Template.html"));

                switch (sendMailRequest.MailType)
                {
                    case MailTypeEnum.Register:
                        mailItem.MailSender = MailSenderTypeEnum.User;

                        mailItem.Subject = "Kayıt";

                        var mailRegister = (MailDataDto.Register)sendMailRequest.Data;
                        mailContent.Replace("[USER-NAME]", mailRegister.UserName).Replace("[USER-EMAIL]", mailRegister.EMail).Replace("[USER-PASS]", mailRegister.Pass);
                        break;
                    case MailTypeEnum.ForgetPassword:
                        mailItem.MailSender = MailSenderTypeEnum.User;

                        mailItem.Subject = "Şifremi Unuttum";

                        var mailDataForget = (MailDataDto.ForgetPassword)sendMailRequest.Data;
                        mailContent.Replace("[USER-NAME]", mailDataForget.UserName).Replace("[USER-EMAIL]", mailDataForget.EMail).Replace("[USER-PASS]", mailDataForget.Pass);
                        break;
                    case MailTypeEnum.ResetPassword:

                        mailItem.MailSender = MailSenderTypeEnum.User;

                        mailItem.Subject = "Şifremi Sıfırla";

                        var maDataReset = (MailDataDto.ResetPassword)sendMailRequest.Data;
                        mailContent.Replace("[USER-NAME]", maDataReset.UserName).Replace("[RESET-LINK]", maDataReset.ResetLink);
                        break;
                    case MailTypeEnum.UserActivation:
                        mailItem.MailSender = MailSenderTypeEnum.User;

                        mailItem.Subject = "Kullanıcı Etkinleştirme";

                        var mDataActivation = (MailDataDto.UserActivation)sendMailRequest.Data;

                        mailContent = mailContent.Replace("[USER-NAME]", mDataActivation.UserName).Replace("[DETAIL]", mDataActivation.Comment).Replace("[REGISTER-LINK]", mDataActivation.RegisterLink);
                        break;
                    case MailTypeEnum.Welcome:
                        mailItem.MailSender = MailSenderTypeEnum.User;
                        mailItem.Subject = "Hoş Geldiniz";
                        break;
                    default:
                        break;
                }
                StringBuilder emailFile = new StringBuilder();
                emailFile.Append(File.ReadAllText("\\MailTemplates\\EmailTemplate.html"));
                emailFile.Replace("[MAIL-TEMPLATE]", mailContent.ToString());
                mailItem.Message = emailFile.ToString();
                
                List<MailTarget> To = new List<MailTarget>();
                foreach (var item in sendMailRequest.To)
                {
                    To.Add(new MailTarget { MailAddres = item, Active = true });
                }
                mailItem.To = To;
                mailItem.Status = MailStatusTypeEnum.Progress;
                serviceResult = Add(mailItem);

                _mailSenderService.SendMail(mailItem);

            }
            catch (Exception exc)
            {
                serviceResult.Exception = exc;
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod().Name} with exception message {exc.Message} and inner exception {exc.InnerException?.Message}");
            }
            return serviceResult;
        }
    }
}
