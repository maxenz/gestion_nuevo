using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class ClientesHistorialesController : Controller
	{
		#region Properties

		IClientesLicenciasProductosModulosHistorialService _HistorialService;
		IClienteService _ClienteService;
		IClientesLicenciasProductosModuloService _ClientesLicenciasProductosModuloService;

		#endregion

		#region Constructors

		public ClientesHistorialesController(
			IClientesLicenciasProductosModulosHistorialService HistorialService,
			IClienteService ClienteService,
			IClientesLicenciasProductosModuloService ClientesLicenciasProductosModuloService)
		{
			_HistorialService = HistorialService;
			_ClienteService = ClienteService;
			_ClientesLicenciasProductosModuloService = ClientesLicenciasProductosModuloService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(int clienteId)
		{
			IEnumerable<ClientesLicenciasProductosModulosHistorial> historial =
				_HistorialService
				.FindBy(x => x.ClientesLicenciasProductosModulo.ClientesLicenciasProducto.ClientesLicencia.ClienteId == clienteId)
				.OrderBy(x => x.FechaVencimiento).ToList();

			if (historial == null)
			{
				return HttpNotFound();
			}

			ViewBag.ClienteId = clienteId;

			return PartialView("_ClientesHistoriales", historial);
		}

		[AllowAnonymous]
		public JsonResult GetHistoriales()
		{
			return Json(_HistorialService.GetAll(), JsonRequestBehavior.AllowGet);
		}

		[AllowAnonymous]
		[HttpPost]
		public JsonResult CancelProductoModulo(int histId)
		{
			try
			{
				ClientesLicenciasProductosModulosHistorial historial = _HistorialService.GetById(histId);
				ClientesLicenciasProductosModulo pm = historial.ClientesLicenciasProductosModulo;
				_HistorialService.Delete(historial);
				_ClientesLicenciasProductosModuloService.Delete(pm);
				return Json(null);
			}
			catch (Exception ex)
			{
				return null;
			}

		}

		#endregion
	}
}