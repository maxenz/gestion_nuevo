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
    public class ClasificacionesProyectosController : Controller
    {
        #region Properties

		IClasificacionesProyectoService _ClasificacionService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ClasificacionesProyectosController(IClasificacionesProyectoService ClasificacionService)
        {
			_ClasificacionService = ClasificacionService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<ClasificacionesProyecto>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<ClasificacionesProyecto> clasificaciones = _ClasificacionService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _ClasificacionService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<ClasificacionesProyecto>(clasificaciones, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ClasificacionesProyectos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClasificacionesProyecto clasificacion)
        {
            if (ModelState.IsValid)
            {
                _ClasificacionService.Create(clasificacion);
                return RedirectToAction("Index");
            }

            return View(clasificacion);
        }

        public ActionResult Edit(int id = 0)
        {
            ClasificacionesProyecto clasificacion = _ClasificacionService.GetById(id);
            if (clasificacion == null)
            {
                return HttpNotFound();
            }
            return View(clasificacion);
        }

        [HttpPost]
        public ActionResult Edit(ClasificacionesProyecto clasificacion)
        {
            if (ModelState.IsValid)
            {
				_ClasificacionService.Update(clasificacion);
                return RedirectToAction("Index");
            }
            return View(clasificacion);
        }

        public ActionResult Delete(int id = 0)
        {
            ClasificacionesProyecto clasificacion = _ClasificacionService.GetById(id);
            if (clasificacion == null)
            {
                return HttpNotFound();
            }
            return View(clasificacion);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClasificacionesProyecto clasificacion = _ClasificacionService.GetById(id);
			_ClasificacionService.Delete(clasificacion);
            return RedirectToAction("Index");
        }

		#endregion
	}
}