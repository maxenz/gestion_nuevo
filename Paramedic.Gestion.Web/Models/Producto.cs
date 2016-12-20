using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        public int Numero { get; set; }

        [Required]
        public String Descripcion { get; set; }

        public virtual IList<ProductosModulo> ProductosModulos { get; set; }
        public virtual ICollection<Licencia> Licencias { get; set; }
       // public virtual ICollection<ClientesLicencia> ClientesLicencias { get; set; }
    }
}