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
    public class ProductosController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /Productos/

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var qProductos = from p in db.Productos select p;

            if (!String.IsNullOrEmpty(searchName))
            {

                qProductos = qProductos.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            qProductos = qProductos.OrderBy(p => p.Numero);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Productos", qProductos.ToPagedList(page, 6));
            }

            return View(qProductos.ToPagedList(page, 6));
        }


        //
        // GET: /Productos/Details/5

        public ActionResult Details(int id = 0)
        {
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //
        // GET: /Productos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Productos/Create

        [HttpPost]
        public ActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Productos.Add(producto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        //
        // GET: /Productos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //
        // POST: /Productos/Edit/5

        [HttpPost]
        public ActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        //
        // GET: /Productos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Producto producto = db.Productos.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        //
        // POST: /Productos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Productos.Find(id);
            db.Productos.Remove(producto);
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