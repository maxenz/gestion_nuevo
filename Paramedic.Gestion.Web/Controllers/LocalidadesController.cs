using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Net;
using LinqKit;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LocalidadesController : Controller
    {
        #region Properties

        ILocalidadService _LocalidadService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public LocalidadesController(ILocalidadService LocalidadService)
        {
            _LocalidadService = LocalidadService;
        }


        #endregion

        #region Public Methods

        public ActionResult Index(int ProvinciaID, String searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Localidad>();
            predicate = predicate.And(x => x.ProvinciaId == ProvinciaID);
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Localidad> localidades = _LocalidadService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _LocalidadService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Localidad>(localidades, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Localidades", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Localidad localidad = _LocalidadService.GetById(id);
                _LocalidadService.Delete(localidad);
                return RedirectToAction("Index");
            } catch
            {
                return RedirectToAction("Index");
            }

        }

        #endregion

    }
}