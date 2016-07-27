using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.WebCore.BaseControllers;

namespace TangentIdeas.Mail.Api.Controllers {
    [RoutePrefix("api/homeapi")]
    public class HomeApiController : BaseApiController {
        private readonly CeviriDukkaniModel _model;

        public HomeApiController() {
            _model = new CeviriDukkaniModel();
        }

        [HttpGet, Route("hello")]
        public string Hello() {
            return "Hello";
        }

        [HttpPost, Route("sendMails")]
        public HttpResponseMessage SendMails([FromBody]SendMailRequestDto sendMailRequest) {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
           


            return result;
        }
    }
}
