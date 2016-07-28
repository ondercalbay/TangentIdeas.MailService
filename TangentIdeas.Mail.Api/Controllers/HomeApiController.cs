using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Dto.Request;
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
            return "Hello Api";
        }

        [HttpPost, Route("sendMails")]
        public HttpResponseMessage SendMails([FromBody]SendMailRequestDto sendMailRequest) {
            var result = new HttpResponseMessage(HttpStatusCode.OK);

            _mailService.AddMails(sendMailRequest);



            return result;
        }
    }
}
