using System;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ProductosModulosController : Controller
    {

        #region Properties

        IProductoService _ProductoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ProductosModulosController(IProductoService ProductoService)
        {
            _ProductoService = ProductoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ProductoID, string searchName = null, int page = 1)
        {

            Producto producto = _ProductoService.FindBy(x => x.Id == ProductoID).FirstOrDefault();

            ViewBag.ProductoId = producto.Id;

            IEnumerable<ProductosModulo> modulos = producto.ProductosModulos.OrderByDescending(x => x.Descripcion);

            if (!string.IsNullOrEmpty(searchName))
            {
                modulos = modulos.Where(x => x.Descripcion.Contains(searchName));
            }

            int count = modulos.Count();
            var resultAsPagedList = new StaticPagedList<ProductosModulo>(modulos, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_ProductosModulos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(ProductosModulo productosmodulo)
        {
            if (ModelState.IsValid)
            {
                Producto producto = _ProductoService.FindBy(x => x.Id == productosmodulo.ProductoId).FirstOrDefault();
                producto.ProductosModulos.Add(productosmodulo);
                _ProductoService.Update(producto);
                return RedirectToAction("Index");
            }

            return View(productosmodulo);
        }

        public ActionResult Edit(int id = 0, int productId = 0)
        {

            Producto producto = _ProductoService.FindBy(x => x.Id == productId).FirstOrDefault();
            ProductosModulo modulo = producto.ProductosModulos.Where(x => x.Id == id).FirstOrDefault();
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
                Producto producto = _ProductoService.FindBy(x => x.Id == productosmodulo.ProductoId).FirstOrDefault();
                producto.ProductosModulos = producto.ProductosModulos.Where(x => x.Id != productosmodulo.Id).ToList();
                producto.ProductosModulos.Add(productosmodulo);
                _ProductoService.Update(producto);
                return RedirectToAction("Index");
            }
            return View(productosmodulo);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, int productId)
        {

            Producto producto = _ProductoService.FindBy(x => x.Id == productId).FirstOrDefault();
            ProductosModulo modulo = producto.ProductosModulos.Where(x => x.Id == id).FirstOrDefault();

            producto.ProductosModulos.Remove(modulo);
            _ProductoService.Update(producto);

            return RedirectToAction("Index");
        }

        #endregion

    }
}