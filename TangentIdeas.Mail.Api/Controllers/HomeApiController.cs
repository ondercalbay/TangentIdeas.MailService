using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IMailService _mailService;

        public HomeApiController(IMailService mailService) {
            _mailService = mailService;
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
