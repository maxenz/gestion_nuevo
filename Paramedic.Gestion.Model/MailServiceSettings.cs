using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Paramedic.Gestion.Model
{
    public class MailServiceSettings
    {
        #region Properties

        [Display(Name="Usuario del mail")]
        [GenericRequired]
        public string UserName { get; set; }

        [GenericRequired]
        [Display(Name ="Password del mail")]
        public string Password { get; set; }

        [GenericRequired]
        [Display(Name ="Smtp del dominio del mail")]
        public string Smtp { get; set; }

        [GenericRequired]
        [Display(Name ="Puerto de salida")]
        public int SmtpPort { get; set; }

        [Display(Name ="Habilitar SSL?")]
        public bool EnableSsl { get; set; }
        
        #endregion

        #region Constructors

        public MailServiceSettings(string userName, string password, string smtp, int smtpPort, bool enableSsl)
        {
            this.UserName = userName;
            this.Password = password;
            this.Smtp = smtp;
            this.SmtpPort = smtpPort;
            this.EnableSsl = enableSsl;
        }

        public MailServiceSettings() { }

        #endregion
    }
}
