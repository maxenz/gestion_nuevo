using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PaisesController : Controller
    {

        IPaisService _PaisService;

        public PaisesController(IPaisService PaisService)
        {
            _PaisService = PaisService;
        }


        public ActionResult Index(string searchName = null, int page = 1)
        {

            //var paisesFromRepo = _PaisService.GetAll();

            IEnumerable<Pais> paises;

            if (!String.IsNullOrEmpty(searchName))
            {
                paises = _PaisService.GetAll()
                    .Where(p => p.Descripcion.ToUpper()
                    .Contains(searchName.ToUpper()))
                    .OrderBy(p => p.Descripcion);
            }
            else
            {
                paises = _PaisService.GetAll()
                .OrderBy(p => p.Descripcion);
            }

            if (Request != null)
            {
                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Paises", paises.ToPagedList(page, 6));
                }
            }

            return View(paises.ToPagedList(page, 6));
        }

        //
        // GET: /Paises/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Paises/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pais pais)
        {
            if (ModelState.IsValid)
            {
                _PaisService.Create(pais);
                return RedirectToAction("Index");
            }

            return View(pais);
        }

        //
        // GET: /Paises/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Pais pais = _PaisService.GetById(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        //
        // POST: /Paises/Edit/5

        [HttpPost]
        public ActionResult Edit(Pais pais)
        {
            if (ModelState.IsValid)
            {
                _PaisService.Update(pais);
                return RedirectToAction("Index");
            }
            return View(pais);
        }

        //
        // GET: /Paises/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Pais pais = _PaisService.GetById(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        //
        // POST: /Paises/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pais pais = _PaisService.GetById(id);
            _PaisService.Delete(pais);
            return RedirectToAction("Index");
        }

    }
}