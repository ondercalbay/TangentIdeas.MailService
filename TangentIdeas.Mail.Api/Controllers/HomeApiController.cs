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

        //[HttpGet,Route("hello")]
        //public string Hello()
        //{
        //    SendMailRequestDto mailItem = new SendMailRequestDto();
        //    mailItem.MailSender =  MailSenderTypeEnum.User;
        //    mailItem.Message = "Mesaj";
        //    mailItem.Subject = "Konu";
        //    mailItem.To = new List<string> { "ondercalbay@hotmail.com" };

        //    return JsonConvert.SerializeObject(mailItem);
        //}


        [HttpPost, Route("sendMails")]
        public HttpResponseMessage SendMails([FromBody]SendMailRequestDto sendMailRequest) {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            try
            {
                var serviceResult = _mailService.Add(sendMailRequest);
            }
            catch
            {
                result = new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return result;
        }
    }
}
