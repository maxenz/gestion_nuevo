using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using System;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador, Colaborador, ColaboradorCliente")]
	public class MonitorTareasController : Controller
	{

		#region Properties

		IProyectoService _ProyectoService;
		ITareaService _TareaService;
		ITareasGestionService _TareasGestionService;
		IClienteService _ClientesService;
		IUserProfileService _UserProfileService;
		private int controllersPageSize = 6;

		#endregion

		#region Constructors

		public MonitorTareasController(
			IProyectoService ProyectoService,
			ITareasGestionService TareasGestionService,
			IClienteService ClienteService,
			IUserProfileService UserProfileService,
			ITareaService TareaService)
		{
			_ProyectoService = ProyectoService;
			_TareasGestionService = TareasGestionService;
			_ClientesService = ClienteService;
			_UserProfileService = UserProfileService;
			_TareaService = TareaService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string dateFrom = null, string dateTo = null, string searchName = null, int page = 1)
		{

			DateTime dtFrom;
			DateTime dtTo;
			if (string.IsNullOrEmpty(dateFrom) || string.IsNullOrEmpty(dateTo))
			{
				dtFrom = DateTime.Now.AddDays(-30);
				dtTo = DateTime.Now;
			}
			else
			{
				dtFrom = Convert.ToDateTime(dateFrom);
				dtTo = Convert.ToDateTime(dateTo);
			}

			ViewBag.dateFrom = dtFrom.ToShortDateString();
			ViewBag.dateTo = dtTo.ToShortDateString();

			DateQueryControllerParametersDTO queryParameters = new DateQueryControllerParametersDTO(searchName, controllersPageSize, page, dtFrom, dtTo);

			if (!IsCurrentUserAdmin())
			{
				ViewBag.IsCurrentUserAdmin = false;
				var user = _UserProfileService.FindBy(x => x.UserName == User.Identity.Name).FirstOrDefault();
				queryParameters.UserId = user.Id;
			}
			else
			{
				queryParameters.IsCurrentUserAdmin = true;
				ViewBag.IsCurrentUserAdmin = true;
			}

			IEnumerable<TareasGestion> tareasGestion = _TareasGestionService.GetTareasGestion(queryParameters);
			int count = _TareasGestionService.GetCount(_TareasGestionService.getPredicateByConditions(queryParameters));

			var resultAsPagedList = new StaticPagedList<TareasGestion>(tareasGestion, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_TareasGestiones", resultAsPagedList);
			}

			return View(resultAsPagedList);
		}

		public ActionResult Create()
		{
			generateDropdowns();
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create(TareasGestion tareasGestion)
		{
			if (!IsCurrentUserAdmin())
			{
				var user = _UserProfileService.FindBy(x => x.UserName == User.Identity.Name).FirstOrDefault();
				tareasGestion.UsuarioId = user.Id;
			}
			if (ModelState.IsValid)
			{
				_TareasGestionService.Create(tareasGestion);
				return RedirectToAction("Index");
			}

			generateDropdowns();
			return View(tareasGestion);
		}

		public ActionResult Delete(int id = 0)
		{
			TareasGestion tareasGestion = _TareasGestionService.GetById(id);
			if (tareasGestion == null)
			{
				return HttpNotFound();
			}
			return View(tareasGestion);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult DeleteConfirmed(int id)
		{
			TareasGestion tareasGestion = _TareasGestionService.GetById(id);
			_TareasGestionService.Delete(tareasGestion);
			return RedirectToAction("Index");
		}

		#endregion

		#region Private Methods

		private void generateDropdowns()
		{
			ViewBag.ProyectosList = new SelectList(_ProyectoService.GetAll().OrderBy(x => x.Descripcion), "Id", "Descripcion", "ProyectoId");
			ViewBag.ClientesList = new SelectList(_ClientesService.GetAll().OrderBy(x => x.RazonSocial), "Id", "RazonSocial", "ClienteId");
			ViewBag.UsuariosList = new SelectList(_UserProfileService.GetAll().OrderBy(x => x.UserName), "Id", "UserName", "UsuarioId");
			ViewBag.TareasList = new SelectList(new List<Tarea>(), "Id", "Descripcion", "TareaId");
			ViewBag.IsCurrentUserAdmin = IsCurrentUserAdmin();
		}

		private bool IsCurrentUserAdmin()
		{
			return User.IsInRole("Administrador");
		}

		#endregion

	}
}