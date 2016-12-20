using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Net;

namespace Gestion.Controllers
{
    //[Authorize(Roles = "Administrador")]
    public class ProvinciasController : Controller
    {
        IPaisService _PaisesService;
        IProvinciaService _ProvinciasService;

        public ProvinciasController(IPaisService PaisesService, IProvinciaService ProvinciasService)
        {
            _PaisesService = PaisesService;
            _ProvinciasService = ProvinciasService;
        }

        //
        // GET: /Provincias/
        public ActionResult Index(int PaisID, String searchName = null, int page = 1)
        {
            IEnumerable<Provincia> provincias = _ProvinciasService
                .GetAll()
                .Where(x => x.PaisId == PaisID)
                .OrderBy(x => x.Descripcion);

            if (!String.IsNullOrEmpty(searchName))
            {

                provincias = provincias.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            if (Request != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Provincias", provincias.ToPagedList(page, 6));
                }
            }

            return View(provincias.ToPagedList(page, 6));
        }

        //
        // GET: /Provincias/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Provincias/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Provincia provincia)
        {
            if (ModelState.IsValid)
            {
                _ProvinciasService.Create(provincia);
                return RedirectToAction("Index");
            }

            return View(provincia);
        }

        //
        // GET: /Provincias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Provincia provincia = _ProvinciasService.GetById(id);
            if (provincia == null)
            {
                return HttpNotFound();
            }
            return View(provincia);
        }

        //
        // POST: /Provincias/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Provincia provincia)
        {
            if (ModelState.IsValid)
            {
                _ProvinciasService.Update(provincia);
                return RedirectToAction("Index");
            }
            return View(provincia);
        }



        //
        // POST: /Provincias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Provincia provincia = _ProvinciasService.GetById(id);
            _ProvinciasService.Delete(provincia);
            return RedirectToAction("Index");
        }

    }
}