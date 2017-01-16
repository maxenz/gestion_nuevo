using System.Collections.Generic;
using System.Data;
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
        IProductoService _ProductoService;
        IClientesLicenciasProductoService _ClientesLicenciasProductoService;
        IClientesLicenciasProductosModuloService _ClientesLicenciasProductosModuloService;
        #endregion

        #region Constructors

        public ClientesLicenciasController(IClientesLicenciaService ClientesLicenciaService, ILicenciaService LicenciaService, IClienteService ClienteService, ISitioService SitioService, IProductoService ProductoService, IClientesLicenciasProductoService ClientesLicenciasProductoService, IClientesLicenciasProductosModuloService ClientesLicenciasProductosModuloService)
        {
            _ClientesLicenciaService = ClientesLicenciaService;
            _LicenciaService = LicenciaService;
            _ClienteService = ClienteService;
            _SitioService = SitioService;
            _ProductoService = ProductoService;
            _ClientesLicenciasProductoService = ClientesLicenciasProductoService;
            _ClientesLicenciasProductosModuloService = ClientesLicenciasProductosModuloService;
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

        public ActionResult GetModExcluidos(int clientesLicenciaId, int productoId)
        {
            IList<ProductosModulo> modulos = _ProductoService.FindBy(x => x.Id == productoId).FirstOrDefault().ProductosModulos.ToList();
            LlenarProductosModulosExcluidos(modulos, clientesLicenciaId, productoId);

            return PartialView("_ModulosExcluidos", modulos);
        }

        public string SetModExcluidos(int clientesLicenciaId, int productoId, string[] vModExc)
        {
            ClientesLicenciasProducto licenciasProducto =
                _ClientesLicenciaService
                .FindBy(x => x.Id == clientesLicenciaId)
                .FirstOrDefault()
                .ClientesLicenciasProductos
                .Where(x => x.ProductoId == productoId)
                .FirstOrDefault();

            UpdateProductosModulosExc(vModExc, licenciasProducto);

            return "ok";

        }

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
            ViewBag.Sitios = _SitioService.GetAll().ToList();

            return View(clienteslicencia);
        }

        public ActionResult Edit(int id = 0)
        {

            ClientesLicencia clienteslicencia = _ClientesLicenciaService.GetById(id);

            LlenarProductosAsignados(clienteslicencia);
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

            ClientesLicencia licenciaToUpdate = _ClientesLicenciaService.GetById(cliLic.Id);

            UpdateLicenciasProductos(selectedProductos, licenciaToUpdate);

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

        private void UpdateProductosModulosExc(string[] selectedModulos, ClientesLicenciasProducto cliLicProd)
        {
            var licProdMod = _ClientesLicenciasProductoService.FindBy(x => x.Id == cliLicProd.Id).FirstOrDefault().ClientesLicenciasProductosModulos.ToList();
            
            if (selectedModulos == null)
            {

                foreach (var lpm in licProdMod)
                {
                    _ClientesLicenciasProductosModuloService.Delete(lpm);                    
                }               
                return;
            }

            var selectedModulosHS = new HashSet<string>(selectedModulos);
            var prodMod = new HashSet<int>
                (cliLicProd.ClientesLicenciasProductosModulos.Select(l => l.ProductosModuloId));

            IList<ProductosModulo> productosModulos = _ProductoService.FindBy(x => x.Id == cliLicProd.ProductoId).FirstOrDefault().ProductosModulos.ToList();

            foreach (var modulo in productosModulos)
            {
                if (selectedModulosHS.Contains(modulo.Id.ToString()))
                {
                    if (!prodMod.Contains(modulo.Id))
                    {
                        var cliLicProdMod = new ClientesLicenciasProductosModulo();
                        cliLicProdMod.ClientesLicenciasProductoId = cliLicProd.Id;
                        cliLicProdMod.ProductosModuloId = modulo.Id;
                        _ClientesLicenciasProductosModuloService.Create(cliLicProdMod);
                    }
                }
                else
                {

                    if (prodMod.Contains(modulo.Id))
                    {
                        ClientesLicenciasProductosModulo cliLicProdMod = _ClientesLicenciasProductosModuloService
                            .FindBy(x => x.ClientesLicenciasProductoId == cliLicProd.Id && x.ProductosModuloId == modulo.Id)
                            .FirstOrDefault();

                        _ClientesLicenciasProductosModuloService.Delete(cliLicProdMod);

                    }

                }

            }

            _ClientesLicenciasProductoService.Update(cliLicProd);

        }

        private void LlenarProductosModulosExcluidos(IList<ProductosModulo> pMod, int clientesLicenciaId, int productoId)
        {
            var allProductosModulos = pMod;
            ClientesLicencia cliLic = _ClientesLicenciaService.GetById(clientesLicenciaId);
            ClientesLicenciasProducto cliLicProd = cliLic.ClientesLicenciasProductos.FirstOrDefault(x => x.ProductoId == productoId);

            var prodModExc = new HashSet<int>(cliLicProd.ClientesLicenciasProductosModulos.Select(p => p.ProductosModuloId));
            var viewModel = new List<ModulosExcluidos>();
            foreach (var mod in allProductosModulos)
            {
                viewModel.Add(new ModulosExcluidos
                {
                    ProductoModuloID = mod.Id,
                    Descripcion = mod.Descripcion.Length > 15 ? string.Format("{0}...", mod.Descripcion.Substring(0, 12)) : mod.Descripcion,
                    Asignado = prodModExc.Contains(mod.Id)
                });
            }

            ViewBag.ProdModExc = viewModel;

        }

        private void UpdateLicenciasProductos(string[] selectedProductos, ClientesLicencia licenciaToUpdate)
        {
            if (selectedProductos == null)
            {
                var licProd = _ClientesLicenciaService.GetById(licenciaToUpdate.Id).ClientesLicenciasProductos.ToList();
                
                if (licProd.Count() > 0)
                {
                    foreach (var lp in licProd)
                    {
                        _ClientesLicenciasProductoService.Delete(lp);
                    }
                }
                return;
            }

            var selectedProductosHS = new HashSet<string>(selectedProductos);
            var licenciasProductos = new HashSet<int>
                (licenciaToUpdate.ClientesLicenciasProductos.Select(l => l.ProductoId));
            foreach (var prod in _ProductoService.GetAll().ToList())
            {
                if (selectedProductosHS.Contains(prod.Id.ToString()))
                {
                    if (!licenciasProductos.Contains(prod.Id))
                    {
                        var cliLicProd = new ClientesLicenciasProducto();
                        cliLicProd.ProductoId = prod.Id;
                        cliLicProd.ClientesLicenciaId = licenciaToUpdate.Id;
                        _ClientesLicenciasProductoService.Create(cliLicProd);
                    }
                }
                else
                {
                    if (licenciasProductos.Contains(prod.Id))
                    {
                        var cliLicProd = _ClientesLicenciasProductoService.FindBy(x => x.ProductoId == prod.Id && x.ClientesLicenciaId == licenciaToUpdate.Id).FirstOrDefault();
                        _ClientesLicenciasProductoService.Delete(cliLicProd);                                                
                    }
                }
            }
        }

        private void LlenarProductosAsignados(ClientesLicencia licencia)
        {
            ICollection<Producto> allProductos = _ProductoService.GetAll().ToList();
            var licenciasProductos = new HashSet<int>(licencia.ClientesLicenciasProductos.Select(p => p.ProductoId));
            var viewModel = new List<ProductosAsignados>();
            foreach (var producto in allProductos)
            {
                viewModel.Add(new ProductosAsignados
                {
                    ProductoID = producto.Id,
                    Descripcion = producto.Descripcion,
                    Asignado = licenciasProductos.Contains(producto.Id)
                });
            }

            ViewBag.Productos = viewModel;
        }

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