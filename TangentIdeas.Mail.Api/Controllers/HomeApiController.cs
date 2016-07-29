using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Entities.System;
using Tangent.CeviriDukkani.WebCore.BaseControllers;
using TangetIdeas.MailService.Business.Implementations;
using TangetIdeas.MailService.Business.Interfaces;



namespace TangentIdeas.Mail.Api.Controllers
{
    [RoutePrefix("api/homeapi")]
    public class HomeApiController : BaseApiController {
        private readonly CeviriDukkaniModel _model;
        private IMailService _mailService;

        public HomeApiController() {
            _model = new CeviriDukkaniModel();
            _mailService = new MailService(new CeviriDukkaniModel(), new YandexMailService());
        }

        [HttpGet, Route("hello")]
        public string Hello() {
            SendMailRequestDto test = new SendMailRequestDto();
            test.MailSender = MailSenderTypeEnum.System;
            test.Message = "Test Mesajıdır.";
            test.Subject = "Test Mesajıdır.";
            test.To = new List<string> { "ondercalbay@hotmail.com" } ;
            
            return JsonConvert.SerializeObject(test);
        }

        [HttpPost, Route("sendMails")]
        public HttpResponseMessage SendMails([FromBody]SendMailRequestDto sendMailRequest) {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                MailItem mailItem = new MailItem();
                mailItem.MailSender = sendMailRequest.MailSender;
                mailItem.Message = sendMailRequest.Message;
                mailItem.Subject = sendMailRequest.Subject;
                mailItem.To = new List<MailTarget>();
                foreach (var item in sendMailRequest.To)
                {
                    mailItem.To.Add(new MailTarget { MailAddres = item });
                }
                var serviceResult = _mailService.Add(mailItem);

                serviceResult = _mailService.SendMail((MailItem)serviceResult.Data);
            }
            catch
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }
    }
}
