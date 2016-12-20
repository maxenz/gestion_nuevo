using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesTerminalesController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ClientesTerminales/

        public ActionResult Index(int ClienteID, String searchName = null, int page = 1)
        {

            var qTerminales = from t in db.ClientesTerminales where t.ClienteID == ClienteID select t;

            if (qTerminales == null)
            {
                return HttpNotFound();
            }

            qTerminales = qTerminales.OrderBy(p => p.TipoTerminal.Descripcion);

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesTerminales", qTerminales.ToList());

        }

        //
        // GET: /ClientesTerminales/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientesTerminal clientesterminal = db.ClientesTerminales.Find(id);
            if (clientesterminal == null)
            {
                return HttpNotFound();
            }
            return View(clientesterminal);
        }

        //
        // GET: /ClientesTerminales/Create

        public ActionResult Create()
        {
            ViewBag.TipoTerminales = db.TipoTerminales.ToList();
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial");
            ViewBag.TipoTerminalID = new SelectList(db.TipoTerminales, "id", "descripcion");
            return View();
        }

        //
        // POST: /ClientesTerminales/Create

        [HttpPost]
        public ActionResult Create(ClientesTerminal clientesterminal)
        {
            if (ModelState.IsValid)
            {
                db.ClientesTerminales.Add(clientesterminal);
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteID });
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clientesterminal.ClienteID);
            ViewBag.TipoTerminalID = new SelectList(db.TipoTerminales, "id", "descripcion", clientesterminal.TipoTerminalID);
            ViewBag.TipoTerminales = db.TipoTerminales.ToList();
            return View(clientesterminal);
        }

        //
        // GET: /ClientesTerminales/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.TipoTerminales = db.TipoTerminales.ToList();
            ClientesTerminal clientesterminal = db.ClientesTerminales.Find(id);
            if (clientesterminal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clientesterminal.ClienteID);
           // ViewBag.TipoTerminalID = new SelectList(db.TipoTerminales, "ID", "Descripcion", clientesterminal.TipoTerminalID);
            return View(clientesterminal);
        }

        //
        // POST: /ClientesTerminales/Edit/5

        [HttpPost]
        public ActionResult Edit(ClientesTerminal clientesterminal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientesterminal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteID });
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clientesterminal.ClienteID);
            ViewBag.TipoTerminalID = new SelectList(db.TipoTerminales, "ID", "Descripcion", clientesterminal.TipoTerminalID);
            return View(clientesterminal);
        }

        //
        // GET: /ClientesTerminales/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientesTerminal clientesterminal = db.ClientesTerminales.Find(id);
            if (clientesterminal == null)
            {
                return HttpNotFound();
            }
            return View(clientesterminal);
        }

        //
        // POST: /ClientesTerminales/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesTerminal clientesterminal = db.ClientesTerminales.Find(id);
            var cliente_id = clientesterminal.ClienteID;
            db.ClientesTerminales.Remove(clientesterminal);
            db.SaveChanges();
            return RedirectToAction("Index", routeValues: new { ClienteID = cliente_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}