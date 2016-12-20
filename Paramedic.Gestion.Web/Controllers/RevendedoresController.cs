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

    public class RevendedoresController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /Revendedores/

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var qRevendedores = from p in db.Revendedores select p;

            if (!String.IsNullOrEmpty(searchName))
            {

                qRevendedores = qRevendedores.Where(p => p.Nombre.ToUpper().Contains(searchName.ToUpper()));
            }

            qRevendedores = qRevendedores.OrderBy(p => p.Nombre);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Revendedores", qRevendedores.ToPagedList(page, 6));
            }

            return View(qRevendedores.ToPagedList(page, 6));
        }

        //
        // GET: /Revendedores/Details/5

        public ActionResult Details(int id = 0)
        {
            Revendedor revendedor = db.Revendedores.Find(id);
            if (revendedor == null)
            {
                return HttpNotFound();
            }
            return View(revendedor);
        }

        //
        // GET: /Revendedores/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Revendedores/Create

        [HttpPost]
        public ActionResult Create(Revendedor revendedor)
        {
            if (ModelState.IsValid)
            {
                db.Revendedores.Add(revendedor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(revendedor);
        }

        //
        // GET: /Revendedores/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Revendedor revendedor = db.Revendedores.Find(id);
            if (revendedor == null)
            {
                return HttpNotFound();
            }
            return View(revendedor);
        }

        //
        // POST: /Revendedores/Edit/5

        [HttpPost]
        public ActionResult Edit(Revendedor revendedor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(revendedor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(revendedor);
        }

        //
        // GET: /Revendedores/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Revendedor revendedor = db.Revendedores.Find(id);
            if (revendedor == null)
            {
                return HttpNotFound();
            }
            return View(revendedor);
        }

        //
        // POST: /Revendedores/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Revendedor revendedor = db.Revendedores.Find(id);
            db.Revendedores.Remove(revendedor);
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