using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Gestion.Models
{
    [Table("TicketEventos")]
    public class TicketEvento
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Evento")]
        public DateTime FechaCreacion { get; set; }

        [Required(ErrorMessage = @"Debe ingresar la descripcion.")]
        [Display(Name = "Descripción")]
        public String Descripcion { get; set; }

        [Required]
        [ForeignKey("TicketTipoEvento")]
        public int TicketTipoEventoID { get; set; }

        [Required]
        [ForeignKey("Ticket")]
        public int TicketID { get; set; }

        [ForeignKey("UserProfile")]
        public int UserID { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public String ImageMimeType { get; set; }

        public virtual TicketTipoEvento TicketTipoEvento { get; set; }
        public virtual Ticket Ticket { get; set; }
        public virtual UserProfile UserProfile { get; set; }

    }
}