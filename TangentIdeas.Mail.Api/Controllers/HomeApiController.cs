using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Entities.System;
using Tangent.CeviriDukkani.WebCore.BaseControllers;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangentIdeas.Mail.Api.Controllers
{
    [RoutePrefix("api/homeapi")]
    public class HomeApiController : BaseApiController {
        private readonly IMailService _mailService;

        public HomeApiController(IMailService mailService) {
            _mailService = mailService;
        }

        [HttpGet,Route("hello")]
        public string Hello()
        {
            SendMailRequestDto mailItem = new SendMailRequestDto();
            mailItem.MailSender =  MailSenderTypeEnum.User;
            mailItem.Message = "Mesaj";
            mailItem.Subject = "Konu";
            mailItem.To = new List<string> { "ondercalbay@hotmail.com" };

            return JsonConvert.SerializeObject(mailItem);
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
                mailItem.Status = MailStatusTypeEnum.Waiting;
                mailItem.To = sendMailRequest.To.Select(a => new MailTarget { MailAddres = a }).ToList();
                mailItem.CreatedAt = DateTime.Now;
                mailItem.CreatedBy = 1;
                mailItem.Active = true;
                
                var serviceResult = _mailService.Add(mailItem);
            }
            catch
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }
    }
}
