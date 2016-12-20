using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Tickets")]
    public class Ticket : AuditableEntity<int>
    {
        [Required]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        public bool Resuelto { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Required]
        [Display(Name = "Estado de ticket")]
        public int TicketEstadoId { get; set; }

        [Display(Name = "Futura mejora")]
        public bool FuturaMejora { get; set; }

        public virtual IEnumerable<TicketEvento> TicketEventos { get; set; }

        [ForeignKey("UsuarioId")]
        public virtual UserProfile Usuario { get; set; }

        [ForeignKey("TicketEstadoId")]
        public virtual TicketEstado TicketEstado { get; set; }
    }
}
