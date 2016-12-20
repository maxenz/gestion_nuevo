using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("Revendedores")]
    public class Revendedor
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Required]
        [Display(Name = "Comisión")]
        public Double Comision { get; set; }

        [Required]
        [Display(Name = "Contrato?")]
        public Boolean BajoContrato { get; set; }
    }
}