using Paramedic.Gestion.Model;
using System.Web;

namespace Paramedic.Gestion.Web.Converters
{
    public static class TicketConverter
    {
        public static Ticket CreateTicketWithEvent(Ticket ticket, string description, HttpPostedFileBase image, int userId)
        {
            TicketEvento te = new TicketEvento(description, userId, Model.Enums.TicketEventoType.Answer);

            if (image != null)
            {
                te.ImageMimeType = image.ContentType;
                te.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(te.ImageData, 0, image.ContentLength);
            }
            image.InputStream.Read(te.ImageData, 0, image.ContentLength);

            ticket.TicketEventos.Add(te);
            return ticket;
        }    
    }
}