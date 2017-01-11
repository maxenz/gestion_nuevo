using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using Gestion.ViewModels;
using System.Net;
using Paramedic.Gestion.Service;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesLicenciasController : Controller
    {

        #region Properties

        IClientesLicenciaService _ClientesLicenciaService;
        ILicenciaService _LicenciaService;
        IClienteService _ClienteService;
        ISitioService _SitioService;

        #endregion

        #region Constructors

        public ClientesLicenciasController(IClientesLicenciaService ClientesLicenciaService, ILicenciaService LicenciaService, IClienteService ClienteService, ISitioService SitioService)
        {
            _ClientesLicenciaService = ClientesLicenciaService;
            _LicenciaService = LicenciaService;
            _ClienteService = ClienteService;
            _SitioService = SitioService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ClienteID, string searchName = null, int page = 1)
        {
            IEnumerable<ClientesLicencia> licencias =
                _ClientesLicenciaService
                .FindBy(x => x.ClienteId == ClienteID)
                .OrderByDescending(x => x.Licencia.Serial)
                .ToList();

            if (licencias == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesLicencias", licencias);
        }

        //public ActionResult GetModExcluidos(int ClientesLicenciaID, int ProductoID)
        //{

        //    var prodMod = db.Productos.Find(ProductoID).ProductosModulos.ToList();
        //    Producto producto = db.Productos
        //        .Include(p => p.ProductosModulos)
        //        .Where(p => p.ID == ProductoID)
        //        .Single();
        //    LlenarProductosModulosExcluidos(producto.ProductosModulos, ClientesLicenciaID, ProductoID);

        //    return PartialView("_ModulosExcluidos", prodMod);
        //}

        //public string SetModExcluidos(int ClientesLicenciaID, int ProductoID, string[] vModExc)
        //{

        //    var cliProd = db.ClientesLicenciasProductos
        //        .Where(l => l.ClientesLicenciaID == ClientesLicenciaID)
        //        .Where(l => l.ProductoID == ProductoID)
        //        .Single();

        //    UpdateProductosModulosExc(vModExc, cliProd);


        //    db.Entry(cliProd).State = EntityState.Modified;
        //    db.SaveChanges();

        //    return "ok";

        //}
      
        public ActionResult Create()
        {
            if (getLicenciasDisponibles().Count > 0)
            {
                ViewBag.Licencias = getLicenciasDisponibles();
                ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial");
                ViewBag.Sitios = _SitioService.GetAll().ToList();

                return View();
            }
            else
            {
                TempData["noMoreLicence"] = "No hay licencias disponibles para agregar";
                var id = Url.RequestContext.RouteData.Values["ClienteID"];
                return Redirect("/Clientes/Edit/" + id);
            }
        }

        [HttpPost]
        public ActionResult Create(ClientesLicencia clienteslicencia)
        {
            if (ModelState.IsValid)
            {
                _ClientesLicenciaService.Create(clienteslicencia);
                return RedirectToAction("Edit", "Clientes", new { id = clienteslicencia.ClienteId });
            }

            ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial", clienteslicencia.ClienteId);
            ViewBag.Licencias = getLicenciasDisponibles();
            ViewBag.Sites = _SitioService.GetAll().ToList();

            return View(clienteslicencia);
        }

        public ActionResult Edit(int id = 0)
        {

            ClientesLicencia clienteslicencia = _ClientesLicenciaService.GetById(id);

            //LlenarProductosAsignados(clienteslicencia);
            if (clienteslicencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().ToList(), "Id", "RazonSocial", clienteslicencia.ClienteId);
            ViewBag.Licencias = getLicenciasDisponibles(clienteslicencia.Licencia.Id);
            ViewBag.Sitios = _SitioService.GetAll().ToList();
            return View(clienteslicencia);

        }

        [HttpPost]
        public ActionResult Edit(ClientesLicencia cliLic, string[] selectedProductos)
        {
            if (cliLic == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                _ClientesLicenciaService.Update(cliLic);
            }

            //var licenciaToUpdate = db.ClientesLicencias
            //    .Where(l => l.ID == cliLic.ID)
            //    .Include(l => l.ClientesLicenciasProductos)
            //    .Single();

            //UpdateLicenciasProductos(selectedProductos, licenciaToUpdate);

            ViewBag.Sitios = _SitioService.GetAll().ToList();

            return RedirectToAction("Edit", "Clientes", new { id = cliLic.ClienteId });

        }  

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesLicencia clienteslicencia = _ClientesLicenciaService.GetById(id);
            _ClientesLicenciaService.Delete(clienteslicencia);
            return RedirectToAction("Index", routeValues: new { ClienteID = clienteslicencia.ClienteId });
        }

        #endregion

        #region Private Methods

        //private void UpdateProductosModulosExc(string[] selectedModulos, ClientesLicenciasProducto cliLicProd)
        //{
        //    var licProdMod = db.ClientesLicenciasProductos.Find(cliLicProd.ID).ClientesLicenciasProductosModulos.ToList();

        //    if (selectedModulos == null)
        //    {

        //        foreach (var lpm in licProdMod)
        //        {
        //            db.ClientesLicenciasProductosModulos.Remove(lpm);
        //        }
        //        db.SaveChanges();
        //        return;
        //    }

        //    var selectedModulosHS = new HashSet<string>(selectedModulos);
        //    var prodMod = new HashSet<int>
        //        (cliLicProd.ClientesLicenciasProductosModulos.Select(l => l.ProductosModuloID));
        //    foreach (var modulo in db.Productos.Find(cliLicProd.ProductoID).ProductosModulos)
        //    {
        //        if (selectedModulosHS.Contains(modulo.ID.ToString()))
        //        {
        //            if (!prodMod.Contains(modulo.ID))
        //            {
        //                var cliLicProdMod = new ClientesLicenciasProductosModulo();
        //                cliLicProdMod.ClientesLicenciasProductoID = cliLicProd.ID;
        //                cliLicProdMod.ProductosModuloID = modulo.ID;
        //                db.Entry(cliLicProdMod).State = EntityState.Added;
        //            }
        //        }
        //        else
        //        {

        //            if (prodMod.Contains(modulo.ID))
        //            {
        //                var cliLicProdMod = db.ClientesLicenciasProductosModulos
        //                    .Where(c => c.ClientesLicenciasProductoID == cliLicProd.ID)
        //                    .Where(c => c.ProductosModuloID == modulo.ID).FirstOrDefault();
        //                db.Entry(cliLicProdMod).State = EntityState.Deleted;

        //            }

        //        }

        //    }

        //}

        //private void LlenarProductosModulosExcluidos(IList<ProductosModulo> pMod, int clID, int prodID)
        //{
        //    var allProductosModulos = pMod;
        //    var cliLic = db.ClientesLicencias.Find(clID);
        //    var clProd = cliLic.ClientesLicenciasProductos.Where(x => x.ProductoID == prodID).FirstOrDefault();

        //    var prodModExc = new HashSet<int>(clProd.ClientesLicenciasProductosModulos.Select(p => p.ProductosModuloID));
        //    var viewModel = new List<ModulosExcluidos>();
        //    foreach (var mod in allProductosModulos)
        //    {
        //        viewModel.Add(new ModulosExcluidos
        //        {
        //            ProductoModuloID = mod.Id,
        //            Descripcion = mod.Descripcion.Length > 15 ? string.Format("{0}...", mod.Descripcion.Substring(0, 12)) : mod.Descripcion,
        //            Asignado = prodModExc.Contains(mod.Id)
        //        });
        //    }

        //    ViewBag.ProdModExc = viewModel;

        //}

        //private void UpdateLicenciasProductos(string[] selectedProductos, ClientesLicencia licenciaToUpdate)
        //{

        //    if (selectedProductos == null)
        //    {
        //        var licProd = db.ClientesLicencias.Find(licenciaToUpdate.ID).ClientesLicenciasProductos.ToList();
        //        if (licProd.Count() > 0)
        //        {
        //            foreach (var lp in licProd)
        //            {
        //                db.ClientesLicenciasProductos.Remove(lp);
        //            }
        //            db.SaveChanges();
        //        }
        //        return;
        //    }

        //    var selectedProductosHS = new HashSet<string>(selectedProductos);
        //    var licenciasProductos = new HashSet<int>
        //        (licenciaToUpdate.ClientesLicenciasProductos.Select(l => l.ProductoID));
        //    foreach (var prod in db.Productos)
        //    {
        //        if (selectedProductosHS.Contains(prod.ID.ToString()))
        //        {
        //            if (!licenciasProductos.Contains(prod.ID))
        //            {
        //                var cliLicProd = new ClientesLicenciasProducto();
        //                cliLicProd.ProductoID = prod.ID;
        //                cliLicProd.ClientesLicenciaID = licenciaToUpdate.ID;
        //                db.Entry(cliLicProd).State = EntityState.Added;
        //                // licenciaToUpdate.ClientesLicenciasProductos.Add(cliLicProd);
        //            }
        //        }
        //        else
        //        {

        //            if (licenciasProductos.Contains(prod.ID))
        //            {
        //                var cliLicProd = db.ClientesLicenciasProductos.Where(c => c.ProductoID == prod.ID).Where(c => c.ClientesLicenciaID == licenciaToUpdate.ID).FirstOrDefault();
        //                db.Entry(cliLicProd).State = EntityState.Deleted;

        //            }

        //        }

        //    }

        //}

        //private void LlenarProductosAsignados(ClientesLicencia licencia)
        //{
        //    var allProductos = db.Productos;
        //    var licenciasProductos = new HashSet<int>(licencia.ClientesLicenciasProductos.Select(p => p.ProductoID));
        //    var viewModel = new List<ProductosAsignados>();
        //    foreach (var producto in allProductos)
        //    {
        //        viewModel.Add(new ProductosAsignados
        //        {
        //            ProductoID = producto.ID,
        //            Descripcion = producto.Descripcion,
        //            Asignado = licenciasProductos.Contains(producto.ID)
        //        });
        //    }

        //    ViewBag.Productos = viewModel;
        //}

        private ICollection<Licencia> getLicenciasDisponibles(int idLic = 0)
        {
            ICollection<Licencia> lstLicEnUso = _ClientesLicenciaService.GetAll().Select(x => x.Licencia).ToList();
            ICollection<Licencia> lstLicTotales = _LicenciaService.GetAll().ToList();
            ICollection<Licencia> lstDisponibles = new List<Licencia>();

            lstDisponibles = lstLicTotales.Except(lstLicEnUso).ToList();

            if (idLic != 0)
            {
                Licencia lic = _LicenciaService.FindBy(x => x.Id == idLic).FirstOrDefault();
                lstDisponibles.Add(lic);
            }

            return lstDisponibles;
        }

        #endregion
    }
}