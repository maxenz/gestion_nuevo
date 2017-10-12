using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        #region Properties

        IClientesGestionService _ClientesGestionService;

        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public HomeController(IClientesGestionService ClientesGestionService)
        {
            _ClientesGestionService = ClientesGestionService;            
        }

        #endregion

        #region Public Methods

        [ChildActionOnly]
        public ActionResult Recontacto()
        {
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddDays(7);

            IEnumerable<ClientesGestion> recontactos = _ClientesGestionService.GetRecontactos(from, to);
            return PartialView("~/Views/Shared/_NotificacionRecontacto.cshtml", recontactos);
        }

		public ActionResult Index()
        {
            if (User.IsInRole("Administrador"))
            {
                return RedirectToAction("Index", "Recontactos");
            }
            return View();
        }

        #endregion

    }
}
