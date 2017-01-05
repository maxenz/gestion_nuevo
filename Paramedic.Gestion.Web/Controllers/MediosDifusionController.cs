using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using Paramedic.Gestion.Model;

namespace Gestion.Controllers
{
    public class MediosDifusionController : Controller
    {
        #region Properties

        IMedioDifusionService _MedioDifusionService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public MediosDifusionController(IMedioDifusionService MedioDifusionService)
        {
            _MedioDifusionService = MedioDifusionService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<MedioDifusion>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<MedioDifusion> medios = _MedioDifusionService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _MedioDifusionService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<MedioDifusion>(medios, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MediosDifusion", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(MedioDifusion medio)
        {
            if (ModelState.IsValid)
            {
                _MedioDifusionService.Create(medio);
                return RedirectToAction("Index");
            }

            return View(medio);
        }

        public ActionResult Edit(int id = 0)
        {
            MedioDifusion medio = _MedioDifusionService.FindBy(x => x.Id == id).FirstOrDefault();
            if (medio == null)
            {
                return HttpNotFound();
            }
            return View(medio);
        }

        [HttpPost]
        public ActionResult Edit(MedioDifusion medio)
        {
            if (ModelState.IsValid)
            {
                _MedioDifusionService.Update(medio);
                return RedirectToAction("Index");
            }
            return View(medio);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MedioDifusion medio = _MedioDifusionService.FindBy(x => x.Id == id).FirstOrDefault();
            _MedioDifusionService.Delete(medio);
            return RedirectToAction("Index");
        }

        #endregion

    }
}