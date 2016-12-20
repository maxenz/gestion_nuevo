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
    public class MediosDifusionController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /MediosDifusion/


        public ActionResult Index(string searchName = null, int page = 1)
        {
            var qMediosDif = from p in db.MediosDifusion select p;

            if (!String.IsNullOrEmpty(searchName))
            {

                qMediosDif = qMediosDif.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            qMediosDif = qMediosDif.OrderBy(p => p.Descripcion);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MediosDifusion", qMediosDif.ToPagedList(page, 6));
            }

            return View(qMediosDif.ToPagedList(page, 6));
        }

        //
        // GET: /MediosDifusion/Details/5

        public ActionResult Details(int id = 0)
        {
            MedioDifusion mediodifusion = db.MediosDifusion.Find(id);
            if (mediodifusion == null)
            {
                return HttpNotFound();
            }
            return View(mediodifusion);
        }

        //
        // GET: /MediosDifusion/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MediosDifusion/Create

        [HttpPost]
        public ActionResult Create(MedioDifusion mediodifusion)
        {
            if (ModelState.IsValid)
            {
                db.MediosDifusion.Add(mediodifusion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mediodifusion);
        }

        //
        // GET: /MediosDifusion/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MedioDifusion mediodifusion = db.MediosDifusion.Find(id);
            if (mediodifusion == null)
            {
                return HttpNotFound();
            }
            return View(mediodifusion);
        }

        //
        // POST: /MediosDifusion/Edit/5

        [HttpPost]
        public ActionResult Edit(MedioDifusion mediodifusion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mediodifusion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mediodifusion);
        }

        //
        // GET: /MediosDifusion/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MedioDifusion mediodifusion = db.MediosDifusion.Find(id);
            if (mediodifusion == null)
            {
                return HttpNotFound();
            }
            return View(mediodifusion);
        }

        //
        // POST: /MediosDifusion/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MedioDifusion mediodifusion = db.MediosDifusion.Find(id);
            db.MediosDifusion.Remove(mediodifusion);
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