using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Entities.System;
using System.Collections.Generic;

namespace TangentIdeas.Mail.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IsJsonOkRegistrationSocial()
        {
            //Mock<IUserProvider> mock = new Mock<IUserProvider>();

            ////Arrange
            //SendMailRequestDto test = new SendMailRequestDto();
            //test.MailSender = MailSenderTypeEnum.System;
            //test.Message = "Test Mesajıdır.";
            //test.Subject = "Test Mesajıdır.";
            //test.To = new List<MailTarget> { new MailTarget { MailAddres = "ondercalbay@hotmail.com" } };

            //UserController controller = new UserController(mock.Object);
            //Answer answerOk = new Answer
            //{
            //    Result = 0,
            //    Caption = "Ok",
            //    Description = null,
            //    Data = null
            //};

            ////Action
            //var answer = controller.RegistrationSocial(new RegistrationSocialRequest
            //{
            //    SocialProvider = 1,
            //    Email = "lincoln@usa.gov",
            //    PhoneNumber = "123",
            //    SocialProviderId = "0987654321"
            //});

            ////Assert
            //Assert.AreEqual(JsonConvert.SerializeObject(answerOk),
            //                JsonConvert.SerializeObject(answer));
        }
    }
}
