using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Licencias")]
    public class Licencia : AuditableEntity<int>
    {

        public Licencia()
        {
            Productos = new List<Producto>();
        }

        [Required]
        public string Serial { get; set; }

        [Display(Name = "Nro. HardkeyNet")]
        public string NumeroDeLlave { get; set; }

        public virtual ICollection<Producto> Productos { get; set; }

    }
}