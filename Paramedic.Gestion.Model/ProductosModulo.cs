using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ProductosModulos")]
    public class ProductosModulo : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [Display(Name = "Producto")]
        public int ProductoId { get; set; }

        public string Codigo { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        #endregion
    }
}