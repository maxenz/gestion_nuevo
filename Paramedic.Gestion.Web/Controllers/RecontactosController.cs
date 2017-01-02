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
    public class RecontactosController : Controller
    {
        //
        // GET: /Recontactos/
        private GestionDb db = new GestionDb();

        public ActionResult Index(string searchName = null, int page = 1, string fechaDesde = null, string fechaHasta = null, int selTipoGestion = 3)
        {

            var margenMayor = DateTime.Now.AddDays(30);
            var hoy = DateTime.Now;
            ViewBag.dftDesde = hoy.ToShortDateString();
            ViewBag.dftHasta = margenMayor.ToShortDateString();
            IQueryable<ClientesGestion> allGestiones = db.ClientesGestiones;
            var qRecontactos = new List<Recontacto>();
        
            foreach (var gst in allGestiones)
            {
                String observ = gst.Estado.Descripcion + " - " + gst.Observaciones;
                agregarGestion(gst,qRecontactos,gst.Fecha,observ,"G");
                if (!(gst.FechaRecontacto.ToShortDateString().Equals("01-01-1900")))
                {
                    observ = gst.Observaciones;
                    agregarGestion(gst,qRecontactos,gst.FechaRecontacto,observ,"R");
                }

            }

            if (!String.IsNullOrEmpty(searchName))
            {

                qRecontactos = qRecontactos
                                .Where(p => p.Cliente.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(fechaDesde))
            {
                fechaDesde = fechaDesde + " 00:00";
                fechaHasta = fechaHasta + " 23:59";

                DateTime dtDesde = DateTime.ParseExact(fechaDesde, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                DateTime dtHasta = DateTime.ParseExact(fechaHasta, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                qRecontactos = qRecontactos.Where(a => a.Fecha >= dtDesde && a.Fecha <= dtHasta).ToList();
            }
            else
            {
                DateTime dtLocal = hoy.AddDays(-1);
                qRecontactos = qRecontactos.Where(a => a.Fecha >= dtLocal && a.Fecha <= margenMayor).ToList();
            }

            switch (selTipoGestion)
            {
                case 2:
                    qRecontactos = qRecontactos.Where(a => a.Tipo == "G").ToList();
                    break;
                case 3:
                    qRecontactos = qRecontactos.Where(a => a.Tipo == "R").ToList();
                    break;
            }


            qRecontactos = qRecontactos.OrderBy(p => p.Fecha).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Recontactos", qRecontactos.ToPagedList(page, 6));
            }

            return View(qRecontactos.ToPagedList(page, 6));
        }

        private void agregarGestion(ClientesGestion gst, List<Recontacto> lst, DateTime fecha, String observ, String tipo)
        {
            lst.Add(new Recontacto
            {
                Cliente = gst.Cliente.RazonSocial,
                ClienteID = gst.ClienteID,
                Observaciones = observ,
                Fecha = fecha,
                Tipo = tipo
            });
        }

    }
}
