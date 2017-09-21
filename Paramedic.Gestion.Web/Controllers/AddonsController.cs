using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Web.ViewModels;
using WebMatrix.WebData;
using System;
using SocialMedia.Services;
using System.Configuration;

namespace Paramedic.Gestion.Web.Controllers
{
	public class AddonsController : Controller
	{
		#region Properties

		INoticiaService _NoticiaService;
		IProductosModuloService _ModuloService;
		IClientesLicenciaService _ClientesLicenciaService;
		IClientesLicenciasProductosModulosHistorialService _HistorialService;
		IClientesLicenciasProductosModuloService _ClientesLicenciasProductosModuloService;
		IProductosModulosIntentoService _ProductosModulosIntentoService;
		IClientesLicenciasProductoService _ClientesLicenciasProductoService;

		#endregion

		#region Constructors

		public AddonsController(
			INoticiaService Service,
			IProductosModuloService ModuloService,
			IClientesLicenciaService ClientesLicenciaService,
			IClientesLicenciasProductosModulosHistorialService HistorialService,
			IClientesLicenciasProductosModuloService ClientesLicenciasProductosModuloService,
			IProductosModulosIntentoService ProductosModulosIntentoService,
			IClientesLicenciasProductoService ClientesLicenciasProductoService
			)
		{
			_NoticiaService = Service;
			_ModuloService = ModuloService;
			_ClientesLicenciaService = ClientesLicenciaService;
			_HistorialService = HistorialService;
			_ClientesLicenciasProductosModuloService = ClientesLicenciasProductosModuloService;
			_ProductosModulosIntentoService = ProductosModulosIntentoService;
			_ClientesLicenciasProductoService = ClientesLicenciasProductoService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string license)
		{
			IEnumerable<ProductosModulo> modulos = _ClientesLicenciaService.GetProductosModulosForAddon(license);
			IEnumerable<Noticia> noticias = _NoticiaService.GetNoticiasNoVencidas();
			AddonsViewModel vm = new AddonsViewModel(noticias, modulos, license);

			return View("Index", "_AddonsTemplate", vm);
		}

		public ActionResult ConfirmAddonTrial(string license, int prodModId)
		{
			try
			{
				ProductosModulosIntento intento = _ProductosModulosIntentoService.GetIntentosByProductoModuloId(prodModId).FirstOrDefault();

				if (intento == null)
				{
					return View("NoIntentoConfigured", "_AddonsTemplate");
				}

				ClientesLicencia clientesLicencia = _ClientesLicenciaService
					.FindBy(x => x.Licencia.Serial == license)
					.FirstOrDefault();

				ClientesLicenciasProducto cliLicProd = _ClientesLicenciasProductoService
					.FindBy(x => x.ClientesLicenciaId == clientesLicencia.Id && x.ProductoId == intento.ProductosModulo.ProductoId)
					.FirstOrDefault();

				AddonHistorialViewModel vm = new AddonHistorialViewModel(intento, cliLicProd, prodModId);

				return View("ConfirmAddonTrial", "_AddonsTemplate", vm);

			}

			catch
			{
				return HttpNotFound();
			}
		}

		[HttpPost]
		public ActionResult ConfirmAddonTrial(AddonHistorialViewModel vm)
		{
			try
			{
				ProductosModulosIntento intento = _ProductosModulosIntentoService.GetById(vm.ProductosModulosIntentoId);
				ClientesLicenciasProductosModulo pm = new ClientesLicenciasProductosModulo(vm.ClientesLicenciasProductoId, vm.ProductosModuloId);
				_ClientesLicenciasProductosModuloService.Create(pm);

				ClientesLicenciasProducto prod = _ClientesLicenciasProductoService.GetById(vm.ClientesLicenciasProductoId);
				ClientesLicenciasProductosModulosHistorial historial = new ClientesLicenciasProductosModulosHistorial(pm, intento);
				_HistorialService.Create(historial);
				string to = ConfigurationManager.AppSettings["importantStuffMail"].ToString();
				string from = ConfigurationManager.AppSettings["administratorMail"].ToString();
				string body = string.Format("Se generó un nuevo trial para la licencia {0} del cliente {1}. El módulo seleccionado fue: {2} del producto {3}",
					prod.ClientesLicencia.Licencia.Serial,
					prod.ClientesLicencia.Cliente.RazonSocial,
					vm.ModuloDescripcion,
					prod.Producto.Descripcion);
				Message message = new EmailMessage(body, from, to, "Nuevo trial");
				MailService.Instance.Send(message);
				return View("SuccessAddonTrial", "_AddonsTemplate", vm);
			}

			catch (Exception ex)
			{
				return HttpNotFound();
			}
		}

		#endregion
	}
}