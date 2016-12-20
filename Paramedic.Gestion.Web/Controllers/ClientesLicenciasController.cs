using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using Gestion.ViewModels;
using System.Net;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesLicenciasController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ClientesLicencias/

        public ActionResult Index(int ClienteID, String searchName = null, int page = 1)
        {

            var qLicencias = from l in db.ClientesLicencias where l.ClienteID == ClienteID select l;

            if (qLicencias == null)
            {
                return HttpNotFound();
            }

            qLicencias = qLicencias.OrderBy(p => p.Licencia.Serial);

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesLicencias", qLicencias.ToList());

        }

        //
        // GET: /ClientesLicencias/Details/5

        public ActionResult GetModExcluidos(int ClientesLicenciaID, int ProductoID)
        {

            var prodMod = db.Productos.Find(ProductoID).ProductosModulos.ToList();
            Producto producto = db.Productos
                .Include(p => p.ProductosModulos)
                .Where(p => p.ID == ProductoID)
                .Single();
            LlenarProductosModulosExcluidos(producto.ProductosModulos, ClientesLicenciaID, ProductoID);

            return PartialView("_ModulosExcluidos", prodMod);
        }

        public string SetModExcluidos(int ClientesLicenciaID, int ProductoID, string[] vModExc)
        {

            var cliProd = db.ClientesLicenciasProductos
                .Where(l => l.ClientesLicenciaID == ClientesLicenciaID)
                .Where(l => l.ProductoID == ProductoID)
                .Single();

            UpdateProductosModulosExc(vModExc, cliProd);


            db.Entry(cliProd).State = EntityState.Modified;
            db.SaveChanges();

            return "ok";

        }

        private void UpdateProductosModulosExc(string[] selectedModulos, ClientesLicenciasProducto cliLicProd)
        {
            var licProdMod = db.ClientesLicenciasProductos.Find(cliLicProd.ID).ClientesLicenciasProductosModulos.ToList();

            if (selectedModulos == null)
            {

                foreach (var lpm in licProdMod)
                {
                    db.ClientesLicenciasProductosModulos.Remove(lpm);
                }
                db.SaveChanges();
                return;
            }

            var selectedModulosHS = new HashSet<string>(selectedModulos);
            var prodMod = new HashSet<int>
                (cliLicProd.ClientesLicenciasProductosModulos.Select(l => l.ProductosModuloID));
            foreach (var modulo in db.Productos.Find(cliLicProd.ProductoID).ProductosModulos)
            {
                if (selectedModulosHS.Contains(modulo.ID.ToString()))
                {
                    if (!prodMod.Contains(modulo.ID))
                    {
                        var cliLicProdMod = new ClientesLicenciasProductosModulo();
                        cliLicProdMod.ClientesLicenciasProductoID = cliLicProd.ID;
                        cliLicProdMod.ProductosModuloID = modulo.ID;
                        db.Entry(cliLicProdMod).State = EntityState.Added;
                    }
                }
                else
                {

                    if (prodMod.Contains(modulo.ID))
                    {
                        var cliLicProdMod = db.ClientesLicenciasProductosModulos
                            .Where(c => c.ClientesLicenciasProductoID == cliLicProd.ID)
                            .Where(c => c.ProductosModuloID == modulo.ID).FirstOrDefault();
                        db.Entry(cliLicProdMod).State = EntityState.Deleted;

                    }

                }

            }

        }

        private void LlenarProductosModulosExcluidos(IList<ProductosModulo> pMod, int clID, int prodID)
        {
            var allProductosModulos = pMod;
            var cliLic = db.ClientesLicencias.Find(clID);
            var clProd = cliLic.ClientesLicenciasProductos.Where(x => x.ProductoID == prodID).FirstOrDefault();

            var prodModExc = new HashSet<int>(clProd.ClientesLicenciasProductosModulos.Select(p => p.ProductosModuloID));
            var viewModel = new List<ModulosExcluidos>();
            foreach (var mod in allProductosModulos)
            {
                viewModel.Add(new ModulosExcluidos
                {
                    ProductoModuloID = mod.ID,
                    Descripcion = setModuloDescripcion(mod.Descripcion),
                    Asignado = prodModExc.Contains(mod.ID)
                });
            }

            ViewBag.ProdModExc = viewModel;

        }

        //
        // GET: /ClientesLicencias/Create

        private string setModuloDescripcion(string desc)
        {
            if (desc.Length > 15)
            {
                desc = desc.Substring(0, 12) + "...";
                
            }

            return desc;

        }

        public ActionResult Create()
        {
            if (getLicenciasDisponibles().Count > 0)
            {
                ViewBag.Licencias = getLicenciasDisponibles();
                ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial");
                getSites();

                return View();
            }
            else
            {
                TempData["noMoreLicence"] = "No hay licencias disponibles para agregar";
                var id = Url.RequestContext.RouteData.Values["ClienteID"];
                return Redirect("/Clientes/Edit/" + id);
            }

        }

        private List<Licencia> getLicenciasDisponibles(int idLic = 0)
        {
            
            List<Licencia> lstLicEnUso = db.ClientesLicencias.Select(x => x.Licencia).ToList();
            List<Licencia> lstLicTotales = db.Licencias.ToList();
            List<Licencia> lstDisponibles = new List<Licencia>();


            lstDisponibles = lstLicTotales.Except(lstLicEnUso).ToList();

            if (idLic != 0)
            {
                Licencia lic = db.Licencias.Find(idLic);
                lstDisponibles.Add(lic);
            }

            return lstDisponibles;


        }

        //
        // POST: /ClientesLicencias/Create

        [HttpPost]
        public ActionResult Create(ClientesLicencia clienteslicencia)
        {
            if (ModelState.IsValid)
            {
                db.ClientesLicencias.Add(clienteslicencia);
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clienteslicencia.ClienteID });
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clienteslicencia.ClienteID);
            ViewBag.Licencias = getLicenciasDisponibles();
            getSites();
            return View(clienteslicencia);
        }

        //
        // GET: /ClientesLicencias/Edit/5

        public ActionResult Edit(int id = 0)
        {

            ClientesLicencia clienteslicencia = db.ClientesLicencias
                .Include(p => p.ClientesLicenciasProductos)
                .Where(p => p.ID == id)
                .Single();
            LlenarProductosAsignados(clienteslicencia);
            if (clienteslicencia == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clienteslicencia.ClienteID);
            ViewBag.Licencias = getLicenciasDisponibles(clienteslicencia.Licencia.ID);
            getSites();
            return View(clienteslicencia);

        }

        private void LlenarProductosAsignados(ClientesLicencia licencia)
        {
            var allProductos = db.Productos;
            var licenciasProductos = new HashSet<int>(licencia.ClientesLicenciasProductos.Select(p => p.ProductoID));
            var viewModel = new List<ProductosAsignados>();
            foreach (var producto in allProductos)
            {
                viewModel.Add(new ProductosAsignados
                {
                    ProductoID = producto.ID,
                    Descripcion = producto.Descripcion,
                    Asignado = licenciasProductos.Contains(producto.ID)
                });
            }

            ViewBag.Productos = viewModel;
        }

        //
        // POST: /ClientesLicencias/Edit/5

        //[HttpPost]
        //public ActionResult Edit(ClientesLicencia clienteslicencia)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(clienteslicencia).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Edit", "Clientes", new { id = clienteslicencia.ClienteID });
        //    }
        //    ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clienteslicencia.ClienteID);
        //    return View(clienteslicencia);
        //}

        [HttpPost]
        public ActionResult Edit(ClientesLicencia cliLic, string[] selectedProductos)
        {
            if (cliLic == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (ModelState.IsValid)
            {
                db.Entry(cliLic).State = EntityState.Modified;
                db.SaveChanges();
            }

            var licenciaToUpdate = db.ClientesLicencias
                .Where(l => l.ID == cliLic.ID)
                .Include(l => l.ClientesLicenciasProductos)
                .Single();

            UpdateLicenciasProductos(selectedProductos, licenciaToUpdate);

            db.SaveChanges();
            getSites();

            return RedirectToAction("Edit", "Clientes", new { id = db.ClientesLicencias.Find(cliLic.ID).ClienteID });

        }

        private void UpdateLicenciasProductos(string[] selectedProductos, ClientesLicencia licenciaToUpdate)
        {

            if (selectedProductos == null)
            {
                var licProd = db.ClientesLicencias.Find(licenciaToUpdate.ID).ClientesLicenciasProductos.ToList();
                if (licProd.Count() > 0)
                {
                    foreach (var lp in licProd)
                    {
                        db.ClientesLicenciasProductos.Remove(lp);
                    }
                    db.SaveChanges();
                }
                return;
            }

            var selectedProductosHS = new HashSet<string>(selectedProductos);
            var licenciasProductos = new HashSet<int>
                (licenciaToUpdate.ClientesLicenciasProductos.Select(l => l.ProductoID));
            foreach (var prod in db.Productos)
            {
                if (selectedProductosHS.Contains(prod.ID.ToString()))
                {
                    if (!licenciasProductos.Contains(prod.ID))
                    {
                        var cliLicProd = new ClientesLicenciasProducto();
                        cliLicProd.ProductoID = prod.ID;
                        cliLicProd.ClientesLicenciaID = licenciaToUpdate.ID;
                        db.Entry(cliLicProd).State = EntityState.Added;
                       // licenciaToUpdate.ClientesLicenciasProductos.Add(cliLicProd);
                    }
                }
                else
                {

                    if (licenciasProductos.Contains(prod.ID))
                    {
                        var cliLicProd = db.ClientesLicenciasProductos.Where(c => c.ProductoID == prod.ID).Where(c => c.ClientesLicenciaID == licenciaToUpdate.ID).FirstOrDefault();
                        db.Entry(cliLicProd).State = EntityState.Deleted;

                    }

                }

            }

        }

        //
        // GET: /ClientesLicencias/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientesLicencia clienteslicencia = db.ClientesLicencias.Find(id);
            if (clienteslicencia == null)
            {
                return HttpNotFound();
            }
            return View(clienteslicencia);
        }

        //
        // POST: /ClientesLicencias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesLicencia clienteslicencia = db.ClientesLicencias.Find(id);
            var cliente_id = clienteslicencia.ClienteID;
            db.ClientesLicencias.Remove(clienteslicencia);
            db.SaveChanges();
            return RedirectToAction("Index", routeValues: new { ClienteID = cliente_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        private void getSites()
        {
            List<Sitio> sites = new List<Sitio>();

            sites = db.Sitios.ToList();

            ViewBag.Sites = sites;
        }
    }
}