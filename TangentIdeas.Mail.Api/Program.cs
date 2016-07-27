using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangentIdeas.Mail.Api
{
    class Program
    {
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            var webApp = WebApp.Start<Startup>(url: baseAddress);

            Console.ReadLine();
        }
    }
}
