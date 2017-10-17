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
			DateTime dtFechaDesde;
			DateTime dtFechaHasta;
			if (fechaDesde != null)
			{
				dtFechaDesde = Convert.ToDateTime(fechaDesde);
				dtFechaHasta = Convert.ToDateTime(fechaHasta);
			} else
			{
				dtFechaDesde = DateTime.Now.AddDays(-20);
				dtFechaHasta = DateTime.Now;
			}

			ViewBag.dftDesde = dtFechaDesde.ToShortDateString();
			ViewBag.dftHasta = dtFechaHasta.ToShortDateString();

			VencimientosQueryControllerParametersDTO queryParameters = new VencimientosQueryControllerParametersDTO(searchName, controllersPageSize, page, dtFechaDesde, dtFechaHasta);

			IEnumerable<ClientesLicencia> cliLics = _ClientesLicenciaService.GetLicencias(queryParameters);
			int count = _ClientesLicenciaService.GetCount(_ClientesLicenciaService.GetPredicateByConditions(queryParameters));

			List<VencimientoViewModel> vencimientos = new List<VencimientoViewModel>();
			foreach(ClientesLicencia cl in cliLics)
			{
				if (cl.FechaDeVencimiento > dtFechaDesde && cl.FechaDeVencimiento < dtFechaHasta)
				{
					VencimientoViewModel v = new VencimientoViewModel(cl.FechaDeVencimiento, cl.Licencia.Serial, cl.Cliente.RazonSocial, VencimientoType.Licencia);
					vencimientos.Add(v);
				}

				if (cl.FechaVencimientoSoporte > dtFechaDesde && cl.FechaVencimientoSoporte < dtFechaHasta)
				{
					VencimientoViewModel v = new VencimientoViewModel(cl.FechaVencimientoSoporte, cl.Licencia.Serial, cl.Cliente.RazonSocial, VencimientoType.Soporte);
					vencimientos.Add(v);
				}
			}

			var resultAsPagedList = new StaticPagedList<VencimientoViewModel>(vencimientos, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Vencimientos", resultAsPagedList);
			}

			return View(resultAsPagedList);
		}

		#endregion
	}
}
