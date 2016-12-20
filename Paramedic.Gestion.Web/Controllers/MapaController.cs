using Gestion.Models;
using Gestion.ViewModels;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]

    public class MapaController : Controller
    {
        private GestionDb db = new GestionDb();
        //
        // GET: /Mapa/

        public ActionResult Index()
        {
            ViewBag.MediosDifusion = db.MediosDifusion.ToList();
            return View();
        }

        private IQueryable<Cliente> SearchClients(params int[] vMediosDifusion)
        {

            var predicate = PredicateBuilder.New<Cliente>();

            foreach (int item in vMediosDifusion)
            {
                int temp = item;
                predicate = predicate.Or(p => p.MedioDifusionID == temp);
            }
            return db.Clientes.AsExpandable().Where(predicate);
        }

        [AllowAnonymous]
        public string GetPositionsOfClients(int[] vData)
        {

            if (vData != null)
            {
                IQueryable<Cliente> clientes = SearchClients(vData);

                //switch (tipoCliente)
                //{
                //    case 1:
                //        //Todos
                //        clientes = db.Clientes.ToList();
                //        break;
                //    case 2:
                //        //Vendidos
                //        clientes = db.Clientes.Where(c => c.ClientesLicencias.Count > 0).ToList();
                //        break;
                //    case 3:
                //        //En Gestión
                //        clientes = db.Clientes
                //                        .Where(c => c.ClientesLicencias.Count == 0)
                //                        .Where(c => c.ClientesGestiones.Count > 0).ToList();
                //        break;
                //}

                List<GeoPositions> geoPos = new List<GeoPositions>();

                foreach (var cli in clientes)
                {
                    string latitud = validateNotRepeatedLatitude(geoPos, cli.Latitud);

                    geoPos.Add(new GeoPositions
                    {
                        ID = cli.ID,
                        Latitud = latitud,
                        Longitud = cli.Longitud,
                        Cliente = cli.RazonSocial,
                        Localidad = cli.Localidad.Descripcion,
                        EmailPrincipal = getEmail(cli),
                        Telefono = getTelefono(cli),
                        SitioWeb = cli.SitioWeb,
                        EstadoCliente = getEstadoCliente(cli),
                        MedioDifusionID = cli.MedioDifusionID,
                        VersionShaman = getVersionShaman(cli)
                    });
                }

                string json = JsonConvert.SerializeObject(geoPos.OrderBy(x => x.Latitud));

                return json;
            }
            else
            {
                return JsonConvert.SerializeObject(null);
            }


        }

        private string getVersionShaman(Cliente cli)
        {
            string producto = "";
            if (getEstadoCliente(cli) == 2)
            {
                var prods = cli.ClientesLicencias.FirstOrDefault().Licencia.Productos;
                try
                {
                    producto = prods.Where(x => (x.Numero == 1 || x.Numero == 500)).FirstOrDefault().Descripcion;
                }
                catch
                {
                    producto = "";
                }
            }
            
            return producto;

        }

        private string validateNotRepeatedLatitude(List<GeoPositions> lst, string lat)
        {
            foreach (var pos in lst)
            {
                if (pos.Latitud == lat)
                {
                    int lengthLat = lat.Length;
                    int lastFourDigits = Convert.ToInt32(lat.Substring(lengthLat - 6, 6));
                    lastFourDigits = lastFourDigits + 400000;
                    string strLastFourDigits = lastFourDigits.ToString();
                    string firstPartLat = lat.Substring(0, lengthLat - 6);
                    return firstPartLat + strLastFourDigits;
                    //double dblLat = Convert.ToDouble(lat);
                    //lat = lat + 2000;
                    //string strLat = Convert.ToString(dblLat);
                    //return strLat;

                }
            }
            return lat;
        }

        private string getEmail(Cliente cli)
        {
            string email = cli.ClientesContactos
                                   //.Where(x => x.flgPrincipal == 1)
                                   .Where(x => x.esInstitucional == true)
                                   .Select(x => x.Email).FirstOrDefault();
            if (email != null)
            {
                return email;
            }
            else
            {
                return "Sin información";
            }
        }

        private string getTelefono(Cliente cli)
        {

            string telefono = cli.ClientesContactos
                                                    //.Where(x => x.flgPrincipal == 1)
                                                    .Where(x => x.esInstitucional == true)
                                                    .Select(x => x.Telefono).FirstOrDefault();
            if (telefono != null)
            {
                return telefono;
            }
            else
            {
                return "Sin información";
            }
        }

        private int getEstadoCliente(Cliente cli)
        {

            if (cli.ClientesLicencias.Count > 0)
            {
                return 2;
            }
            else
            {
                return 3;
            }

        }

        //
        // GET: /Mapa/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Mapa/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Mapa/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Mapa/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Mapa/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Mapa/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Mapa/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
