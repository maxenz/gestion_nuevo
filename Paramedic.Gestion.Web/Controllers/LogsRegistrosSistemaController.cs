using Gestion.Models;
using Gestion.ViewModels;
using LinqKit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Gestion.Controllers
{

    public class LogsRegistrosSistemaController : Controller
    {
        private GestionDb db = new GestionDb();

        [Authorize(Roles = "Administrador")]
        public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null)
        {

            var margenMenor = DateTime.Now.AddDays(-3);
            var hoy = DateTime.Now;
            ViewBag.dftDesde = margenMenor.ToShortDateString();
            ViewBag.dftHasta = hoy.ToShortDateString();
            List<LogRegistroSistema> allLogs = db.LogsRegistroSistema.ToList();

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                fechaDesde = fechaDesde + " 00:00";
                fechaHasta = fechaHasta + " 23:59";

                DateTime dtDesde = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime dtHasta = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                allLogs = allLogs.Where(a => a.Fecha >= dtDesde && a.Fecha <= dtHasta).ToList();
            }
            else
            {
                allLogs = db.LogsRegistroSistema.Where(a => a.Fecha >= margenMenor && a.Fecha <= hoy).ToList();
            }

            if (!String.IsNullOrEmpty(searchName))
            {

                allLogs = allLogs.Where(p => p.DescripcionAccion.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            allLogs = allLogs.OrderByDescending(p => p.Fecha).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Logs", allLogs.ToPagedList(page, 6));
            }

            return View(allLogs.ToPagedList(page, 6));

        }

        [Authorize(Roles = "Cliente")]
        [HttpPost]
        public int SetVideoLog(int idVideo)
        {
            try
            {
                Video video = db.Videos.Find(idVideo);
                int userID = db.UserProfiles.Where(x => x.UserName == User.Identity.Name).Select(x => x.UserId).FirstOrDefault();
                LogRegistroSistema log = new LogRegistroSistema(String.Format("VISTA DE VIDEO: {0}, ID: {1}", video.Descripcion, idVideo), userID);
                db.LogsRegistroSistema.Add(log);
                db.SaveChanges();
                return 1;
            }
            catch
            {
                return 0;
            }

        }

    }
}
