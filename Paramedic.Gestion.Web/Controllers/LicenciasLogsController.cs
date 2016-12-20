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
using System.Globalization;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class LicenciasLogsController : Controller
    {

        private GestionDb db = new GestionDb();

        public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null, string chkAndroidLogs = null)
        {

            try
            {
                var margenMenor = DateTime.Now.AddDays(-3);
                var hoy = DateTime.Now;
                ViewBag.dftDesde = margenMenor.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                ViewBag.dftHasta = hoy.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                IEnumerable<LicenciasLog> allLogs = null;
                List<LogPrincipal> qLogs = new List<LogPrincipal>();

                if (!String.IsNullOrEmpty(fechaDesde))
                {
                    fechaDesde = fechaDesde + " 00:00";
                    fechaHasta = fechaHasta + " 23:59";

                    DateTime dtDesde = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    DateTime dtHasta = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    allLogs = db.LicenciasLogs.Where(a => a.CreatedAt >= dtDesde && a.CreatedAt <= dtHasta).OrderByDescending(x => x.CreatedAt);
                }
                else
                {
                    allLogs = db.LicenciasLogs.Where(a => a.CreatedAt >= margenMenor && a.CreatedAt <= hoy).OrderByDescending(x => x.CreatedAt);
                }

                if (String.IsNullOrEmpty(chkAndroidLogs))
                {
                    allLogs = allLogs.Where(x => x.SolicitudID != 3).ToList();
                }

                foreach (var log in allLogs)
                {
                    ClientesLicencia cliLic = getClienteLicencia(log.LicenciaID);

                    if (cliLic != null)
                    {
                        qLogs.Add(new LogPrincipal
                        {
                            Cliente = cliLic.Cliente != null ? cliLic.Cliente.RazonSocial : "Cliente sin razon social",
                            ClienteID = cliLic.Cliente != null ? cliLic.Cliente.ID : 0,
                            Serial = cliLic.Licencia != null ? cliLic.Licencia.Serial : "Cliente sin serial",
                            FechaHora = log.CreatedAt,
                            IP = log.IP,
                            Referencia = log.Referencias
                        });
                    }
                    else
                    {
                        string description = log.GenericDescription;
                        
                        if (log.SolicitudID == 3)
                        {
                            description = string.Format("Android connection: {0}", log.GenericDescription);
                        }

                        qLogs.Add(new LogPrincipal
                        {
                            Cliente = "No es cliente",
                            ClienteID = 0,
                            Serial = "",
                            FechaHora = log.CreatedAt,
                            IP = log.IP,
                            Referencia = description
                        });
                    }


                }

                if (!String.IsNullOrEmpty(searchName))
                {

                    qLogs = qLogs.Where(p => p.Cliente.ToUpper().Contains(searchName.ToUpper()) ||
                                        p.Serial.ToUpper().Contains(searchName.ToUpper()) ||
                                        p.IP.ToUpper().Contains(searchName.ToUpper()) ||
                                        p.Referencia.ToUpper().Contains(searchName.ToUpper())).ToList();
                }

                if (Request.IsAjaxRequest())
                {
                    return PartialView("_Logs", qLogs.ToPagedList(page, 6));
                }

                return View(qLogs.ToPagedList(page, 6));
            }
            catch (Exception exception)
            {
                var ex = exception.Message;
                return PartialView("_Logs", null);
            }



        }

        //
        // GET: /LicenciasLogs/Details/5

        private int getCliID(int id)
        {
            var cliLic = db.ClientesLicencias
                            .Where(c => c.LicenciaID == id)
                            .FirstOrDefault();

            if (cliLic == null)
            {
                return 0;
            }
            else
            {
                return cliLic.Cliente.ID;
            }
        }

        private ClientesLicencia getClienteLicencia(int id)
        {

            return db.ClientesLicencias.FirstOrDefault(x => x.LicenciaID == id);

        }

        private String getSerial(int id)
        {
            if (id == 0)
            {
                return "";
            }
            else
            {
                return db.Licencias.Find(id).Serial.ToString();
            }
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /LicenciasLogs/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /LicenciasLogs/Create

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
        // GET: /LicenciasLogs/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /LicenciasLogs/Edit/5

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
        // GET: /LicenciasLogs/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /LicenciasLogs/Delete/5

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
