using System.Collections.Generic;
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

		[Display(Name = "Path imagen del Addon")]
		public string PathImagenAddon { get; set; }

		[Display(Name = "Descripción del Addon")]
		public string DescripcionAddon { get; set; }

		public virtual List<ProductosModulosIntento> Intentos { get; set; }

		#endregion

		#region Constructors

		public ProductosModulo()
		{
			this.Intentos = new List<ProductosModulosIntento>();
		}

		#endregion
	}
}