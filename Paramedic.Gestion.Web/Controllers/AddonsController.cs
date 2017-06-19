using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using LinqKit;
using System.Collections;
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

		#endregion

		#region Constructors

		public AddonsController(
			INoticiaService Service,
			IProductosModuloService ModuloService,
			IClientesLicenciaService ClientesLicenciaService
			)
		{
			_NoticiaService = Service;
			_ModuloService = ModuloService;
			_ClientesLicenciaService = ClientesLicenciaService;
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
					ProductosModulosIntento intento = _ClientesLicenciaService.GetAddonIntento(license, prodModId);
					if (intento != null)
					{
						return View("TrialAddonConfirmation", intento);
					}
				}

				return HttpNotFound();
			}

			catch
			{
				return HttpNotFound();
			}
		}

		#endregion


	}
}