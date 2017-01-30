using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LicenciasController : Controller
    {

        #region Properties

        ILicenciaService _LicenciaService;
        IClientesLicenciaService _ClientesLicenciaService;
        IProductoService _ProductoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public LicenciasController(ILicenciaService LicenciaService, IProductoService ProductoService, IClientesLicenciaService ClientesLicenciaService)
        {
            _LicenciaService = LicenciaService;
            _ProductoService = ProductoService;
            _ClientesLicenciaService = ClientesLicenciaService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Licencia>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Serial.Contains(searchName)) : null;
            IEnumerable<ClientesLicencia> cliLics = _ClientesLicenciaService.GetAll();
            IEnumerable<Licencia> licencias = _LicenciaService.FindByPage(predicate, "Serial DESC", controllersPageSize, page);
            int count = _LicenciaService.FindBy(predicate).Count();

            foreach (Licencia lic in licencias)
            {
                ClientesLicencia cliLic = cliLics.Where(x => x.LicenciaId == lic.Id).FirstOrDefault();
                if (cliLic != null)
                {
                    lic.Estado = cliLic.Cliente.RazonSocial;
                }
            }

            var resultAsPagedList = new StaticPagedList<Licencia>(licencias, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Licencias", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                _LicenciaService.Create(licencia);
                return RedirectToAction("Index");
            }

            return View(licencia);
        }

        public ActionResult Edit(int id = 0)
        {
            Licencia licencia = _LicenciaService.FindBy(x => x.Id == id).FirstOrDefault();

            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        [HttpPost]
        public ActionResult Edit(Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                _LicenciaService.Update(licencia);
                return RedirectToAction("Index");
            }
            return View(licencia);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Licencia licencia = _LicenciaService.FindBy(x => x.Id == id).FirstOrDefault();
            _LicenciaService.Delete(licencia);
            return RedirectToAction("Index");
        }

        private void UpdateLicenciasProductos(string[] selectedProductos, Licencia licenciaToUpdate)
        {
            if (selectedProductos == null)
            {
                licenciaToUpdate.Productos = new List<Producto>();
                return;
            }

            var selectedProductosHS = new HashSet<string>(selectedProductos);
            var licenciasProductos = new HashSet<int>
                (licenciaToUpdate.Productos.Select(l => l.Id));
            foreach (var prod in _ProductoService.GetAll())
            {
                if (selectedProductosHS.Contains(prod.Id.ToString()))
                {
                    if (!licenciasProductos.Contains(prod.Id))
                    {
                        licenciaToUpdate.Productos.Add(prod);
                    }
                }
                else
                {

                    if (licenciasProductos.Contains(prod.Id))
                    {
                        licenciaToUpdate.Productos.Remove(prod);
                    }
                }
            }
        }

        #endregion

    }
}