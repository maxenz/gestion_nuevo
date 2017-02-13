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
        #region Properties

        private static volatile MailService instance;
        private static object syncRoot = new object();
        private string urlGestion = ConfigurationManager.AppSettings["urlGestion"].ToString();
        private string administratorMail = ConfigurationManager.AppSettings["administratorMail"].ToString();

        #endregion

        #region Constructors

        private MailService()
        {

        }

        public static MailService Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new MailService();
                    }
                }

                return instance;
            }
        }


        #endregion

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
                smtpClient.EnableSsl = msg.MailConfiguration.EnableSsl;
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

            StringBuilder body = new StringBuilder();
            body = body.AppendLine("<h2>Shaman SGE - Sistema de tickets</h2> <br />");

            if (ticketEvento.TicketTipoEventoType == TicketEventoType.Answer)
            {
                body = body.AppendLine(string.Format("Han respondido tu mensaje con el asunto: {0}. <br />", ticketEvento.Ticket.Asunto));
                string href = string.Format("<a href=\"{0}/MisTickets/Edit/{1}\"> Aquí </a>", urlGestion, ticketEvento.Ticket.Id);
                body = body.AppendLine("Para acceder a la respuesta haga click: " + href);
            }
            else
            {
                body = body.AppendLine(string.Format("El usuario {0} ha realizado una consulta con el asunto: {1}. <br />", ticketEvento.Ticket.Usuario.UserName, ticketEvento.Ticket.Asunto));
                body = body.AppendLine(string.Format("El mensaje es el siguiente: <b> {0} </b> <br/>", ticketEvento.Descripcion));
                string href = string.Format("<a href=\"{0}/MisTickets/Edit/{1}\"> Aquí </a>", urlGestion, ticketEvento.Ticket.Id);
                body = body.AppendLine("Para responder la consulta haga click: " + href);
                // --> Si es una pregunta, que llegue al mail administrador
                ticketEvento.Ticket.Usuario.Email = administratorMail;
            }

            Message msg = new EmailMessage(body.ToString(), administratorMail, ticketEvento.Ticket.Usuario.Email, ticketEvento.Ticket.Asunto);
            Send(msg);
        }

        public void SendNewAdminTicketMail(TicketEvento ticketEvento)
        {
            StringBuilder body = new StringBuilder();
            body = body.AppendLine("<h2>Shaman SGE - Sistema de tickets</h2> <br />");

            body = body.AppendLine(string.Format("Se ha generado un ticket para su usuario con el asunto: {0}. <br />", ticketEvento.Ticket.Asunto));
            string href = string.Format("<a href=\"{0}/MisTickets/Edit/{1}\"> Aquí </a>", urlGestion, ticketEvento.Ticket.Id);
            body = body.AppendLine("Para acceder al ticket, haga click: " + href);

            Message msg = new EmailMessage(body.ToString(), administratorMail, ticketEvento.Ticket.Usuario.Email, ticketEvento.Ticket.Asunto);
            Send(msg);
        }

        #endregion
    }
}
