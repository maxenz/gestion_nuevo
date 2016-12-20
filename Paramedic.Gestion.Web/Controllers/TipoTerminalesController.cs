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
    public class TipoTerminalesController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /TipoTerminales/

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var tTerminales = from t in db.TipoTerminales
                              select t;
            if (!String.IsNullOrEmpty(searchName))
            {
                tTerminales = tTerminales.Where(t => t.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            tTerminales = tTerminales.OrderBy(t => t.Descripcion);


            if (Request.IsAjaxRequest())
            {
                return PartialView("_TipoTerminales", tTerminales.ToPagedList(page, 6));
            }

            return View(tTerminales.ToPagedList(page, 6));
        }

        //
        // GET: /TipoTerminales/Details/5

        public ActionResult Details(int id = 0)
        {
            TipoTerminal tipoterminal = db.TipoTerminales.Find(id);
            if (tipoterminal == null)
            {
                return HttpNotFound();
            }
            return View(tipoterminal);
        }

        //
        // GET: /TipoTerminales/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /TipoTerminales/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoTerminal tipoterminal)
        {
            if (ModelState.IsValid)
            {
                db.TipoTerminales.Add(tipoterminal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tipoterminal);
        }

        //
        // GET: /TipoTerminales/Edit/5

        public ActionResult Edit(int id = 0)
        {
            TipoTerminal tipoterminal = db.TipoTerminales.Find(id);
            if (tipoterminal == null)
            {
                return HttpNotFound();
            }
            return View(tipoterminal);
        }

        //
        // POST: /TipoTerminales/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoTerminal tipoterminal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tipoterminal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tipoterminal);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoTerminal tipoterminal = db.TipoTerminales.Find(id);
            db.TipoTerminales.Remove(tipoterminal);
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