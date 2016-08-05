using log4net;
using RabbitMQ.Client;
using System;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Event.MailEvents;
using Tangent.CeviriDukkani.Messaging.Consumer;
using Tangent.CeviriDukkani.Messaging.Producer;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangentIdeas.Mail.Api
{
    public class MailEventProjection
    {
        private readonly IMailService _mailService;
        private readonly IDispatchCommits _dispatcher;
        private readonly RabbitMqSubscription _consumer;

        public MailEventProjection(IConnection connection, IMailService mailService, IDispatchCommits dispatcher, ILog logger)
        {
            _mailService = mailService;
            _dispatcher = dispatcher;
            _consumer = new RabbitMqSubscription(connection, "Cev-Exchange", logger);
            _consumer
                .WithAppName("mail-projection")
                .WithEvent<SendMailEvent>(Handle);
        }

        public void Start()
        {
            _consumer.Subscribe();
        }

        public void Stop()
        {
            _consumer.StopSubscriptionTasks();
        }

        public void Handle(SendMailEvent sendMailEvent)
        {
            SendMailRequestDto mail = new SendMailRequestDto
            {
                Data = sendMailEvent.Data,
                MailType = sendMailEvent.MailType,                
                To = sendMailEvent.To
            };

            var serviceResult = _mailService.Add(mail);
            if (serviceResult.ServiceResultType != ServiceResultType.Success)
            {
                Console.WriteLine("Error occure while sending mail.");
            }
        }
    }
}