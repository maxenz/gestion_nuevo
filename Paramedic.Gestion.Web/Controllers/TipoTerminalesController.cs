using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using Paramedic.Gestion.Model;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TipoTerminalesController : Controller
    {

        #region Properties

        ITipoTerminalService _TipoTerminalService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public TipoTerminalesController(ITipoTerminalService TipoTerminalService)
        {
            _TipoTerminalService = TipoTerminalService;
        }


        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<TipoTerminal>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<TipoTerminal> tipoTerminales = _TipoTerminalService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _TipoTerminalService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<TipoTerminal>(tipoTerminales, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_TipoTerminales", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TipoTerminal tipoterminal)
        {
            if (ModelState.IsValid)
            {
                _TipoTerminalService.Create(tipoterminal);
                return RedirectToAction("Index");
            }

            return View(tipoterminal);
        }

        public ActionResult Edit(int id = 0)
        {
            TipoTerminal tipoterminal = _TipoTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
            if (tipoterminal == null)
            {
                return HttpNotFound();
            }
            return View(tipoterminal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TipoTerminal tipoterminal)
        {
            if (ModelState.IsValid)
            {
                _TipoTerminalService.Update(tipoterminal);
                return RedirectToAction("Index");
            }
            return View(tipoterminal);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TipoTerminal tipoterminal = _TipoTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
            _TipoTerminalService.Delete(tipoterminal);
            return RedirectToAction("Index");
        }

        #endregion

    }
}