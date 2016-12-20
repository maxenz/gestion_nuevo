using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("Tickets")]
    public class Ticket
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Ticket")]
        public DateTime FechaCreacion { get; set; }

        [Required]
        [Display(Name = "Asunto")]
        public String Asunto { get; set; }

        public bool Resuelto { get; set; }

        [Required]
        [ForeignKey("Usuario")]
        public int UsuarioID { get; set; }

        [Required]
        [ForeignKey("TicketEstado")]
        public int TicketEstadoID { get; set; }

        [Display(Name="Futura mejora")]
        public bool FuturaMejora { get; set; }

        public virtual IList<TicketEvento> TicketEventos { get; set; }
        public virtual UserProfile Usuario { get; set; }
        public virtual TicketEstado TicketEstado { get; set; }

    }
}