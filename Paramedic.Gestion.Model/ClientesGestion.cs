using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesGestiones")]
    public class ClientesGestion : AuditableEntity<int>
    {

        [Required]
        [Display(Name ="Estado")]
        public int EstadoId { get; set; }

        [Required]
        [Display(Name ="Cliente")]
        public int ClienteId { get; set; }

        [Required]
        public string Observaciones { get; set; }

        public byte[] PdfGestion{ get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Recontacto")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRecontacto { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
        [ForeignKey("EstadoId")]
        public virtual Estado Estado { get; set; }

    }
}