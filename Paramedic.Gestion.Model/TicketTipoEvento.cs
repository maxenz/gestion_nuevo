using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("TicketTipoEventos")]
    public class TicketTipoEvento : AuditableEntity<int>
    {
        [Required]
        [Display(Name = "Tipo de Evento")]
        public String Descripcion { get; set; }
    }
}
