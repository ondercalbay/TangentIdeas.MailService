using Autofac;
using Autofac.Integration.WebApi;
using log4net;
using Microsoft.Owin.Hosting;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Domain.Common;
using Tangent.CeviriDukkani.Domain.Dto.Enums;
using Tangent.CeviriDukkani.Domain.Dto.Request;
using Tangent.CeviriDukkani.Domain.Dto.System;
using Tangent.CeviriDukkani.Domain.Mappers;
using Tangent.CeviriDukkani.Logging;
using Tangent.CeviriDukkani.Messaging;
using Tangent.CeviriDukkani.Messaging.Producer;
using TangetIdeas.MailService.Business.Implementations;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangentIdeas.Mail.Api
{
    class Program
    {
       
        static void Main(string[] args)
        {
            string baseAddress = "http://localhost:9000/";

            Bootstrapper();
            Console.WriteLine("Bootstrapper finished");

            var webApp = WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine($"Mail is ready in {baseAddress}");

            //Container.Resolve<MailEventProjection>().Start();
            Console.WriteLine("Projection started...");
            Console.ReadLine();

            Console.WriteLine("Starting to close Mail...");

            //CustomLogger.Logger.Info($"Mail service is down with projections {DateTime.Today}");

            //Container.Resolve<IConnection>().Close();
            Test();

        }

        private static void Test()
        {
            CustomLogger.Logger.Info($"Mail service is Start {DateTime.Today}");
            MailDataDto.BireyselRegistration data = new MailDataDto.BireyselRegistration { adsoyad = "Önder ÇALBAY" };

            SendMailRequestDto mail = new SendMailRequestDto
            {
                Data = data,
                MailType = MailTypeEnum.BireyselRegistration,
                To = new List<string>(new string[] { "ondercalbay@hotmail.com" })
            };
            IMailService mailService = new MailService(new CeviriDukkaniModel(), new YandexMailService(), CustomLogger.Logger);
            var serviceResult = mailService.Add(mail);
            if (serviceResult.ServiceResultType != ServiceResultType.Success)
            {
                Console.WriteLine("Error occure while sending mail.");
            }
        }

        public static void Bootstrapper()
        {
            var builder = new ContainerBuilder();
            builder.RegisterCommons();
            builder.RegisterBusiness();

            var settings = builder.RegisterSettings();
            //builder.RegisterEvents(settings);
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterType<MailEventProjection>().AsSelf().SingleInstance();

            Container = builder.Build();
            //CustomLogger.Logger.Info($"Mail service is up and ready with projections {DateTime.Today}");
        }

        public static IContainer Container { get; set; }
    }

    public static class AutofacExtensions
    {
        public static void RegisterCommons(this ContainerBuilder builder)
        {

            builder.RegisterType<CeviriDukkaniModel>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomMapperConfiguration>().AsSelf().SingleInstance();
            builder.RegisterInstance(CustomLogger.Logger).As<ILog>().SingleInstance();
        }

        public static void RegisterBusiness(this ContainerBuilder builder)
        {
            builder.RegisterType<MailService>().As<IMailService>().InstancePerLifetimeScope();
            builder.RegisterType<YandexMailService>().As<IMailSenderService>().InstancePerLifetimeScope();
        }

        public static void RegisterEvents(this ContainerBuilder builder, Settings settings)
        {
            var connection = new RabbitMqConnectionFactory(settings.RabbitHost, settings.RabbitPort, settings.RabbitUserName, settings.RabbitPassword).CreateConnection();
            var dispatcher = new RabbitMqDispatcherFactory(connection, settings.RabbitExchangeName, CustomLogger.Logger).CreateDispatcher();

            builder.RegisterInstance<IConnection>(connection);
            builder.RegisterInstance<IDispatchCommits>(dispatcher);
        }

        public static Settings RegisterSettings(this ContainerBuilder builder)
        {
            var settings = new Settings
            {
                RabbitExchangeName = ConfigurationManager.AppSettings["RabbitExchangeName"],
                RabbitHost = ConfigurationManager.AppSettings["RabbitHost"],
                RabbitPassword = ConfigurationManager.AppSettings["RabbitPassword"],
                RabbitPort = int.Parse(ConfigurationManager.AppSettings["RabbitPort"]),
                RabbitUserName = ConfigurationManager.AppSettings["RabbitUserName"]
            };

            builder.RegisterInstance(settings);
            return settings;
        }
    }
}