using log4net;
using System;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;
using Tangent.CeviriDukkani.Data.Model;
using Tangent.CeviriDukkani.Logging;
using TangetIdeas.MailService.Business.Implementations;
using TangetIdeas.MailService.Business.Interfaces;

namespace TangentIdeas.MailWindowsService
{
    partial class MailWindowsService : ServiceBase
    {
        public MailWindowsService()
        {
            InitializeComponent();
        }
        Timer tmMail = new Timer();

        private IMailService _mailService;
        private readonly ILog _logger = CustomLogger.Logger;
        private Exception _exc;
        protected override void OnStart(string[] args)
        {
            try
            {
                
                tmMail.Interval = 15000;
                tmMail.AutoReset = true;
                tmMail.Enabled = true;
                tmMail.Start();
                tmMail.Elapsed += TmMail_Elapsed;

                CustomLogger.Logger.Info($"Mail service is Start {DateTime.Today}");

                _mailService = new MailService(new CeviriDukkaniModel(), new YandexMailService(), CustomLogger.Logger);

            }
            catch (Exception exc)
            {
                _exc = exc;
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod()} with message {exc.Message}");
            }
        }
        private void TmMail_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmMail.Enabled = false;

            try
            {
                var srMail = _mailService.SendWaitingMails();                
            }
            catch (Exception exc)
            {
                _logger.Error($"Error occured in {MethodBase.GetCurrentMethod()} with message {exc.Message}");
            }

            tmMail.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
