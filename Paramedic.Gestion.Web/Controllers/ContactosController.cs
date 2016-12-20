using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using PagedList;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ContactosController : Controller
    {
        private GestionDb db = new GestionDb();

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var qCliCont = from c in db.ClientesContactos select c;

            if (!String.IsNullOrEmpty(searchName))
            {

                qCliCont = qCliCont
                            .Where(p => p.Cliente.RazonSocial.ToUpper().Contains(searchName.ToUpper()) ||
                                    p.Nombre.ToUpper().Contains(searchName.ToUpper()) ||
                                    p.Email.ToUpper().Contains(searchName.ToUpper()) ||
                                    p.Telefono.ToUpper().Contains(searchName.ToUpper()));
            }

            qCliCont = qCliCont.OrderBy(p => p.Cliente.RazonSocial);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Contactos", qCliCont.ToPagedList(page, 6));
            }

            return View(qCliCont.ToPagedList(page, 6));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}