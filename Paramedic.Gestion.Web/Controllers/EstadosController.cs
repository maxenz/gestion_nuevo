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
    public class EstadosController : Controller
    {
        #region Properties

        IEstadoService _EstadoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public EstadosController(IEstadoService EstadoService)
        {
            _EstadoService = EstadoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Estado>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Estado> estados = _EstadoService.FindByPage(predicate, "Numero ASC", controllersPageSize, page);
            int count = _EstadoService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Estado>(estados, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Estados", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Estado estado)
        {
            if (ModelState.IsValid)
            {
                _EstadoService.Create(estado);
                return RedirectToAction("Index");
            }

            return View(estado);
        }

        public ActionResult Edit(int id = 0)
        {
            Estado estado = _EstadoService.FindBy(x => x.Id == id).FirstOrDefault();
            if (estado == null)
            {
                return HttpNotFound();
            }
            return View(estado);
        }

        [HttpPost]
        public ActionResult Edit(Estado estado)
        {
            if (ModelState.IsValid)
            {
                _EstadoService.Update(estado);
                return RedirectToAction("Index");
            }
            return View(estado);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Estado estado = _EstadoService.FindBy(x => x.Id == id).FirstOrDefault();
            _EstadoService.Delete(estado);
            return RedirectToAction("Index");
        }


        #endregion
    }
}