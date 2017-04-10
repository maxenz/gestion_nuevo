using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
    // --> Este vm se usa para que el admin cree un ticket
    // --> "haciendose pasar" por cliente.
    public class TicketAdminViewModel
    {
        #region Properties

        [Required]
        public string Asunto { get; set; }

        [Required]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int UserProfileId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "Clasificación del ticket")]
        public int TicketsClasificacionId { get; set; }

        #endregion

        #region Public Methods

        public Ticket ToTicket()
        {
            Ticket ticket = new Ticket();
            ticket.TicketEstadoType = TicketEstadoType.NotAnswered;
            ticket.UserProfileId = this.UserProfileId;
            ticket.Asunto = this.Asunto;
            ticket.TicketsClasificacionId = this.TicketsClasificacionId;
            return ticket;
        }

        #endregion
    }
}