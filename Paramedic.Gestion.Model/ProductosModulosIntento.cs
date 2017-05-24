using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("ProductosModulosIntentos")]
	public class ProductosModulosIntento : AuditableEntity<int>
	{
		#region Properties

		[Required]
		public int ProductosModuloId { get; set; }

		[ForeignKey("ProductosModuloId")]
		public virtual ProductosModulo ProductosModulo { get; set; }

		public int Dias { get; set; }

		public int Orden { get; set; }

		#endregion
	}
}
