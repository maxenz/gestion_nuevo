using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using LinqKit;
using System.Collections.Generic;
using PagedList;

namespace Gestion.Controllers
{
	[Authorize(Roles = "Administrador")]
	public class ProductosModulosIntentosController : Controller
	{
		#region Properties

		IProductoService _ProductoService;
		IProductosModuloService _ProductosModuloService;
		IProductosModulosIntentoService _ProductosModulosIntentoService;
		private int controllersPageSize = 6;

		#endregion

		#region Constructors

		public ProductosModulosIntentosController(IProductoService ProductoService, IProductosModuloService ProductosModuloService, IProductosModulosIntentoService ProductosModulosIntentoService)
		{
			_ProductoService = ProductoService;
			_ProductosModuloService = ProductosModuloService;
			_ProductosModulosIntentoService = ProductosModulosIntentoService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(int ProductoId, int ProductosModuloId, string searchName = null, int page = 1)
		{
			var predicate = PredicateBuilder.New<ProductosModulosIntento>();
			predicate = predicate.And(x => x.ProductosModuloId == ProductosModuloId);

			IEnumerable<ProductosModulosIntento> intentos = _ProductosModulosIntentoService.FindByPage(predicate, "Orden ASC", controllersPageSize, page);
			int count = _ProductosModulosIntentoService.FindBy(predicate).Count();
			var resultAsPagedList = new StaticPagedList<ProductosModulosIntento>(intentos, page, controllersPageSize, count);

			if (Request.IsAjaxRequest())
			{
				return PartialView("_Intentos", resultAsPagedList);
			}

			return View(resultAsPagedList);
		}

		public ActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public ActionResult Create(ProductosModulosIntento intento)
		{
			if (ModelState.IsValid)
			{
				_ProductosModulosIntentoService.Create(intento);
				return RedirectToAction("Index");
			}

			return View(intento);
		}

		public ActionResult Edit(int id)
		{
			ProductosModulosIntento intento = _ProductosModulosIntentoService.FindBy(x => x.Id == id).FirstOrDefault();
			if (intento == null)
			{
				return HttpNotFound();
			}
			return View(intento);
		}

		[HttpPost]
		public ActionResult Edit(ProductosModulosIntento intento)
		{
			if (ModelState.IsValid)
			{
				_ProductosModulosIntentoService.Update(intento);
				return RedirectToAction("Index");
			}
			return View(intento);
		}

		[HttpPost, ActionName("Delete")]
		public ActionResult Delete(int id)
		{
			ProductosModulosIntento intento = _ProductosModulosIntentoService.FindBy(x => x.Id == id).FirstOrDefault();
			_ProductosModulosIntentoService.Delete(intento);

			return RedirectToAction("Index");
		}

		#endregion
	}
}