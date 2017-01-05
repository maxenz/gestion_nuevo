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
    public class ProvinciasController : Controller
    {

        #region Properties

        IPaisService _PaisesService;
        IProvinciaService _ProvinciasService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ProvinciasController(IPaisService PaisesService, IProvinciaService ProvinciasService)
        {
            _PaisesService = PaisesService;
            _ProvinciasService = ProvinciasService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int PaisID, string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<Provincia>();
            predicate = predicate.And(x => x.PaisId == PaisID);
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Provincia> provincias = _ProvinciasService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _ProvinciasService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Provincia>(provincias, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Provincias", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

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

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Provincia provincia = _ProvinciasService.GetById(id);
            _ProvinciasService.Delete(provincia);
            return RedirectToAction("Index");
        }

        #endregion

    }
}