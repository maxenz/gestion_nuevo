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

namespace Gestion.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private GestionDb db = new GestionDb();

        [ChildActionOnly]
        public ActionResult Recontacto()
        {
            var margenMayor = DateTime.Now.AddDays(7);
            var hoy = DateTime.Now;
            var recontactos = db.ClientesGestiones.Where(a => a.FechaRecontacto >= hoy && a.FechaRecontacto <= margenMayor).ToList();
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
