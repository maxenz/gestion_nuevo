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

		#endregion

		#region Constructors

		public AddonsController(
			INoticiaService Service,
			IProductosModuloService ModuloService,
			IClientesLicenciaService ClientesLicenciaService,
			IClientesLicenciasProductosModulosHistorialService HistorialService,
			IClientesLicenciasProductosModuloService ClientesLicenciasProductosModuloService
			)
		{
			_NoticiaService = Service;
			_ModuloService = ModuloService;
			_ClientesLicenciaService = ClientesLicenciaService;
			_HistorialService = HistorialService;
			_ClientesLicenciasProductosModuloService = ClientesLicenciasProductosModuloService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string license)
		{
			IEnumerable<ProductosModulo> modulos =
				_ClientesLicenciaService
				.GetProductosModulosForAddon(license)
				.Select(x => x.ProductosModulo);

			IEnumerable<Noticia> noticias = _NoticiaService.GetNoticiasNoVencidas().Take(4);
			AddonsViewModel vm = new AddonsViewModel(noticias, modulos, license);
			return View("Index", "_AddonsTemplate", vm);
		}

		public ActionResult ConfirmAddonTrial(string license, int prodModId)
		{
			ViewBag.License = license;
			try
			{
				ClientesLicenciasProductosModulosHistorial historial = _ClientesLicenciaService.GetAddonHistorial(license, prodModId);
				if (historial != null)
				{
					return View("ConfirmAddonTrial", "_AddonsTemplate", historial);
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
			try
			{
				_HistorialService.Create(historial);
				ClientesLicenciasProductosModulo p = _ClientesLicenciasProductosModuloService.GetById(historial.ClientesLicenciasProductosModuloId);
				historial.ClientesLicenciasProductosModulo = p;
				string to = ConfigurationManager.AppSettings["importantStuffMail"].ToString();
				string from = ConfigurationManager.AppSettings["administratorMail"].ToString();
				string body = string.Format("Se generó un nuevo trial para la licencia {0} del cliente {1}. El módulo seleccionado fue: {2} del producto {3}",
					p.ClientesLicenciasProducto.ClientesLicencia.Licencia.Serial,
					p.ClientesLicenciasProducto.ClientesLicencia.Cliente.RazonSocial,
					p.ProductosModulo.Descripcion,
					p.ProductosModulo.Producto.Descripcion);
				Message message = new EmailMessage(body, from, to, "Nuevo trial");
				MailService.Instance.Send(message);
				return View("SuccessAddonTrial","_AddonsTemplate", historial);
			}

			catch (Exception ex)
			{
				return HttpNotFound();
			}
		}

		#endregion
	}
}