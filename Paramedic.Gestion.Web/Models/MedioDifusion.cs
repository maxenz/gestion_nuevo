using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("MediosDifusion")]
    public class MedioDifusion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name="Visible en Mapa")]
        public bool MapaVisible { get; set; }

        [Required]
        public String Descripcion { get; set; }

    }
}