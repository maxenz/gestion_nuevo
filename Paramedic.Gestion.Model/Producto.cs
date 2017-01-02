using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Productos")]
    public class Producto : AuditableEntity<int>
    {
        [Required]
        public int Numero { get; set; }

        [Required]
        public string Descripcion { get; set; }

        public virtual ICollection<ProductosModulo> ProductosModulos { get; set; }
        public virtual ICollection<Licencia> Licencias { get; set; }
    }
}