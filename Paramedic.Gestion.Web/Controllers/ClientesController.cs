using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using Gestion.ViewModels;
using PagedList;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesController : Controller
    {
        private GestionDb db = new GestionDb();

        private List<ClientesPrincipal> GetInfoClientes(List<Cliente> clientes, string searchName = null)
        {
            var qClientes = new List<ClientesPrincipal>();

            foreach (var cliente in clientes)
            {
                qClientes.Add(new ClientesPrincipal
                {
                    ID = cliente.ID,
                    RazonSocial = cliente.RazonSocial,
                    Email = (getContactoPrincipal(cliente).Email ?? "").ToString(),
                    Telefono = (getContactoPrincipal(cliente).Telefono ?? "").ToString(),
                    Pais = cliente.Localidad.Provincia.Pais.Descripcion,
                    Provincia = cliente.Localidad.Provincia.Descripcion,
                    Localidad = cliente.Localidad.Descripcion,
                    Gestion = getEstadoUltGestion(cliente),
                    FecUltGestion = getFechaUltGestion(cliente)
                });
            }

            if (!String.IsNullOrEmpty(searchName))
            {

                qClientes = qClientes
                            .Where(p => p.RazonSocial.ToUpper().Contains(searchName.ToUpper()) ||
                                p.Pais.ToUpper().Contains(searchName.ToUpper()) ||
                                p.Email.ToString().ToUpper().Contains(searchName.ToUpper()) ||
                                p.Provincia.ToUpper().Contains(searchName.ToUpper()) ||
                                p.Localidad.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            qClientes = qClientes.OrderBy(p => p.RazonSocial).ToList();

            return qClientes;
        }

        private String getEstadoUltGestion(Cliente cliente)
        {

            var est = cliente.ClientesGestiones
                 .OrderByDescending(c => c.Fecha)
                 .Select(c => c.Estado).FirstOrDefault();
            if (est == null)
            {
                return "";
            }

            else
            {
                return est.Descripcion;
            }

        }

        private String getFechaUltGestion(Cliente cliente)
        {

            var fec = cliente.ClientesGestiones
                .OrderByDescending(c => c.Fecha)
                .Select(c => c.Fecha).FirstOrDefault();

            return fec.ToString().Substring(0, 10);
        }

        public String ValidarLocalidad(int id = 0)
        {
            if (id != 0)
            {
                String prov = db.Localidades.Find(id).Provincia.Descripcion;
                String pais = db.Localidades.Find(id).Provincia.Pais.Descripcion;
                return prov + "&" + pais;
            }
            else
            {
                return "";
            }

        }


        public ActionResult Index(string searchName = null, int page = 1, int selTipoClientes = 1, int selDatosSegunVista = 1)
        {

            //var paises = 


            var allClientes = GetClientesSegunEstado(selTipoClientes);

            var qClientes = GetInfoClientes(allClientes, searchName);

            if (Request.IsAjaxRequest())
            {
                if (selDatosSegunVista.Equals(1))
                {
                    return PartialView("_Clientes", qClientes.ToPagedList(page, 12));
                }
                else
                {
                    return PartialView("_ClientesGestion", qClientes.ToPagedList(page, 12));
                }

            }

            return View(qClientes.ToPagedList(page, 12));

        }

        private void getLocalidades()
        {

            List<Localidad> localidades = new List<Localidad>();

            localidades = db.Localidades.OrderBy(p => p.Descripcion).ToList();

            ViewBag.Localidades = localidades;

        }

        private void getMediosDifusion()
        {
            List<MedioDifusion> mediosDifusion = new List<MedioDifusion>();

            mediosDifusion = db.MediosDifusion.OrderBy(p => p.Descripcion).ToList();

            ViewBag.MediosDifusion = mediosDifusion;

        }

        private void getRevendedores()
        {

            List<Revendedor> revendedores = new List<Revendedor>();

            revendedores = db.Revendedores.ToList();

            ViewBag.Revendedores = revendedores;

        }

        private ClientesContacto getContactoPrincipal(Cliente cliente)
        {
            return cliente.ClientesContactos.Where(c => c.flgPrincipal == 1).FirstOrDefault();
        }

        public List<Cliente> GetClientesSegunEstado(int tipoCliente)
        {

            var clientes = new List<Cliente>();

            switch (tipoCliente)
            {
                case 1:
                    //Todos
                    clientes = db.Clientes.ToList();
                    break;
                case 2:
                    //Vendidos
                    clientes = db.Clientes.Where(c => c.ClientesLicencias.Count > 0).ToList();
                    break;
                case 3:
                    //En Gestión
                    clientes = db.Clientes
                                    .Where(c => c.ClientesLicencias.Count == 0)
                                    .Where(c => c.ClientesGestiones.Count > 0).ToList();
                    break;
            }

            return clientes;

        }


        //
        // GET: /Clientes/Details/5

        public ActionResult Details(int id = 0)
        {
            Cliente cliente = db.Clientes.Find(id);

            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        //
        // GET: /Clientes/Create

        public ActionResult Create()
        {
            getLocalidades();
            getRevendedores();
            getMediosDifusion();

            // GetContactoPrincipal();

            return View();
        }

        private Cliente validarGeoreferenciacion(Cliente cli)
        {
            string altura = cli.Altura;
            string calle = cli.Calle;
            string localidad = db.Localidades.Find(cli.LocalidadID).Descripcion;
            string provincia = db.Localidades.Find(cli.LocalidadID).Provincia.Descripcion;
            string pais = db.Localidades.Find(cli.LocalidadID).Provincia.Pais.Descripcion;
            string address = altura + " " + calle + "," + localidad + "," + provincia + "," + pais;
            string latLong = getGeofString(address);
            string[] vPos = latLong.Split('&');
            string lat = vPos[0];
            string lng = vPos[1];
            cli.Latitud = lat;
            cli.Longitud = lng;
            return cli;
        }

        private string getGeofString(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");
            string lat = locationElement.Element("lat").Value.ToString();
            string lng = locationElement.Element("lng").Value.ToString();
            string latLong = lat + "&" + lng;
            return latLong;
        }

        [HttpPost]
        public ActionResult Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                cliente = validarGeoreferenciacion(cliente);
                cliente.ClientesContactos[0].flgPrincipal = 1;
                db.Clientes.Add(cliente);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            getLocalidades();
            getRevendedores();
            getMediosDifusion();
            return View(cliente);
        }

        public ActionResult Edit(int id = 0)
        {
            getLocalidades();
            getRevendedores();
            getMediosDifusion();
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }

            ViewBag.Contactos = cliente.ClientesContactos.ToList();

            return View(cliente);
        }

        //
        // POST: /Clientes/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cli)
        {
            if (ModelState.IsValid)
            {
                cli = validarGeoreferenciacion(cli);
                db.Entry(cli).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            getLocalidades();
            getRevendedores();
            getMediosDifusion();
            return View(cli);
        }

        //
        // GET: /Clientes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        //
        // POST: /Clientes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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