using LinqKit;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

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

		public IEnumerable<ProductosModulo> GetProductosModulosForAddon(string license)
		{
			ClientesLicencia cliLic = _clientesLicenciaRepo.FindBy(x => x.Licencia.Serial == license).FirstOrDefault();
			IEnumerable<ProductosModulo> allModules = _prodModRepo
				.GetAll()
				.Where(x => cliLic.ClientesLicenciasProductos.Select(p => p.ProductoId)
				.Contains(x.ProductoId));

			IEnumerable<ProductosModulo> availableModules = null;
			if (cliLic != null)
			{
				availableModules = cliLic
					.ClientesLicenciasProductos
					.SelectMany(x => x.ClientesLicenciasProductosModulos)
					.Select(x => x.ProductosModulo);

				return allModules.Except(availableModules);
			}

			return null;

		}

		public void DeleteClientesLicencia(int id)
		{
			ClientesLicencia clientesLicencia = GetById(id);
			List<ClientesLicenciasProducto> productos = clientesLicencia.ClientesLicenciasProductos.ToList();
			List<ClientesLicenciasProductosModulo> modulos = productos.SelectMany(x => x.ClientesLicenciasProductosModulos).ToList();
			List<ClientesLicenciasProductosModulosHistorial> historial = modulos.SelectMany(x => x.Historial).ToList();
			historial.ForEach(s => _historialRepo.Delete(s));
			modulos.ForEach(s => _clientesLicenciasProductosModuloRepo.Delete(s));
			productos.ForEach(s => _clientesLicenciasProdRepo.Delete(s));
			this.Delete(clientesLicencia);
		}

		public IEnumerable<ClientesLicencia> GetLicencias(VencimientosQueryControllerParametersDTO queryParameters)
		{
			var predicate = GetPredicateByConditions(queryParameters);

			return FindByPage(predicate, "FechaDeVencimiento DESC", queryParameters.PageSize, queryParameters.Page);
		}

		public Expression<Func<ClientesLicencia, bool>> GetPredicateByConditions(VencimientosQueryControllerParametersDTO queryParameters)
		{
			var predicate = PredicateBuilder.New<ClientesLicencia>();

			if (!string.IsNullOrEmpty(queryParameters.SearchDescription))
			{
				predicate = predicate.And(p => (p.Cliente.RazonSocial.Contains(queryParameters.SearchDescription)));
			}

			return predicate;
		}

		#endregion
	}
}
