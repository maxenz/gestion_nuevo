using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Collections.Generic;
using LinqKit;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductosModulosController : Controller
    {

		#region Properties

		IProductoService _ProductoService;
		IProductosModuloService _ProductosModuloService;
		IProductosModulosIntentoService _ProductosModulosIntentoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ProductosModulosController(
			IProductoService ProductoService,
			IProductosModuloService ProductosModuloService,
			IProductosModulosIntentoService ProductosModulosIntentoService
			)
        {
			_ProductoService = ProductoService;
			_ProductosModuloService = ProductosModuloService;
			_ProductosModulosIntentoService = ProductosModulosIntentoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ProductoID, string searchName = null, int page = 1)
        {
			var predicate = PredicateBuilder.New<ProductosModulo>();
			predicate = predicate.And(x => x.ProductoId == ProductoID);
			if (!string.IsNullOrEmpty(searchName))
			{
				predicate.And(x => x.Descripcion.Contains(searchName));
			}

			IEnumerable<ProductosModulo> modulos = _ProductosModuloService.FindByPage(predicate, "Codigo ASC", controllersPageSize, page);
			int count = _ProductosModuloService.FindBy(predicate).Count();
			var resultAsPagedList = new StaticPagedList<ProductosModulo>(modulos, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_ProductosModulos", resultAsPagedList);
			}

			return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
			ViewBag.IsCreateForm = true;
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductosModulo productosmodulo)
        {
            if (ModelState.IsValid)
            {
				_ProductosModuloService.Create(productosmodulo);
                return RedirectToAction("Index");
            }

            return View(productosmodulo);
        }

        public ActionResult Edit(int id = 0)
        {
			ProductosModulo modulo = _ProductosModuloService.FindBy(x => x.Id == id).FirstOrDefault();
            if (modulo == null)
            {
                return HttpNotFound();
            }
            return View(modulo);
        }

        [HttpPost]
        public ActionResult Edit(ProductosModulo productosmodulo)
        {
            if (ModelState.IsValid)
            {
				_ProductosModuloService.Update(productosmodulo);
                return RedirectToAction("Index");
            }
            return View(productosmodulo);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

			ProductosModulo modulo = _ProductosModuloService.FindBy(x => x.Id == id).FirstOrDefault();
			_ProductosModuloService.Delete(modulo);

			return RedirectToAction("Index");
        }

		#endregion

	}
}