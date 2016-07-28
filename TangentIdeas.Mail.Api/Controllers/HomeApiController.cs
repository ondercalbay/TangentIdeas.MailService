using System.Net;
using System.Net.Http;
using System.Web.Http;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.WebCore.BaseControllers;

namespace TangentIdeas.Mail.Api.Controllers
{
    [RoutePrefix("api/homeapi")]
    public class HomeApiController : BaseApiController {
        private readonly CeviriDukkaniModel _model;

        public HomeApiController() {
            _model = new CeviriDukkaniModel();
        }

        [HttpGet, Route("hello")]
        public string Hello() {
            return "Hello Api";
        }

        [HttpPost, Route("sendMails")]
        public HttpResponseMessage SendMails([FromBody]SendMailRequestDto sendMailRequest) {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
           


            return result;
        }
    }
}
