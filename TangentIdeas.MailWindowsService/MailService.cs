using System;
using System.ServiceProcess;
using System.Timers;
using TangentIdeas.MailWindowsService.Implementations;
using TangentIdeas.MailWindowsService.Interfaces;
using TangentIdeas.MailWindowsService.Model;

namespace TangentIdeas.MailWindowsService
{
    partial class MailService : ServiceBase
    {
        public MailService()
        {
            InitializeComponent();
        }
        Timer tmMail = new Timer();

        private IMailService _mailService;

        protected override void OnStart(string[] args)
        {
            tmMail.Interval = 15000;
            tmMail.AutoReset = true;
            tmMail.Enabled = true;
            tmMail.Start();
            tmMail.Elapsed += TmMail_Elapsed;

            _mailService = new Implementations.MailService(new MailServiceModel(), new YandexMailService());
        }
        
        private void TmMail_Elapsed(object sender, ElapsedEventArgs e)
        {
            tmMail.Enabled = false;

            try
            {
                var srMail = _mailService.SendWaitingMails();                
            }
            catch (Exception ex)
            {
                throw;
            }

            tmMail.Enabled = true;
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }
    }
}
