using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Gestion.Models;
using PagedList;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SitiosController : Controller
    {
        private GestionDb db = new GestionDb();

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var sites = from s in db.Sitios select s;

            if (!string.IsNullOrEmpty(searchName))
            {
                sites = sites.Where(s => s.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            sites = sites.OrderBy(p => p.Descripcion);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Sitios", sites.ToPagedList(page, 6));
            }

            return View(sites.ToPagedList(page, 6));
        }
        public ActionResult Details(int id = 0)
        {
            Sitio site = db.Sitios.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sitio site)
        {
            if (ModelState.IsValid)
            {
                db.Sitios.Add(site);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(site);
        }

        public ActionResult Edit(int id = 0)
        {
            Sitio site = db.Sitios.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        [HttpPost]
        public ActionResult Edit(Sitio site)
        {
            if (ModelState.IsValid)
            {
                db.Entry(site).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(site);
        }

        public ActionResult Delete(int id = 0)
        {
            Sitio site = db.Sitios.Find(id);
            if (site == null)
            {
                return HttpNotFound();
            }
            return View(site);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sitio site = db.Sitios.Find(id);
            db.Sitios.Remove(site);
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
