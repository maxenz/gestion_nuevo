using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("TicketEstados")]
    public class TicketEstado : AuditableEntity<int>
    {
        [Required]
        [Display(Name = "Estado de Ticket")]
        public String Descripcion { get; set; }
    }
}
