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
    public class RevendedoresController : Controller
    {
        #region Properties

        IRevendedorService _RevendedorService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public RevendedoresController(IRevendedorService RevendedorService)
        {
            _RevendedorService = RevendedorService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Revendedor>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Nombre.Contains(searchName)) : null;

            IEnumerable<Revendedor> revendedores = _RevendedorService.FindByPage(predicate, "Nombre ASC", controllersPageSize, page);
            int count = _RevendedorService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Revendedor>(revendedores, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Revendedores", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Revendedor revendedor)
        {
            if (ModelState.IsValid)
            {
                _RevendedorService.Create(revendedor);
                return RedirectToAction("Index");
            }

            return View(revendedor);
        }

        public ActionResult Edit(int id = 0)
        {
            Revendedor revendedor = _RevendedorService.FindBy(x => x.Id == id).FirstOrDefault();
            if (revendedor == null)
            {
                return HttpNotFound();
            }
            return View(revendedor);
        }

        [HttpPost]
        public ActionResult Edit(Revendedor revendedor)
        {
            if (ModelState.IsValid)
            {
                _RevendedorService.Update(revendedor);
                return RedirectToAction("Index");
            }
            return View(revendedor);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Revendedor revendedor = _RevendedorService.FindBy(x => x.Id == id).FirstOrDefault();
            _RevendedorService.Delete(revendedor);
            return RedirectToAction("Index");
        }


        #endregion
    }
}