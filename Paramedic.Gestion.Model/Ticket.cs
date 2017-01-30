using Paramedic.Gestion.Model.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Tickets")]
    public class Ticket : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int UserProfileId { get; set; }

        [Required]
        [Display(Name = "Estado de ticket")]
        public TicketEstadoType TicketEstadoType { get; set; }

        [Display(Name = "Futura mejora")]
        public bool FuturaMejora { get; set; }

        public virtual ICollection<TicketEvento> TicketEventos { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile Usuario { get; set; }

        #endregion

        #region Constructors

        public Ticket()
        {
            this.TicketEventos = new List<TicketEvento>();
        }

        #endregion

    }
}
