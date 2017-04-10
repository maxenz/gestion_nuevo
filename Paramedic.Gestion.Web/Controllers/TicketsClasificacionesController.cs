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
    public class TicketsClasificacionesController : Controller
    {
        #region Properties

        ITicketsClasificacionService _TicketsClasificacionService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public TicketsClasificacionesController(ITicketsClasificacionService TicketsClasificacionService)
        {
            _TicketsClasificacionService = TicketsClasificacionService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<TicketsClasificacion>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<TicketsClasificacion> clasificaciones =
                _TicketsClasificacionService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _TicketsClasificacionService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<TicketsClasificacion>(clasificaciones, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Clasificaciones", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(TicketsClasificacion clasificacion)
        {
            if (ModelState.IsValid)
            {
                _TicketsClasificacionService.Create(clasificacion);
                return RedirectToAction("Index");
            }

            return View(clasificacion);
        }

        public ActionResult Edit(int id = 0)
        {
            TicketsClasificacion clasificacion = _TicketsClasificacionService.FindBy(x => x.Id == id).FirstOrDefault();
            if (clasificacion == null)
            {
                return HttpNotFound();
            }
            return View(clasificacion);
        }

        [HttpPost]
        public ActionResult Edit(TicketsClasificacion clasificacion)
        {
            if (ModelState.IsValid)
            {
                _TicketsClasificacionService.Update(clasificacion);
                return RedirectToAction("Index");
            }
            return View(clasificacion);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketsClasificacion clasificacion = _TicketsClasificacionService.FindBy(x => x.Id == id).FirstOrDefault();
            _TicketsClasificacionService.Delete(clasificacion);
            return RedirectToAction("Index");
        }


        #endregion
    }
}