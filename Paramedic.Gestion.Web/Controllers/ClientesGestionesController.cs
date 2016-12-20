using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesGestionesController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ClientesGestiones/

        public ActionResult Index(int ClienteID, String searchName = null, int page = 1)
        {

            var qGestiones = from g in db.ClientesGestiones where g.ClienteID == ClienteID select g;

            if (qGestiones == null)
            {
                return HttpNotFound();
            }

            qGestiones = qGestiones.OrderByDescending(p => p.Fecha);

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesGestiones", qGestiones.ToList());

        }


        //
        // GET: /ClientesGestiones/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientesGestion clientesgestion = db.ClientesGestiones.Find(id);
            if (clientesgestion == null)
            {
                return HttpNotFound();
            }
            return View(clientesgestion);
        }

        //
        // GET: /ClientesGestiones/Create

        public ActionResult Create()
        {
            ViewBag.ClienteID = RouteData.Values["ClienteID"];
            ViewBag.Estados = db.Estados.ToList();
            return View();
        }

        //
        // POST: /ClientesGestiones/Create

        [HttpPost]
        public ActionResult Create(ClientesGestion clientesgestion, HttpPostedFileBase pdfDoc)
        {
            try
            {
                if (clientesgestion.FechaRecontacto < DateTime.Now)
                {
                    clientesgestion.FechaRecontacto = new DateTime(1900, 1, 1);
                }

                if ((clientesgestion.Fecha != new DateTime(1, 1, 1)) && (clientesgestion.Observaciones != ""))
                {
                    if (pdfDoc != null)
                    {
                        clientesgestion.PdfGestion = new byte[pdfDoc.ContentLength];
                        pdfDoc.InputStream.Read(clientesgestion.PdfGestion, 0, pdfDoc.ContentLength);
                    }
                    db.ClientesGestiones.Add(clientesgestion);
                    db.SaveChanges();


                    return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.ClienteID });
                }

                ViewBag.ClienteID = clientesgestion.ClienteID;
                return RedirectToAction("Create");
            } catch {
                
                return RedirectToAction("Create");
            }

        }

        public FileResult PDFDisplay(int id)
        {
            byte[] fileData = db.ClientesGestiones.Find(id).PdfGestion;

            return File(fileData, "application/pdf");
        }

        //
        // GET: /ClientesGestiones/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ViewBag.Estados = db.Estados.ToList();
            ClientesGestion clientesgestion = db.ClientesGestiones.Find(id);
            if (clientesgestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estados = db.Estados.ToList();
            return View(clientesgestion);
        }

        //
        // POST: /ClientesGestiones/Edit/5

        [HttpPost]
        public ActionResult Edit(ClientesGestion clientesgestion, HttpPostedFileBase pdfDoc)
        {

            if (ModelState.IsValid)
            {
                if (clientesgestion.FechaRecontacto < DateTime.Now)
                {
                    clientesgestion.FechaRecontacto = new DateTime(1900, 1, 1);
                }

                if (pdfDoc != null)
                {
                    clientesgestion.PdfGestion = new byte[pdfDoc.ContentLength];
                    pdfDoc.InputStream.Read(clientesgestion.PdfGestion, 0, pdfDoc.ContentLength);
                }

                db.Entry(clientesgestion).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.ClienteID });
            }

            ViewBag.Estados = db.Estados.ToList();

            return View(clientesgestion);
        }

        //
        // GET: /ClientesGestiones/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientesGestion clientesgestion = db.ClientesGestiones.Find(id);
            if (clientesgestion == null)
            {
                return HttpNotFound();
            }
            return View(clientesgestion);
        }

        //
        // POST: /ClientesGestiones/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            ClientesGestion clientesgestion = db.ClientesGestiones.Find(id);
            var cliente_id = clientesgestion.ClienteID;
            db.ClientesGestiones.Remove(clientesgestion);
            db.SaveChanges();
            return RedirectToAction("Index", routeValues: new { ClienteID = cliente_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}