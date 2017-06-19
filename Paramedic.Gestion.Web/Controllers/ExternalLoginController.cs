using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;
using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Paramedic.Gestion.Web.Controllers
{
	public class ExternalLoginController : Controller
	{
		#region Properties

		IClientesLicenciaService _ClientesLicenciaService;
		IUserProfileService _UserProfileService;
		IClientesUsuarioService _ClientesUsuarioService;

		#endregion

		#region Constructors

		public ExternalLoginController(IClientesLicenciaService ClientesLicenciaService, IUserProfileService UserProfileService, IClientesUsuarioService ClientesUsuarioService)
		{
			_ClientesLicenciaService = ClientesLicenciaService;
			_UserProfileService = UserProfileService;
			_ClientesUsuarioService = ClientesUsuarioService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string user = null, string pass = null, string llave = null, int shex_id = 0)
		{
			if (ModelState.IsValid && WebSecurity.Login(user, pass, true))
			{
				ClientesUsuario clientesUsuario = getClientesUsuario(llave, user);

				if (clientesUsuario != null)
				{
					if (clientesUsuario.ShamanExpressId.Equals(null))
					{
						clientesUsuario.ShamanExpressId = shex_id;
						_ClientesUsuarioService.Update(clientesUsuario);
					}
				}

				return RedirectToAction("Index", "Home");
			}
			else
			{
				return RedirectToAction("Login", "Account");
			}
		}

		public int IsInGestion(string user = null, string pass = null, string llave = null)
		{
			ClientesUsuario clientesUsuario = getClientesUsuario(llave, user);

			if (clientesUsuario == null)
			{
				return 0;
			}

			int ret = WebSecurity.Login(user, pass, true) ? 1 : 0;

			return ret;
		}

		#endregion

		#region Private Methods

		private ClientesUsuario getClientesUsuario(string llave = null, string user = null)
		{
			int cli_id = _ClientesLicenciaService
				.FindBy(p => p.Licencia.Serial == llave)
				.Select(p => p.Cliente.Id)
				.FirstOrDefault();

			int usr_id = _UserProfileService
							.FindBy(p => p.UserName == user)
							.Select(p => p.Id)
							.FirstOrDefault();


			ClientesUsuario clientesUsuario = _ClientesUsuarioService
									.FindBy(p => p.ClienteId == cli_id)
									.Where(p => p.Usuario.UserName == user)
									.FirstOrDefault();

			return clientesUsuario;
		}

		#endregion
	}
}
