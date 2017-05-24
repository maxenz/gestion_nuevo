using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("ClientesLicenciasProductosModulosHistoriales")]
	public class ClientesLicenciasProductosModulosHistorial : AuditableEntity<int>
	{
		#region Properties

		public int ClientesLicenciasProductosModuloId { get; set; }

		[ForeignKey("ClientesLicenciasProductosModuloId")]
		public virtual ClientesLicenciasProductosModulo ClientesLicenciasProductosModulo { get; set; }

		public DateTime FechaVencimiento { get; set; }

		public int ProductosModulosIntentoId { get; set; }

		[ForeignKey("ProductosModulosIntentoId")]
		public virtual ProductosModulosIntento ProductosModulosIntento { get; set; }

		#endregion
	}
}
