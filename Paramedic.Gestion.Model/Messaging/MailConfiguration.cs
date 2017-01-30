using System;
using System.Configuration;

namespace Paramedic.Gestion.Model.Messaging
{
   public class MailConfiguration
    {
        #region Properties

        public int SmtpPort { get; set; }

        public string Smtp { get; set; }

        public string SenderMail { get; set; }

        public string SenderPassword { get; set; }

        #endregion

        #region Constructors

        public MailConfiguration()
        {
            var appSettings = ConfigurationManager.AppSettings;
            this.SmtpPort = Convert.ToInt32(appSettings["smtpPort"]);
            this.Smtp = appSettings["smtp"];
            this.SenderMail = appSettings["administratorMail"];
            this.SenderPassword = appSettings["administratorMailPassword"];
        }

        public MailConfiguration(string smtp, int smtpPort, string mail, string password)
        {
            this.Smtp = smtp;
            this.SmtpPort = smtpPort;
            this.SenderMail = mail;
            this.SenderPassword = password;
        }

        #endregion
    }
}
