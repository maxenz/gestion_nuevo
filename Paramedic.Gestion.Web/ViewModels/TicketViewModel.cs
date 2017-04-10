using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class TicketViewModel
    {
        #region Properties

        public string Asunto { get; set; }

        [Display(Name = "Última actualización")]
        public DateTime FechaHora { get; set; }

        public string Estado { get; set; }

        public int Id { get; set; }

        public string Usuario { get; set; }

        public string Cliente { get; set; }

        public int ClienteId { get; set; }

        public bool FuturaMejora { get; set; }

        public TicketEstadoType Type { get; set; }

        public Model.TicketsClasificacion TicketsClasificacion { get; set; }

        #endregion

        #region Constructors

        public TicketViewModel(Ticket ticket)
        {
            this.FechaHora = ticket.UpdatedDate;
            this.Id = ticket.Id;
            this.Usuario = ticket.Usuario.UserName;
            this.FuturaMejora = ticket.FuturaMejora;
            this.Type = ticket.TicketEstadoType;
            this.Asunto = ticket.Asunto;
            this.TicketsClasificacion = ticket.TicketsClasificacion;
        }

        #endregion
    }
}