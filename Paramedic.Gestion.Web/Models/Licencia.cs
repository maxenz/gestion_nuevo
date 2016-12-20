using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("Licencias")]
    public class Licencia
    {

        public Licencia()
        {
            Productos = new List<Producto>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public String Serial { get; set; }

        [Display(Name = "Nro. HardkeyNet")]
        public string NumeroDeLlave { get; set; }

        public virtual IList<Producto> Productos { get; set; }

    }
}