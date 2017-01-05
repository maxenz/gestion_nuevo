using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using LinqKit;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SitiosController : Controller
    {
        #region Properties

        ISitioService _SitioService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public SitiosController(ISitioService SitioService)
        {
            _SitioService = SitioService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Sitio>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Sitio> sitios = _SitioService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _SitioService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Sitio>(sitios, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Sitios", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Sitio sitio)
        {
            if (ModelState.IsValid)
            {
                _SitioService.Create(sitio);
                return RedirectToAction("Index");
            }

            return View(sitio);
        }

        public ActionResult Edit(int id = 0)
        {
            Sitio sitio = _SitioService.FindBy(x => x.Id == id).FirstOrDefault();
            if (sitio == null)
            {
                return HttpNotFound();
            }
            return View(sitio);
        }

        [HttpPost]
        public ActionResult Edit(Sitio sitio)
        {
            if (ModelState.IsValid)
            {
                _SitioService.Update(sitio);
                return RedirectToAction("Index");
            }
            return View(sitio);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Sitio sitio = _SitioService.FindBy(x => x.Id == id).FirstOrDefault();
            _SitioService.Delete(sitio);
            return RedirectToAction("Index");
        }


        #endregion
    }
}