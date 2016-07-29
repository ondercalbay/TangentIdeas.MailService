using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Entities.System;
using Tangent.CeviriDukkani.Domain.Exceptions.ExceptionCodes;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangetIdeas.MailService.Business.Implementations
{
    public class YandexMailService : IMailSenderService
    {        
        #region Implementation of IMailService

        public ServiceResult SendMail(string subject, string message, List<MailTarget> to, MailSenderTypeEnum mailSender)
        {
            var serviceResult = new ServiceResult
            {
                ServiceResultType = ServiceResultType.NotKnown
            };
            try
            {
                string emailAddress = ConfigurationManager.AppSettings["EmailAddress" + mailSender],
                    emailPassword = ConfigurationManager.AppSettings["EmailPassword" + mailSender],
                    emailSmtp = ConfigurationManager.AppSettings["EmailSmtp"], emailSmtpPort = ConfigurationManager.AppSettings["EmailSmtpPort"];


                var smtp = new SmtpClient
                {
                    Credentials = new NetworkCredential(emailAddress, emailPassword),
                    Host = emailSmtp,
                    Port = int.Parse(emailSmtpPort),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    //UseDefaultCredentials = false,
                    EnableSsl = true
                };
                smtp.SendCompleted += Smtp_SendCompleted;

                var mail = new MailMessage
                {
                    Body = message,
                    Subject = subject,
                    IsBodyHtml = true
                };

                to.ForEach(a => mail.To.Add(a.MailAddres));
                mail.From = new MailAddress(emailAddress);

                //var mailMessage = new MailMessage(emailAddress, mail) {
                //    Subject = subject,
                //    Body = message,
                //    IsBodyHtml = true
                //};
                smtp.Send(mail);

                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = true;

            }
            catch (Exception exc)
            {
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.Exception = exc;
                serviceResult.Data = false;
                serviceResult.ExceptionCode = ExceptionCodes.EmailCouldntSendToUser;
            }

            return serviceResult;

        }

        public ServiceResult SendMail(MailItem mailItem)
        {
            var serviceResult = new ServiceResult
            {
                ServiceResultType = ServiceResultType.NotKnown
            };
            try
            {                
                SendMail(mailItem.Subject, mailItem.Message, mailItem.To, mailItem.MailSender);
                serviceResult.ServiceResultType = ServiceResultType.Success;
                serviceResult.Data = true;

            }
            catch (Exception exc)
            {
                serviceResult.ServiceResultType = ServiceResultType.Fail;
                serviceResult.Exception = exc;
                serviceResult.Data = false;
                serviceResult.ExceptionCode = ExceptionCodes.EmailCouldntSendToUser;
            }

            return serviceResult;

        }



        private void Smtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                Trace.WriteLine($"The message is {e.Error} and {e.UserState}");
            }
        }

        #endregion
    }
}
