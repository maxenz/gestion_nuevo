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
    public class ProductosController : Controller
    {
        #region Properties

        IProductoService _ProductoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ProductosController(IProductoService ProductoService)
        {
            _ProductoService = ProductoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Producto>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Producto> productos = _ProductoService.FindByPage(predicate, "Numero ASC", controllersPageSize, page);
            int count = _ProductoService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Producto>(productos, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Productos", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _ProductoService.Create(producto);
                return RedirectToAction("Index");
            }

            return View(producto);
        }

        public ActionResult Edit(int id = 0)
        {
            Producto producto = _ProductoService.FindBy(x => x.Id == id).FirstOrDefault();
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }

        [HttpPost]
        public ActionResult Edit(Producto producto)
        {
            if (ModelState.IsValid)
            {
                _ProductoService.Update(producto);
                return RedirectToAction("Index");
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = _ProductoService.FindBy(x => x.Id == id).FirstOrDefault();
            _ProductoService.Delete(producto);
            return RedirectToAction("Index");
        }


        #endregion
    }
}