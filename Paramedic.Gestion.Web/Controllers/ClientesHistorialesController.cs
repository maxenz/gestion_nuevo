using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class ClientesHistorialesController : Controller
	{
		#region Properties

		IClientesLicenciasProductosModulosHistorialService _HistorialService;
		IClienteService _ClienteService;

		#endregion

		#region Constructors

		public ClientesHistorialesController(
			IClientesLicenciasProductosModulosHistorialService HistorialService,
			IClienteService ClienteService)
		{
			_HistorialService = HistorialService;
			_ClienteService = ClienteService;
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

		//public ActionResult Create(int clienteId)
		//{
		//	ViewBag.ClienteId = clienteId;
		//	setGeneralViewData(null);
		//	return View();
		//}

		//[HttpPost]
		//public ActionResult Create(ClientesTerminal clientesterminal)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_ClientesTerminalService.Create(clientesterminal);
		//		return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteId });
		//	}

		//	setGeneralViewData(clientesterminal);
		//	return View(clientesterminal);
		//}

		//public ActionResult Edit(int id = 0)
		//{
		//	ClientesTerminal clientesterminal = _ClientesTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
		//	if (clientesterminal == null)
		//	{
		//		return HttpNotFound();
		//	}

		//	ViewBag.ClienteId = clientesterminal.ClienteId;
		//	setGeneralViewData(clientesterminal);

		//	return View(clientesterminal);
		//}

		//[HttpPost]
		//public ActionResult Edit(ClientesTerminal clientesterminal)
		//{
		//	if (ModelState.IsValid)
		//	{
		//		_ClientesTerminalService.Update(clientesterminal);
		//		return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteId });
		//	}

		//	setGeneralViewData(clientesterminal);
		//	return View(clientesterminal);
		//}

		//[HttpPost, ActionName("Delete")]
		//public ActionResult DeleteConfirmed(int id)
		//{
		//	ClientesTerminal clientesterminal = _ClientesTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
		//	_ClientesTerminalService.Delete(clientesterminal);
		//	return RedirectToAction("Index", routeValues: new { ClienteID = clientesterminal.ClienteId });
		//}

		#endregion
	}
}