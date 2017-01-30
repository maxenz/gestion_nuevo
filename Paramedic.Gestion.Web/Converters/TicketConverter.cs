using Paramedic.Gestion.Model;
using System.Web;
using Paramedic.Gestion.Model.Enums;
using SocialMedia.Services;

namespace Paramedic.Gestion.Web.Converters
{
    public static class TicketConverter
    {
        public static Ticket CreateTicketWithEvent(Ticket ticket, string description, HttpPostedFileBase image, TicketEventoType ticketEventoType, int userProfileId)
        {
            MailService mailService = new MailService();
            TicketEvento te = new TicketEvento(description, userProfileId, ticketEventoType);

            if (image != null)
            {
                te.ImageMimeType = image.ContentType;
                te.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(te.ImageData, 0, image.ContentLength);
            }

            te.Ticket = ticket;

            ticket.TicketEventos.Add(te);
            mailService.SendNewTicketEventoMail(te);
            return ticket;
        }    
    }
}