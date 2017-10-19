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
    public class ProyectosController : Controller
    {

        #region Properties

        IProyectoService _ProyectoService;
		IClasificacionesProyectoService _ClasificacionService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ProyectosController(IProyectoService ProyectoService, IClasificacionesProyectoService ClasificacionService)
        {
			_ProyectoService = ProyectoService;
			_ClasificacionService = ClasificacionService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<Proyecto>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Proyecto> proyectos = _ProyectoService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _ProyectoService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Proyecto>(proyectos, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Proyectos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
			generateClasificacionDropdown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
                _ProyectoService.Create(proyecto);
                return RedirectToAction("Index");
            }

            return View(proyecto);
        }

        public ActionResult Edit(int id = 0)
        {
			generateClasificacionDropdown();
            Proyecto proyecto = _ProyectoService.GetById(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        [HttpPost]
        public ActionResult Edit(Proyecto proyecto)
        {
            if (ModelState.IsValid)
            {
				_ProyectoService.Update(proyecto);
                return RedirectToAction("Index");
            }
            return View(proyecto);
        }

        public ActionResult Delete(int id = 0)
        {
            Proyecto proyecto = _ProyectoService.GetById(id);
            if (proyecto == null)
            {
                return HttpNotFound();
            }
            return View(proyecto);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Proyecto proyecto = _ProyectoService.GetById(id);
            _ProyectoService.Delete(proyecto);
            return RedirectToAction("Index");
        }

		#endregion

		#region Private Methods

		private void generateClasificacionDropdown()
		{
			ViewBag.ClasificacionesList = new SelectList(_ClasificacionService.GetAll(), "Id", "Descripcion", "ClasificacionId");
		}

		#endregion

	}
}