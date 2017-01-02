using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesLicenciasProductos")]
    public class ClientesLicenciasProducto : AuditableEntity<int>
    {
        [Required]
        [Display(Name ="Licencia del Cliente")]
        public int ClientesLicenciaId { get; set; }

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        [ForeignKey("ClientesLicenciaId")]
        public virtual ClientesLicencia ClientesLicencia { get; set; }
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        public virtual ICollection<ClientesLicenciasProductosModulo> ClientesLicenciasProductosModulos { get; set; }


    }
}