using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesLicenciasProductosModulos")]
    public class ClientesLicenciasProductosModulo : AuditableEntity<int>
    {
        [Required]
        public int ClientesLicenciasProductoId { get; set; }

        [Required]
        public int ProductosModuloId { get; set; }

        [ForeignKey("ClientesLicenciasProductoId")]
        public virtual ClientesLicenciasProducto ClientesLicenciasProducto { get; set; }


    }
}