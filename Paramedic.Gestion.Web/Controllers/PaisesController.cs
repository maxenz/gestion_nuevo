using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class PaisesController : Controller
    {

        #region Properties

        IPaisService _PaisService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public PaisesController(IPaisService PaisService)
        {
            _PaisService = PaisService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<Pais>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Pais> paises = _PaisService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _PaisService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Pais>(paises, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Paises", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        public ActionResult Edit(int id = 0)
        {
            Pais pais = _PaisService.GetById(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

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

        public ActionResult Delete(int id = 0)
        {
            Pais pais = _PaisService.GetById(id);
            if (pais == null)
            {
                return HttpNotFound();
            }
            return View(pais);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Pais pais = _PaisService.GetById(id);
            _PaisService.Delete(pais);
            return RedirectToAction("Index");
        }

        #endregion

    }
}