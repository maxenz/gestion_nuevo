using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using PagedList;
using Gestion.ViewModels;
using System.Net;


namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LicenciasController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /Licencias/

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var allLicencias = db.Licencias;    
            var qLicencias = new List<LicenciasEstados>();

            foreach (var licencia in allLicencias)
            {
                qLicencias.Add(new LicenciasEstados
                {
                    ID = licencia.ID,
                    Estado = getLicenciaEstado(licencia),
                    Productos = formatLicenciaProductos(licencia.Productos),
                    Serial = licencia.Serial,
                    NumeroLlave = licencia.NumeroDeLlave
                });
            }


            if (!String.IsNullOrEmpty(searchName))
            {
                qLicencias = qLicencias.Where(p => p.Serial.ToUpper().Contains(searchName.ToUpper())).ToList();             
            }

            qLicencias = qLicencias.OrderByDescending(p => p.Serial).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Licencias", qLicencias.ToPagedList(page, 6));
            }

            return View(qLicencias.ToPagedList(page, 6));
        }

        //
        // GET: /Licencias/Details/5

        public ActionResult Details(int id = 0)
        {
            Licencia licencia = db.Licencias.Find(id);
            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        private String getLicenciaEstado(Licencia licencia)
        {
            String strEstado = "";

            var cliLic = db.ClientesLicencias.Where(cl => cl.LicenciaID == licencia.ID);

            if (cliLic.Count() > 0)
            {
                String cliente = cliLic.FirstOrDefault().Cliente.RazonSocial;
                strEstado = "ASIGNADA A ";
                strEstado = strEstado + cliente;
            }
            else
            {
                strEstado = "SIN ASIGNAR";
            }

            return strEstado;

        }

        private String formatLicenciaProductos(ICollection<Producto> productos)
        {

            String strProductos = "";

            foreach (var prod in productos)
            {
                if (strProductos == "")
                {
                    strProductos = prod.Numero.ToString();
                }
                else
                {
                    strProductos = strProductos + " / " + prod.Numero.ToString();
                }
            }

            return strProductos;

        }

        //
        // GET: /Licencias/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Licencias/Create

        [HttpPost]
        public ActionResult Create(Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                db.Licencias.Add(licencia);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(licencia);
        }

        //
        // GET: /Licencias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Licencia licencia = db.Licencias.Find(id);

            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }



        //
        // POST: /Licencias/Edit/5

        [HttpPost]
        public ActionResult Edit(Licencia licencia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(licencia).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(licencia);
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
                (licenciaToUpdate.Productos.Select(l => l.ID));
            foreach (var prod in db.Productos)
            {
                if (selectedProductosHS.Contains(prod.ID.ToString()))
                {
                    if (!licenciasProductos.Contains(prod.ID))
                    {
                        licenciaToUpdate.Productos.Add(prod);
                    }
                }
                else
                {

                    if (licenciasProductos.Contains(prod.ID))
                    {
                        licenciaToUpdate.Productos.Remove(prod);
                    }

                }

            }

        }

        //
        // GET: /Licencias/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Licencia licencia = db.Licencias.Find(id);
            if (licencia == null)
            {
                return HttpNotFound();
            }
            return View(licencia);
        }

        //
        // POST: /Licencias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Licencia licencia = db.Licencias.Find(id);
            db.Licencias.Remove(licencia);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}