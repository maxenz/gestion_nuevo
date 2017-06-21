using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Web.ViewModels;
using WebMatrix.WebData;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class AddonsController : Controller
	{
		#region Properties

		INoticiaService _NoticiaService;
		IProductosModuloService _ModuloService;
		IClientesLicenciaService _ClientesLicenciaService;
		IClientesLicenciasProductosModulosHistorialService _HistorialService;

		#endregion

		#region Constructors

		public AddonsController(
			INoticiaService Service,
			IProductosModuloService ModuloService,
			IClientesLicenciaService ClientesLicenciaService,
			IClientesLicenciasProductosModulosHistorialService HistorialService
			)
		{
			_NoticiaService = Service;
			_ModuloService = ModuloService;
			_ClientesLicenciaService = ClientesLicenciaService;
			_HistorialService = HistorialService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string searchName = null, int page = 1)
		{
			IEnumerable<Noticia> noticias = _NoticiaService.GetNoticiasNoVencidas().Take(4);
			IEnumerable<ProductosModulo> modulos = _ModuloService.GetAll().Where(x => x.DescripcionAddon != null);
			AddonsViewModel vm = new AddonsViewModel(noticias, modulos);
			return View(vm);
		}

		public ActionResult ConfirmAddonTrial(string user, string pass, string license, int prodModId)
		{
			try
			{
				if (WebSecurity.Login(user, pass, true))
				{
					ClientesLicenciasProductosModulosHistorial historial = _ClientesLicenciaService.GetAddonHistorial(license, prodModId);
					if (historial != null)
					{
						return View("ConfirmAddonTrial", historial);
					}
				}

				return HttpNotFound();
			}

			catch
			{
				return HttpNotFound();
			}
		}

		[HttpPost]
		public ActionResult ConfirmAddonTrial(ClientesLicenciasProductosModulosHistorial historial)
		{
			_HistorialService.Create(historial);
			return View("SuccessAddonTrial");
		}

		#endregion
	}
}