using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Paramedic.Gestion.Model
{
    [Table("TicketEventos")]
    public class TicketEvento : AuditableEntity<int>
    {

        [Required(ErrorMessage = @"Debe ingresar la descripcion.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Tipo de evento del ticket")]
        public int TicketTipoEventoId { get; set; }

        [Required]
        [Display(Name ="Ticket")]
        public int TicketId { get; set; }

        [Display(Name = "Usuario")]
        public int UserId { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        [ForeignKey("TicketTipoEventoId")]
        public virtual TicketTipoEvento TicketTipoEvento { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile UserProfile { get; set; }
    }
}
