using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Model.Messaging;

namespace Paramedic.Gestion.Model
{
    public class EmailMessage : Message
    {
        #region Properties

        public string Subject { get; set; }

        public MailConfiguration MailConfiguration { get; set; }

        #endregion

        #region Constructors

        public EmailMessage(string body,string from, string to, string subject) : base(body, to)
        {
            this.SocialMediaType = SocialMediaTypes.Mail;
            this.Subject = subject;
            this.From = from;
            this.MailConfiguration = new MailConfiguration();
        }

        #endregion
    }
}
