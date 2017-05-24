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

        [ForeignKey("ClientesLicenciasProductoId")]
        public virtual ClientesLicenciasProducto ClientesLicenciasProducto { get; set; }

		public virtual List<ClientesLicenciasProductosModulosHistorial> Historial { get; set; }

		#endregion

		#region Constructors

		public ClientesLicenciasProductosModulo()
		{
			this.Historial = new List<ClientesLicenciasProductosModulosHistorial>();
		}

		#endregion
	}
}