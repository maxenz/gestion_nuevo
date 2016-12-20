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
    public class ClientesContactosController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ClientesContactos/

        public ActionResult Index(int ClienteID, String searchName = null, int page = 1)
        {

            var qContactos = from c in db.ClientesContactos where c.ClienteID == ClienteID select c;

            if (qContactos == null)
            {
                return HttpNotFound();
            }

            qContactos = qContactos.OrderBy(p => p.Nombre);


            return PartialView("_ClientesContactos", qContactos.ToList());
  
        }

        //
        // GET: /ClientesContactos/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientesContacto clientescontacto = db.ClientesContactos.Find(id);
            if (clientescontacto == null)
            {
                return HttpNotFound();
            }
            return View(clientescontacto);
        }

        //
        // GET: /ClientesContactos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ClientesContactos/Create

        [HttpPost]
        public ActionResult Create(ClientesContacto clientescontacto)
        {
            if ((ModelState.IsValid) & validateContacto(clientescontacto))
            {
                db.ClientesContactos.Add(clientescontacto);
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientescontacto.ClienteID });
            }

            return View(clientescontacto);
        }

        private bool validateContacto(ClientesContacto clientescontacto) {

            if (!(clientescontacto.Email == null) | !(clientescontacto.Otros == null) | !(clientescontacto.Telefono == null)) 
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        //
        // GET: /ClientesContactos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ClientesContacto clientescontacto = db.ClientesContactos.Find(id);
            if (clientescontacto == null)
            {
                return HttpNotFound();
            }

            return View(clientescontacto);
        }

        //
        // POST: /ClientesContactos/Edit/5

        [HttpPost]
        public ActionResult Edit(ClientesContacto clientescontacto)
        {
            if (ModelState.IsValid & validateContacto(clientescontacto))
            {
                db.Entry(clientescontacto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientescontacto.ClienteID });
            }
            return View(clientescontacto);
        }

        //
        // GET: /ClientesContactos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientesContacto clientescontacto = db.ClientesContactos.Find(id);
            if (clientescontacto == null)
            {
                return HttpNotFound();
            }
            return View(clientescontacto);
        }

        //
        // POST: /ClientesContactos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesContacto clientescontacto = db.ClientesContactos.Find(id);
            var cliente_id = clientescontacto.ClienteID;
            if (db.ClientesContactos.Count() > 1)
            {            
                if (clientescontacto.flgPrincipal == 0)
                {
                    db.ClientesContactos.Remove(clientescontacto);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", routeValues: new { ClienteID = cliente_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}