using Autofac.Integration.WebApi;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace TangentIdeas.Mail.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 

            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            config.DependencyResolver = new AutofacWebApiDependencyResolver(Program.Container);

            appBuilder.UseWebApi(config);
        }
    }
}
