using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using LinqKit;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class NoticiasController : Controller
    {
        #region Properties

        INoticiaService _NoticiaService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public NoticiasController(INoticiaService NoticiaService)
        {
            _NoticiaService = NoticiaService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Noticia>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Noticia> estados = _NoticiaService.FindByPage(predicate, "FechaVencimiento DESC", controllersPageSize, page);
            int count = _NoticiaService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Noticia>(estados, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Noticias", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                _NoticiaService.Create(noticia);
                return RedirectToAction("Index");
            }

            return View(noticia);
        }

        public ActionResult Edit(int id = 0)
        {
            Noticia noticia = _NoticiaService.FindBy(x => x.Id == id).FirstOrDefault();
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        [HttpPost]
        public ActionResult Edit(Noticia noticia)
        {
            if (ModelState.IsValid)
            {
                _NoticiaService.Update(noticia);
                return RedirectToAction("Index");
            }
            return View(noticia);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
			Noticia noticia = _NoticiaService.FindBy(x => x.Id == id).FirstOrDefault();
			_NoticiaService.Delete(noticia);
            return RedirectToAction("Index");
        }

        #endregion
    }
}