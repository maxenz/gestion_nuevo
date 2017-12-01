using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using System.Net;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Web.ViewModels;
using System;
using Paramedic.Gestion.Model.Helpers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Paramedic.Gestion.TrialsExpirationManager;
using System.Web.Script.Serialization;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador, ColaboradorCliente")]
	public class ClientesLicenciasController : Controller
	{
		#region Properties

		IClientesLicenciaService _ClientesLicenciaService;
		ILicenciaService _LicenciaService;
		IClienteService _ClienteService;
		ISitioService _SitioService;
		IProductoService _ProductoService;
		IClientesLicenciasProductoService _ClientesLicenciasProductoService;
		IClientesLicenciasProductosModuloService _ClientesLicenciasProductosModuloService;
		IClientesLicenciasProductosModulosHistorialService _HistorialService;
		#endregion

		#region Constructors

		public ClientesLicenciasController(
			IClientesLicenciaService ClientesLicenciaService,
			ILicenciaService LicenciaService,
			IClienteService ClienteService,
			ISitioService SitioService,
			IProductoService ProductoService,
			IClientesLicenciasProductoService ClientesLicenciasProductoService,
			IClientesLicenciasProductosModuloService ClientesLicenciasProductosModuloService,
			IClientesLicenciasProductosModulosHistorialService HistorialService)
		{
			_ClientesLicenciaService = ClientesLicenciaService;
			_LicenciaService = LicenciaService;
			_ClienteService = ClienteService;
			_SitioService = SitioService;
			_ProductoService = ProductoService;
			_ClientesLicenciasProductoService = ClientesLicenciasProductoService;
			_ClientesLicenciasProductosModuloService = ClientesLicenciasProductosModuloService;
			_HistorialService = HistorialService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(int clienteId, string searchName = null, int page = 1)
		{
			IEnumerable<ClientesLicencia> licencias =
				_ClientesLicenciaService
				.FindBy(x => x.ClienteId == clienteId)
				.OrderByDescending(x => x.Licencia.Serial)
				.ToList();

			if (licencias == null)
			{
				return HttpNotFound();
			}

			ViewBag.ClienteId = clienteId;

			return PartialView("_ClientesLicencias", licencias);
		}

		public ActionResult GetModAsignados(int clientesLicenciaId, int productoId)
		{
			IList<ProductosModulo> modulos = _ProductoService.FindBy(x => x.Id == productoId).FirstOrDefault().ProductosModulos.ToList();
			LlenarProductosModulos(modulos, clientesLicenciaId, productoId);

			return PartialView("_ModulosAsignados", modulos);
		}

		public string SetModAsignados(int clientesLicenciaId, int productoId, string[] vModAsignados)
		{
			ClientesLicenciasProducto licenciasProducto =
				_ClientesLicenciaService
				.FindBy(x => x.Id == clientesLicenciaId)
				.FirstOrDefault()
				.ClientesLicenciasProductos
				.Where(x => x.ProductoId == productoId)
				.FirstOrDefault();

			UpdateProductosModulosAsignados(vModAsignados, licenciasProducto);

			return "ok";

		}

		public ActionResult Create(string clienteId)
		{
			ViewBag.Cliente_ID = clienteId;
			if (getLicenciasDisponibles().Count > 0)
			{
				ViewBag.Licencias = getLicenciasDisponibles();
				ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial");
				ViewBag.Sitios = _SitioService.GetAll().OrderBy(x => x.Url).ToList();

				return View();
			}
			else
			{
				TempData["noMoreLicence"] = "No hay licencias disponibles para agregar";
				return RedirectToAction("Edit", "Clientes", new { id = clienteId });
			}
		}

		[HttpPost]
		public ActionResult Create(ClientesLicencia clienteslicencia)
		{
			if (ModelState.IsValid)
			{
				_ClientesLicenciaService.Create(clienteslicencia);
				return RedirectToAction("Edit", "Clientes", new { id = clienteslicencia.ClienteId });
			}

			ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial", clienteslicencia.ClienteId);
			ViewBag.Licencias = getLicenciasDisponibles();
			ViewBag.Sitios = _SitioService.GetAll().OrderBy(x => x.Url).ToList();

			return View(clienteslicencia);
		}

		public ActionResult Edit(int id = 0)
		{
			ViewBag.Cliente_ID = id;
			ClientesLicencia clienteslicencia = _ClientesLicenciaService.GetById(id);

			LlenarProductosAsignados(clienteslicencia);
			if (clienteslicencia == null)
			{
				return HttpNotFound();
			}
			ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial", clienteslicencia.ClienteId);
			ViewBag.Licencias = getLicenciasDisponibles(clienteslicencia.Licencia.Id);
			ViewBag.Sitios = _SitioService.GetAll().OrderBy(x => x.Url).ToList();
			return View(clienteslicencia);

		}

		[HttpPost]
		public ActionResult Edit(ClientesLicencia cliLic, string[] selectedProductos)
		{
			if (cliLic == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			if (ModelState.IsValid)
			{
				_ClientesLicenciaService.Update(cliLic);
			}

			ClientesLicencia licenciaToUpdate = _ClientesLicenciaService.GetById(cliLic.Id);

			UpdateLicenciasProductos(selectedProductos, licenciaToUpdate);

			ViewBag.Sitios = _SitioService.GetAll().OrderBy(x => x.Url).ToList();

			return RedirectToAction("Edit", "Clientes", new { id = cliLic.ClienteId });

		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			ClientesLicencia clienteslicencia = _ClientesLicenciaService.GetById(id);
			_ClientesLicenciaService.DeleteClientesLicencia(id);		
			return RedirectToAction("Index", routeValues: new { ClienteID = clienteslicencia.ClienteId });
		}

		[AllowAnonymous]
		[HttpGet]
		public JsonResult GetExpiredLicenses()
		{
			DateTime dateToAlert = DateTime.Now.Date.AddDays(-20);
			IEnumerable<ClientesLicencia> clientesLicencias = _ClientesLicenciaService
				.GetAll()
				.Where(x => (x.FechaDeVencimiento.Date <= dateToAlert) && (bool) (x.FechaDeVencimiento != SqlSmallDateTime.MinValue) && (x.FechaDeVencimiento < dateToAlert.AddDays(90)));
			return Json(
				clientesLicencias.Select(x => new
				{
					Cliente = x.Cliente.RazonSocial,
					NroLicencia = x.Licencia.Serial
				}), JsonRequestBehavior.AllowGet);
		}

		[AllowAnonymous]
		[HttpGet]
		public JsonResult GetExpiredLicenseSupports()
		{
			DateTime dateToAlert = DateTime.Now.Date.AddDays(-20);
			IEnumerable<ClientesLicencia> clientesLicencias = _ClientesLicenciaService
				.GetAll()
				.Where(x => (x.FechaVencimientoSoporte.Date <= dateToAlert) && (bool)(x.FechaVencimientoSoporte != SqlSmallDateTime.MinValue) && (x.FechaVencimientoSoporte < dateToAlert.AddDays(90)));
			return Json(
				clientesLicencias.Select(x => new
				{
					Cliente = x.Cliente.RazonSocial,
					NroLicencia = x.Licencia.Serial
				}), JsonRequestBehavior.AllowGet);
		}

		public ActionResult GetVencimientosCount()
		{
			IEnumerable<LicenciaDTO> licencias = JsonConvert.DeserializeObject<IEnumerable<LicenciaDTO>>(new JavaScriptSerializer().Serialize(GetExpiredLicenses().Data));
			IEnumerable<LicenciaDTO> supports = JsonConvert.DeserializeObject<IEnumerable<LicenciaDTO>>(new JavaScriptSerializer().Serialize(GetExpiredLicenseSupports().Data));
			int count = licencias.Count() + supports.Count();
			return PartialView("~/Views/Shared/_NotificacionVencimientos.cshtml", count);
		}

		#endregion

		#region Private Methods

		private void UpdateProductosModulosAsignados(string[] selectedModulos, ClientesLicenciasProducto cliLicProd)
		{
			var licProdMod = _ClientesLicenciasProductoService.FindBy(x => x.Id == cliLicProd.Id).FirstOrDefault().ClientesLicenciasProductosModulos.ToList();

			if (selectedModulos == null)
			{
				foreach (var lpm in licProdMod)
				{
					_ClientesLicenciasProductosModuloService.Delete(lpm);
				}
				return;
			}

			var selectedModulosHS = new HashSet<string>(selectedModulos);
			var prodMod = new HashSet<int>
				(cliLicProd.ClientesLicenciasProductosModulos.Select(l => l.ProductosModuloId));

			IList<ProductosModulo> productosModulos = _ProductoService.FindBy(x => x.Id == cliLicProd.ProductoId).FirstOrDefault().ProductosModulos.ToList();

			foreach (var modulo in productosModulos)
			{
				if (selectedModulosHS.Contains(modulo.Id.ToString()))
				{
					if (!prodMod.Contains(modulo.Id))
					{
						_ClientesLicenciasProductosModuloService.Create(new ClientesLicenciasProductosModulo(cliLicProd.Id, modulo.Id));
					}
				}
				else
				{

					if (prodMod.Contains(modulo.Id))
					{
						ClientesLicenciasProductosModulo cliLicProdMod = _ClientesLicenciasProductosModuloService
							.FindBy(x => x.ClientesLicenciasProductoId == cliLicProd.Id && x.ProductosModuloId == modulo.Id)
							.FirstOrDefault();

						_ClientesLicenciasProductosModuloService.Delete(cliLicProdMod);
					}
				}
			}

			_ClientesLicenciasProductoService.Update(cliLicProd);

		}

		private void LlenarProductosModulos(IList<ProductosModulo> pMod, int clientesLicenciaId, int productoId)
		{
			var allProductosModulos = pMod;
			ClientesLicencia cliLic = _ClientesLicenciaService.GetById(clientesLicenciaId);
			ClientesLicenciasProducto cliLicProd = cliLic.ClientesLicenciasProductos.FirstOrDefault(x => x.ProductoId == productoId);

			var prodModAsignados = new HashSet<int>(cliLicProd.ClientesLicenciasProductosModulos.Select(p => p.ProductosModuloId));
			var viewModel = new List<ModulosAsignados>();
			foreach (var mod in allProductosModulos)
			{
				viewModel.Add(new ModulosAsignados
				{
					ProductoModuloID = mod.Id,
					Descripcion = mod.Descripcion,
					Asignado = prodModAsignados.Contains(mod.Id)
				});
			}

			ViewBag.ProdModAsignados = viewModel;

		}

		private void UpdateLicenciasProductos(string[] selectedProductos, ClientesLicencia licenciaToUpdate)
		{

			var selectedProductosHS = new HashSet<string>(selectedProductos);
			var licenciasProductos = new HashSet<int>
				(licenciaToUpdate.ClientesLicenciasProductos.Select(l => l.ProductoId));
			foreach (var prod in _ProductoService.GetAll().ToList())
			{
				if (selectedProductosHS.Contains(prod.Id.ToString()))
				{
					if (!licenciasProductos.Contains(prod.Id))
					{
						var cliLicProd = new ClientesLicenciasProducto();
						cliLicProd.ProductoId = prod.Id;
						cliLicProd.ClientesLicenciaId = licenciaToUpdate.Id;
						_ClientesLicenciasProductoService.Create(cliLicProd);
						ICollection<ProductosModulo> productosModulos =
							_ProductoService
							.GetById(prod.Id)
							.ProductosModulos;

						foreach (ProductosModulo mod in productosModulos)
						{
							_ClientesLicenciasProductosModuloService.Create(new ClientesLicenciasProductosModulo(cliLicProd.Id, mod.Id));
						}
					}
				}
				else
				{
					if (licenciasProductos.Contains(prod.Id))
					{
						// --> Borro historiales, despues modulos y dps el producto.
						var cliLicProd = _ClientesLicenciasProductoService.FindBy(x => x.ProductoId == prod.Id && x.ClientesLicenciaId == licenciaToUpdate.Id).FirstOrDefault();

						for (int i = cliLicProd.ClientesLicenciasProductosModulos.Count - 1; i >= 0; i--)
						{
							var mod = cliLicProd.ClientesLicenciasProductosModulos.ElementAt(i);
							if (mod.Historial.Count > 0)
							{
								for (int j = mod.Historial.Count - 1; i >= 0; i--)
								{
									_HistorialService.Delete(mod.Historial[j]);
								}
							}

							_ClientesLicenciasProductosModuloService.Delete(mod);
						}

						_ClientesLicenciasProductoService.Delete(cliLicProd);
					}
				}
			}
		}

		private void LlenarProductosAsignados(ClientesLicencia licencia)
		{
			ICollection<Producto> allProductos = _ProductoService.GetAll().ToList();
			var licenciasProductos = new HashSet<int>(licencia.ClientesLicenciasProductos.Select(p => p.ProductoId));
			var viewModel = new List<ProductosAsignados>();
			foreach (var producto in allProductos)
			{
				viewModel.Add(new ProductosAsignados
				{
					ProductoID = producto.Id,
					Descripcion = producto.Descripcion,
					Asignado = licenciasProductos.Contains(producto.Id)
				});
			}

			ViewBag.Productos = viewModel;
		}

		private ICollection<Licencia> getLicenciasDisponibles(int idLic = 0)
		{
			ICollection<Licencia> lstLicEnUso = _ClientesLicenciaService.GetAll().Select(x => x.Licencia).ToList();
			ICollection<Licencia> lstLicTotales = _LicenciaService.GetAll().ToList();
			ICollection<Licencia> lstDisponibles = new List<Licencia>();

			lstDisponibles = lstLicTotales.Except(lstLicEnUso).ToList();

			if (idLic != 0)
			{
				Licencia lic = _LicenciaService.FindBy(x => x.Id == idLic).FirstOrDefault();
				lstDisponibles.Add(lic);
			}

			return lstDisponibles;
		}

		#endregion
	}
}