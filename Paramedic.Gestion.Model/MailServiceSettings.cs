using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public class MailServiceSettings
    {
        #region Properties

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Smtp { get; set; }

        public string SmtpPort { get; set; }

        public bool EnableSsl { get; set; }

        #endregion

        #region Constructors

        public MailServiceSettings(string userName, string password, string smtp, string smtpPort, bool enableSsl)
        {
            this.UserName = userName;
            this.Password = password;
            this.Smtp = smtp;
            this.SmtpPort = smtpPort;
            this.EnableSsl = enableSsl;
        }

        #endregion
    }
}
