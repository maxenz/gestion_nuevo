using System;
using System.Text;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;

using System.Net.Mail;
using Paramedic.Gestion.Service;
using System.Configuration;

namespace SocialMedia.Services
{
    public class MailService : ISocialMediaService
    {
        #region Public Methods

        public void Send(Message message)
        {
            try
            {
                EmailMessage msg = message as EmailMessage;

                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(msg.To);
                MailAddress mailAddress = new MailAddress(msg.From);
                mailMsg.From = mailAddress;
                mailMsg.Subject = msg.Subject;
                mailMsg.Body = message.Body;
                mailMsg.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient(msg.MailConfiguration.Smtp, msg.MailConfiguration.SmtpPort);
                smtpClient.EnableSsl = true;
                smtpClient.Timeout = 10000;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                System.Net.NetworkCredential credentials =
                   new System.Net.NetworkCredential(msg.MailConfiguration.SenderMail, msg.MailConfiguration.SenderPassword);
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
                LoggingService.Instance.Write(LoggingTypes.Information, string.Format("Se envió un mail de ticket a {0}", mailMsg.To));
            }
            catch (Exception ex)
            {
                LoggingService.Instance.Write(LoggingTypes.Error, ex.Message);
            }

        }

        public void SendNewTicketEventoMail(TicketEvento ticketEvento)
        {
            string urlGestion = ConfigurationManager.AppSettings["urlGestion"].ToString();
            string administratorMail = ConfigurationManager.AppSettings["administratorMail"].ToString();
            StringBuilder body = new StringBuilder();
            body = body.AppendLine("Usuario: " + ticketEvento.Ticket.Usuario.UserName + "<br /><br />");
            body = body.AppendLine("Asunto: " + ticketEvento.Ticket.Asunto + "<br /><br />");

            if (ticketEvento.TicketTipoEventoType == TicketEventoType.Answer)
            {
                string href = string.Format("<a href=\"{0}/MisTickets/Edit/{1}\"> Aquí </a>", urlGestion, ticketEvento.TicketId);
                body = body.AppendLine("Para acceder a la respuesta haga click:  " + href);
            } else
            {
                // --> Si es una pregunta, que llegue al mail administrador
                ticketEvento.Ticket.Usuario.Email = administratorMail;
            }

            Message msg = new EmailMessage(body.ToString(), administratorMail, ticketEvento.Ticket.Usuario.Email, ticketEvento.Ticket.Asunto);
            Send(msg);
        }

        #endregion
    }
}
