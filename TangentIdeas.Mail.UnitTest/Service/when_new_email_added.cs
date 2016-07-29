using FluentAssertions;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Entities.System;
using TangetIdeas.MailService.Business.Implementations;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangentIdeas.Mail.UnitTest.Service
{
    [TestFixture]
    public class when_new_email_added
    {
        private MailService _mailService;
        private MailItem mail = new MailItem
        {
            MailSender = MailSenderTypeEnum.System,
            Message = "Test Mesajı",
            Status = MailStatusTypeEnum.Waiting,
            Subject = "Test Konusu",
            To = new List<MailTarget> { new MailTarget { MailAddres = "ondercalbay@hotmail.com" } }

        };

        [SetUp]
        public void SetupTest()
        {
            var modelMock = new Mock<CeviriDukkaniModel>();
            var mailSenderServiceMock = new Mock<IMailSenderService>();

            modelMock.Setup(a => a.Mail.Add(It.IsAny<MailItem>())).Returns(mail);
            modelMock.Setup(a => a.SaveChanges()).Returns(1);

            mailSenderServiceMock.Setup(a => a.SendMail(It.IsAny<MailItem>())).Returns(new ServiceResult
            {
                ServiceResultType = ServiceResultType.Success
            });

            _mailService = new MailService(modelMock.Object, mailSenderServiceMock.Object);

        }

        [Test]
        public void should_add_item_with_data()
        {
            var serviceResult = _mailService.Add(mail);
            serviceResult.ServiceResultType.Should().Be(ServiceResultType.Success);
        }
    }
}
