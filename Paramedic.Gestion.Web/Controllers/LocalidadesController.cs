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
    public class LocalidadesController : Controller
    {
        ILocalidadService _LocalidadService;

        public LocalidadesController(ILocalidadService LocalidadService)
        {
            _LocalidadService = LocalidadService;
        }

        //
        // GET: /Provincias/
        public ActionResult Index(int ProvinciaID, String searchName = null, int page = 1)
        {
            IEnumerable<Localidad> localidades = _LocalidadService
                .GetAll()
                .Where(x => x.ProvinciaId == ProvinciaID)
                .OrderBy(x => x.Descripcion);

            if (!String.IsNullOrEmpty(searchName))
            {

                localidades = localidades.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper()));
            }

            if (Request != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Localidades", localidades.ToPagedList(page, 6));
                }
            }

            return View(localidades.ToPagedList(page, 6));
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
        public ActionResult Create(Localidad localidad)
        {
            if (ModelState.IsValid)
            {
                _LocalidadService.Create(localidad);
                return RedirectToAction("Index");
            }

            return View(localidad);
        }

        //
        // GET: /Provincias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Localidad localidad = _LocalidadService.GetById(id);
            if (localidad == null)
            {
                return HttpNotFound();
            }
            return View(localidad);
        }

        //
        // POST: /Provincias/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Localidad localidad)
        {
            if (ModelState.IsValid)
            {
                _LocalidadService.Update(localidad);
                return RedirectToAction("Index");
            }
            return View(localidad);
        }



        //
        // POST: /Provincias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Localidad localidad = _LocalidadService.GetById(id);
            _LocalidadService.Delete(localidad);
            return RedirectToAction("Index");
        }

    }
}