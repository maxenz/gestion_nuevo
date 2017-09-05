using Paramedic.Gestion.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesLicenciasProductosModulos")]
    public class ClientesLicenciasProductosModulo : AuditableEntity<int>
    {
		#region Properties

		[Required]
        public int ClientesLicenciasProductoId { get; set; }

        [Required]
        public int ProductosModuloId { get; set; }

		public LicenseModuleStatusType LicenseModuleStatusType { get; set; }

		[ForeignKey("ClientesLicenciasProductoId")]
		public virtual ClientesLicenciasProducto ClientesLicenciasProducto { get; set; }

		public virtual List<ClientesLicenciasProductosModulosHistorial> Historial { get; set; }

		[ForeignKey("ProductosModuloId")]
		public virtual ProductosModulo ProductosModulo { get; set; }

		#endregion

		#region Constructors

		public ClientesLicenciasProductosModulo()
		{
			this.Historial = new List<ClientesLicenciasProductosModulosHistorial>();
		}

		public ClientesLicenciasProductosModulo(int clientesLicenciasProductoId, int productosModuloId) : this()
		{
			this.ClientesLicenciasProductoId = clientesLicenciasProductoId;
			this.ProductosModuloId = productosModuloId;
		}

		#endregion
	}
}