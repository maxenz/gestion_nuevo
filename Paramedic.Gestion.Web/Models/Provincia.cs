using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Gestion.Models
{
    [Table("Provincias")]
    public class Provincia
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("Pais")]
        public int PaisID { get; set; }

        [Required]
        [MaxLength(3)]
        public String Codigo { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public virtual IList<Localidad> Localidades { get; set; }

        public virtual Pais Pais { get; set; }
    }
}