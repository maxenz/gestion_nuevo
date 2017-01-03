using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using Gestion.ViewModels;
using System.Net;
using Paramedic.Gestion.Web.wsCuentaCorriente;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Service;

namespace Gestion.Controllers
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

        [ChildActionOnly]
        public ActionResult Recontacto()
        {
            DateTime from = DateTime.Now;
            DateTime to = DateTime.Now.AddDays(7);

            IEnumerable<ClientesGestion> recontactos = _ClientesGestionService.GetRecontactos(from, to);
            return PartialView("~/Views/Shared/_NotificacionRecontacto.cshtml",recontactos);
        }

        public ActionResult Index()
        {         
            if (User.IsInRole("Administrador")) {
                return RedirectToAction("Index","Recontactos");
            }
            return View();
        }

    }
}
