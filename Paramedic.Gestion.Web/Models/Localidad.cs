using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Gestion.Models
{
    [Table("Localidades")]
    public class Localidad
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Provincia")]
        public int ProvinciaID { get; set; }

        [Required]
        [MaxLength(3)]
        public String Codigo { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public virtual Provincia Provincia { get; set; }
    }
}