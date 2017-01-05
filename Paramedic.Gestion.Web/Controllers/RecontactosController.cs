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

            RecontactosControllerParametersDTO queryParameters = new RecontactosControllerParametersDTO(searchName, controllersPageSize, page, fechaDesde, fechaHasta, (GestionType)selTipoGestion);

            IEnumerable<ClientesGestion> gestiones = _ClientesGestionService.GetRecontactosByPage(queryParameters);
            int count = _ClientesGestionService.GetCount(_ClientesGestionService.GetPredicateByConditions(queryParameters));
            var resultAsPagedList = new StaticPagedList<ClientesGestion>(gestiones, page, controllersPageSize, count);

            ViewBag.dftDesde = DateTime.Now.ToShortDateString();
            ViewBag.dftHasta = DateTime.Now.AddDays(30).ToShortDateString();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Recontactos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        #endregion

    }
}
