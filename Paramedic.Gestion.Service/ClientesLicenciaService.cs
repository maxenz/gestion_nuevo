using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Paramedic.Gestion.Service
{
	public class ClientesLicenciaService : EntityService<ClientesLicencia>, IClientesLicenciaService
	{
		#region Properties

		IUnitOfWork _unitOfWork;
		IClientesLicenciaRepository _clientesLicenciaRepo;
		IProductosModuloRepository _prodModRepo;
		IClientesLicenciasProductoRepository _clientesLicenciasProdRepo;
		IClientesLicenciasProductosModuloRepository _clientesLicenciasProductosModuloRepo;
		IProductosModulosIntentoRepository _productosModulosIntentoRepo;
		IClientesLicenciasProductosModulosHistorialRepository _historialRepo;

		#endregion

		#region Constructors

		public ClientesLicenciaService(
			IUnitOfWork unitOfWork,
			IClientesLicenciaRepository clientesLicenciaRepo,
			IProductosModuloRepository prodModRepo,
			IClientesLicenciasProductoRepository clientesLicenciasProdRepo,
			IClientesLicenciasProductosModuloRepository clientesLicenciasProductosModuloRepo,
			IProductosModulosIntentoRepository productosModulosIntentoRepo,
			IClientesLicenciasProductosModulosHistorialRepository historialRepo
			) : base(
		  unitOfWork,
		  clientesLicenciaRepo
		  )
		{
			_unitOfWork = unitOfWork;
			_clientesLicenciaRepo = clientesLicenciaRepo;
			_prodModRepo = prodModRepo;
			_clientesLicenciasProdRepo = clientesLicenciasProdRepo;
			_clientesLicenciasProductosModuloRepo = clientesLicenciasProductosModuloRepo;
			_productosModulosIntentoRepo = productosModulosIntentoRepo;
			_historialRepo = historialRepo;
	}

		#endregion

		#region Public Methods

		public ClientesLicencia GetById(int id)
		{
			return _clientesLicenciaRepo.GetById(id);
		}

		public ClientesLicenciasProductosModulosHistorial GetAddonHistorial(string license, int prodModId)
		{
			ClientesLicencia cliLic = _clientesLicenciaRepo.GetByLicenseNumber(license);
			ProductosModulo modulo = _prodModRepo.GetById(prodModId);
			ClientesLicenciasProducto cliLicProd = _clientesLicenciasProdRepo.GetByLicenseAndProduct(cliLic.Id, modulo.ProductoId);

			ClientesLicenciasProductosModulo cliLicProdMod = _clientesLicenciasProductosModuloRepo.GetByLicenseAndModule(cliLicProd.Id, modulo.Id);

			IEnumerable<ProductosModulosIntento> intentos = _productosModulosIntentoRepo.GetByModuloId(prodModId);

			IEnumerable<int> historial =
				_historialRepo
				.FindBy(x => x.ClientesLicenciasProductosModuloId == cliLicProdMod.Id)
				.Select(x => x.ProductosModulosIntentoId);

			foreach(ProductosModulosIntento intento in intentos)
			{
				if (!historial.Contains(intento.Id))
				{
					ClientesLicenciasProductosModulosHistorial hist =
						new ClientesLicenciasProductosModulosHistorial(cliLicProdMod, intento);
					return hist;
				}
			}

			return null;
		}

		public List<ClientesLicenciasProductosModulo> GetProductosModulosForAddon(string license)
		{
			ClientesLicencia cliLic = _clientesLicenciaRepo.FindBy(x => x.Licencia.Serial == license).FirstOrDefault();
			if (cliLic != null)
			{
				return cliLic.ClientesLicenciasProductos.SelectMany(x => x.ClientesLicenciasProductosModulos).ToList();
			}

			return null;

		}

		#endregion
	}
}
