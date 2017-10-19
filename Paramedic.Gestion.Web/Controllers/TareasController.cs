using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Net;
using LinqKit;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TareasController : Controller
    {

        #region Properties

        IProyectoService _ProyectoService;
        ITareaService _TareaService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public TareasController(IProyectoService ProyectoService, ITareaService TareaService)
        {
			_ProyectoService = ProyectoService;
			_TareaService = TareaService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ProyectoId, string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<Tarea>();
            predicate = predicate.And(x => x.ProyectoId == ProyectoId);
            if (!string.IsNullOrEmpty(searchName))
            {
                predicate = predicate.And(x => x.Descripcion.Contains(searchName));
            }

            IEnumerable<Tarea> tareas = _TareaService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _TareaService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Tarea>(tareas, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Tareas", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                _TareaService.Create(tarea);
                return RedirectToAction("Index");
            }

            return View(tarea);
        }

        public ActionResult Edit(int id = 0)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
			Tarea tarea = _TareaService.GetById(id);
            if (tarea == null)
            {
                return HttpNotFound();
            }
            return View(tarea);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tarea tarea)
        {
            if (ModelState.IsValid)
            {
                _TareaService.Update(tarea);
                return RedirectToAction("Index");
            }
            return View(tarea);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
			Tarea tarea = _TareaService.GetById(id);
			_TareaService.Delete(tarea);
            return RedirectToAction("Index");
        }

        #endregion

    }
}