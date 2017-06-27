using Gestion.Models;
using Gestion.ViewModels;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model.Enums;

namespace Gestion.Controllers
{
	public class LogsRegistrosSistemaController : Controller
	{
		#region Properties

		ILogsRegistrosSistemaService _LogService;
		IVideoService _VideoService;
		IUserProfileService _UserProfileService;
		private int controllersPageSize = 6;

		#endregion

		#region Constructors

		public LogsRegistrosSistemaController(
			ILogsRegistrosSistemaService Service,
			IVideoService VideoService,
			IUserProfileService UserProfileService)
		{
			_LogService = Service;
			_VideoService = VideoService;
			_UserProfileService = UserProfileService;
		}

		#endregion

		private GestionDb db = new GestionDb();

		[Authorize(Roles = "Administrador")]
		public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null)
		{

			DateTime dtFrom = DateTime.Now.Date.AddDays(-3);
			DateTime dtTo = DateTime.Now.Date.AddDays(1);

			if (!string.IsNullOrEmpty(fechaDesde) && !string.IsNullOrEmpty(fechaHasta))
			{
				dtFrom = Convert.ToDateTime(fechaDesde).Date;
				dtTo = Convert.ToDateTime(fechaHasta).AddDays(1).Date;
			}

			var predicate = PredicateBuilder.New<LogRegistroSistema>();
			predicate = predicate.And(x => x.CreatedDate >= dtFrom && x.CreatedDate < dtTo);
			if (!string.IsNullOrEmpty(searchName))
			{
				predicate = predicate.And(x => x.DescripcionAccion.ToUpper().Contains(searchName.ToUpper()));
			}

			IEnumerable<LogRegistroSistema> logs = _LogService.FindByPage(predicate, "CreatedDate DESC", controllersPageSize, page);
			int count = _LogService.FindBy(predicate).Count();
			var resultAsPagedList = new StaticPagedList<LogRegistroSistema>(logs, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Logs", resultAsPagedList);
			}

			return View(resultAsPagedList);

		}

		[Authorize(Roles = "Cliente")]
		[HttpPost]
		public int SetVideoLog(int idVideo)
		{
			try
			{
				Video video = _VideoService.GetById(idVideo);
				UserProfile profile = _UserProfileService.FindBy(x => x.UserName == User.Identity.Name).FirstOrDefault();
				if (profile != null)
				{
					string logDescription = string.Format("Vista de video id {0}: {1}", video.Id, video.Descripcion);
					LogRegistroSistema log = new LogRegistroSistema(logDescription, profile.Id);
					_LogService.Create(log);
					return 1;
				}

				return 0;

			}
			catch (Exception ex)
			{
				LoggingService.Instance.Write(LoggingTypes.Error, string.Format("Error al generar log para la vista del video {0}. Error: {1} ", idVideo, ex.Message));
				return 0;
			}

		}

	}
}
