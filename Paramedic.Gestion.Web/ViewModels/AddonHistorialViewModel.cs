namespace Paramedic.Gestion.Web.ViewModels
{
	public class AddonHistorialViewModel
	{
		#region Properties

		public int ProductosModulosIntentoId { get; set; }

		public int ClientesLicenciasProductoId { get; set; }

		public int ProductosModuloId { get; set; }

		public string ModuloDescripcion { get; set; }

		public int TrialCantidadDias { get; set; }

		public string LicenseSerial { get; set; }

		#endregion

		#region Constructors

		public AddonHistorialViewModel() { }

		public AddonHistorialViewModel(
			Model.ProductosModulosIntento intento,
			Model.ClientesLicenciasProducto clientesLicenciaProducto,
			int productosModuloId)
		{
			this.ProductosModulosIntentoId = intento.Id;
			this.ClientesLicenciasProductoId = clientesLicenciaProducto.Id;
			this.ProductosModuloId = productosModuloId;
			this.ModuloDescripcion = intento.ProductosModulo.Descripcion;
			this.TrialCantidadDias = intento.Dias;
			this.LicenseSerial = clientesLicenciaProducto.ClientesLicencia.Licencia.Serial;
		}

		#endregion
	}
}