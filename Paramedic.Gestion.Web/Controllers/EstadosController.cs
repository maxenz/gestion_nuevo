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
    [Authorize(Roles="Administrador")]

    public class EstadosController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /Estados/

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var qEstados = from p in db.Estados select p;

            if (!String.IsNullOrEmpty(searchName))
            {

                qEstados = qEstados.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            qEstados = qEstados.OrderBy(p => p.Numero);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Estados", qEstados.ToPagedList(page, 6));
            }

            return View(qEstados.ToPagedList(page, 6));
        }

        //
        // GET: /Estados/Details/5

        public ActionResult Details(int id = 0)
        {
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        //
        // GET: /Estados/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Estados/Create

        [HttpPost]
        public ActionResult Create(Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Estados.Add(estado);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(estado);
        }

        //
        // GET: /Estados/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        //
        // POST: /Estados/Edit/5

        [HttpPost]
        public ActionResult Edit(Estado estado)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estado).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        //
        // GET: /Estados/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Estado estado = db.Estados.Find(id);
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        //
        // POST: /Estados/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Estado estado = db.Estados.Find(id);
            db.Estados.Remove(estado);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}