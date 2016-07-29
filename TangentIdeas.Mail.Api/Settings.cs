using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TangentIdeas.Mail.Api
{
    public class Settings
    {
        public string RabbitExchangeName { get; internal set; }
        public string RabbitHost { get; internal set; }
        public string RabbitPassword { get; internal set; }
        public int RabbitPort { get; internal set; }
        public string RabbitUserName { get; internal set; }
    }
}
