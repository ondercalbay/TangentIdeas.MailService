using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
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
        private readonly string _url;

        public MailService(CeviriDukkaniModel model, IMailSenderService mailSenderService, ILog logger)
        {
            _model = model;
            _mailSenderService = mailSenderService;
            _logger = logger;
            _url = ConfigurationSettings.AppSettings["Url"].ToString();
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


        class FieldValue {
            public string Name { get; set; }
            public object Data { get; set; }
        }

        private List<FieldValue> GetFieldsAndValues(object data) {
            var propInfos = data.GetType().GetProperties();
            var returnList = new List<FieldValue>();
            foreach (var item in propInfos)
            {
                var propName = item.Name;
                var value = item.GetValue(data);

                returnList.Add(new FieldValue { Name = propName, Data = value });
            }
            return returnList;
        }

        public ServiceResult Add(SendMailRequestDto sendMailRequest)
        {
            var serviceResult = new ServiceResult(ServiceResultType.NotKnown);
            try
            {
                MailItem mailItem = new MailItem();

                StringBuilder mailContent = new StringBuilder();

                mailContent.Append(File.ReadAllText("\\MailKaliplari\\" + sendMailRequest.MailType.ToString() + ".html"));
                
                List<FieldValue> fields = GetFieldsAndValues(sendMailRequest.Data);

                
                switch (sendMailRequest.MailType)
                {
                    case MailTypeEnum.AdminTestOnayla:
                        mailItem.MailSender = MailSenderTypeEnum.User;
                        mailItem.Subject = "Test Tercüme Kontrolü";
                        //var mdAdminTestOnayla = (MailDataDto.AdminTestOnayla)sendMailRequest.Data;
                        //mailContent.Replace("{kaynakDokuman}", "<img src=\"" + _url + ttc.TercumanTest.TestResmi + "\" />");                        
                        break;
                    case MailTypeEnum.BireyselRegistration:
                        mailItem.MailSender = MailSenderTypeEnum.User;
                        mailItem.Subject = "Hoşgeldiniz";
                        break;
                    case MailTypeEnum.CeviriTamamlandi:
                        //mailItem.MailSender = MailSenderTypeEnum.User;
                        //mailItem.Subject = "JobNo:" + Orderid + " " + Konu;
                        //var mdCeviriTamamlandi = (MailDataDto.CeviriTamamlandi)sendMailRequest.Data;
                        //string adSoyad = mdCeviriTamamlandi.KurumAdi;
                        //if (adSoyad != "")
                        //{
                        //    adSoyad += " - Gönderen: " + mdCeviriTamamlandi.adsoyad;
                        //}
                        //mailContent.Replace("{adsoyad}", adSoyad);

                        break;
                    case MailTypeEnum.Dekont:
                        break;
                    case MailTypeEnum.EditorKontrolleriBitirdi:
                        break;
                    case MailTypeEnum.EmailConfirmation:
                        break;
                    case MailTypeEnum.IhaleAlindi:
                        break;
                    case MailTypeEnum.IhaleyeIsAc:
                        break;
                    case MailTypeEnum.InsanKaynaklari:
                        break;
                    case MailTypeEnum.KontrolBekleyenDosyalar:
                        break;
                    case MailTypeEnum.KurumsalRegistration:
                        break;
                    case MailTypeEnum.MusteriRevizeGonder:
                        break;
                    case MailTypeEnum.OdemeAlindi:
                        break;
                    case MailTypeEnum.RevizeFinal:
                        break;
                    case MailTypeEnum.SiparisIptal:
                        break;
                    case MailTypeEnum.SiparisTamamlandi:
                        break;
                    case MailTypeEnum.Siparis:
                        break;
                    case MailTypeEnum.TeklifCevabi:
                        break;
                    case MailTypeEnum.TeklifRedAciklamasi:
                        break;
                    case MailTypeEnum.TeklifRededildi:
                        break;
                    case MailTypeEnum.TeklifTalebi:
                        break;
                    case MailTypeEnum.TercumanIsGonder:
                        break;
                    case MailTypeEnum.TercumanSiparisIptal:
                        break;
                    case MailTypeEnum.TercumeyiBitirdim:
                        break;
                    case MailTypeEnum.TestGonder:
                        break;
                    case MailTypeEnum.TestTercumeOnaylandi:
                        break;
                    case MailTypeEnum.TestTercumeRedEdildi:
                        break;
                    case MailTypeEnum.TranslatorWelcomingMail:
                        break;
                    default:
                        break;
                }
                mailContent.Replace("{url}", _url);
                foreach (var item in fields)
                {
                    mailContent.Replace("{" + item.Name + "}", item.Data.ToString());
                }
         
                mailItem.Message = mailContent.ToString();
                
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
