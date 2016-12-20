using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesGestiones")]
    public class ClientesGestion
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Estado")]
        public int EstadoID { get; set; }

        [Required]
        [ForeignKey("Cliente")]
        public int ClienteID { get; set; }

        [Required]
        public String Observaciones { get; set; }

        public byte[] PdfGestion{ get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Recontacto")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime FechaRecontacto { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual Estado Estado { get; set; }

    }
}