using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TangentIdeas.Mail.Api.Controllers
{
    [RoutePrefix("api/homeapi")]
    public class HomeApiController:ApiController
    {
        [HttpGet,Route("hello")]
        public string Hello() {
        return "Hello";
        }
    }
}
