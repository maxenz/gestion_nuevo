using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class RecontactosController : Controller
    {
        #region Properties

        IClienteService _ClienteService;
        IClientesGestionService _ClientesGestionService;

        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public RecontactosController(IClienteService ClienteService, IClientesGestionService ClientesGestionService)
        {
            _ClienteService = ClienteService;
            _ClientesGestionService = ClientesGestionService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null, int selTipoGestion = 3)
        {

            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddDays(30);
            ViewBag.dftDesde = from.ToShortDateString();
            ViewBag.dftHasta = to.ToShortDateString();

            RecontactosControllerParametersDTO queryParameters = new RecontactosControllerParametersDTO(searchName, controllersPageSize, page, fechaDesde, fechaHasta, (GestionType)selTipoGestion);

            IEnumerable<ClientesGestion> gestiones = _ClientesGestionService.GetRecontactosByPage(queryParameters);
            int count = _ClientesGestionService.GetCount(_ClientesGestionService.GetPredicateByConditions(queryParameters));
            var resultAsPagedList = new StaticPagedList<ClientesGestion>(gestiones, page, controllersPageSize, count);

            //if (!String.IsNullOrEmpty(fechaDesde))
            //{
            //    fechaDesde = fechaDesde + " 00:00";
            //    fechaHasta = fechaHasta + " 23:59";

            //    DateTime dtDesde = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //    DateTime dtHasta = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            //    qRecontactos = qRecontactos.Where(a => a.Fecha >= dtDesde && a.Fecha <= dtHasta).ToList();
            //}
            //else
            //{
            //    DateTime dtLocal = hoy.AddDays(-1);
            //    qRecontactos = qRecontactos.Where(a => a.Fecha >= dtLocal && a.Fecha <= margenMayor).ToList();
            //}

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Recontactos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        #endregion

    }
}
