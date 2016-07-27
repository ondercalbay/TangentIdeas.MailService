using System.Collections.Generic;

namespace TangentIdeas.Mail.Api.Controllers {
    public class SendMailRequestDto {
        public List<string> MailAddress { get; set; }
    }
}