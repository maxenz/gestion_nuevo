using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Web.ViewModels;
using Paramedic.Gestion.Model.Helpers;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class VencimientosController : Controller
	{
		#region Properties

		IClientesLicenciaService _ClientesLicenciaService;

		private int controllersPageSize = 6;

		#endregion

		#region Constructors

		public VencimientosController(IClientesLicenciaService ClientesLicenciaService)
		{
			_ClientesLicenciaService = ClientesLicenciaService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null)
		{
			DateTime dtFechaDesde = Convert.ToDateTime(fechaDesde);
			DateTime dtFechaHasta = Convert.ToDateTime(fechaHasta);
			ViewBag.dftDesde = DateTime.Now.AddDays(-30).ToShortDateString();
			ViewBag.dftHasta = DateTime.Now.ToShortDateString();

			VencimientosQueryControllerParametersDTO queryParameters = new VencimientosQueryControllerParametersDTO(searchName, controllersPageSize, page, dtFechaDesde, dtFechaHasta);

			IEnumerable<ClientesLicencia> cliLics = _ClientesLicenciaService.GetLicencias(queryParameters);
			int count = _ClientesLicenciaService.GetCount(_ClientesLicenciaService.GetPredicateByConditions(queryParameters));

			List<Vencimiento> vencimientos = new List<Vencimiento>();
			foreach(ClientesLicencia cl in cliLics)
			{
				if (cl.FechaDeVencimiento != SqlSmallDateTime.MinValue)
				{
					Vencimiento v = new Vencimiento(cl.FechaDeVencimiento, cl.Licencia.Serial, cl.Cliente.RazonSocial, VencimientoType.Licencia);
					vencimientos.Add(v);
				}

				if (cl.FechaVencimientoSoporte != SqlSmallDateTime.MinValue)
				{
					Vencimiento v = new Vencimiento(cl.FechaVencimientoSoporte, cl.Licencia.Serial, cl.Cliente.RazonSocial, VencimientoType.Soporte);
					vencimientos.Add(v);
				}
			}

			var resultAsPagedList = new StaticPagedList<Vencimiento>(vencimientos, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Vencimientos", resultAsPagedList);
			}

			return View(resultAsPagedList);
		}

		#endregion
	}
}
