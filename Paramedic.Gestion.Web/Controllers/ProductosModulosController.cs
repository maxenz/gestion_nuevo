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
    public class ProductosModulosController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ProductosModulos/

        public ActionResult Index(int ProductoID, String searchName = null, int page = 1)
        {

            var qProdMod = from p in db.ProductosModulos where p.ProductoID == ProductoID select p;

            if (qProdMod == null)
            {
                return HttpNotFound();
            }

            if (!String.IsNullOrEmpty(searchName))
            {

                qProdMod = qProdMod.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            qProdMod = qProdMod.OrderByDescending(p => p.Descripcion);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductosModulos", qProdMod.ToPagedList(page, 6));
            }

            return View(qProdMod.ToPagedList(page, 6));
        }

        //
        // GET: /ProductosModulos/Details/5

        public ActionResult Details(int id = 0)
        {
            ProductosModulo productosmodulo = db.ProductosModulos.Find(id);
            if (productosmodulo == null)
            {
                return HttpNotFound();
            }
            return View(productosmodulo);
        }

        //
        // GET: /ProductosModulos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductosModulos/Create

        [HttpPost]
        public ActionResult Create(ProductosModulo productosmodulo)
        {
            if (ModelState.IsValid)
            {
                db.ProductosModulos.Add(productosmodulo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productosmodulo);
        }

        //
        // GET: /ProductosModulos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ProductosModulo productosmodulo = db.ProductosModulos.Find(id);
            if (productosmodulo == null)
            {
                return HttpNotFound();
            }
            return View(productosmodulo);
        }

        //
        // POST: /ProductosModulos/Edit/5

        [HttpPost]
        public ActionResult Edit(ProductosModulo productosmodulo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productosmodulo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productosmodulo);
        }

        //
        // GET: /ProductosModulos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ProductosModulo productosmodulo = db.ProductosModulos.Find(id);
            if (productosmodulo == null)
            {
                return HttpNotFound();
            }
            return View(productosmodulo);
        }

        //
        // POST: /ProductosModulos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductosModulo productosmodulo = db.ProductosModulos.Find(id);
            db.ProductosModulos.Remove(productosmodulo);
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