using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesGestionesController : Controller
    {

        #region Properties

        IClienteService _ClienteService;
        IClientesGestionService _ClientesGestionService;
        IEstadoService _EstadoService;
        private int controllersPageSize = 12;

        #endregion

        #region Constructors

        public ClientesGestionesController(IClienteService ClienteService, IClientesGestionService ClientesGestionService, IEstadoService EstadoService)
        {
            _ClienteService = ClienteService;
            _ClientesGestionService = ClientesGestionService;
            _EstadoService = EstadoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ClienteID, string searchName = null, int page = 1)
        {

            Cliente cliente = _ClienteService.FindBy(x => x.Id == ClienteID).FirstOrDefault();

            IEnumerable<ClientesGestion> gestiones = cliente.ClientesGestiones.OrderByDescending(x => x.Fecha);

            if (gestiones == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cliente_ID = cliente.Id;

            return PartialView("_ClientesGestiones", gestiones);

        }

        public ActionResult Create()
        {
            ViewBag.ClienteID = RouteData.Values["ClienteID"];
            ViewBag.Estados = _EstadoService.GetAll();
            return View();
        }

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

                    _ClientesGestionService.Create(clientesgestion);

                    return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.Cliente.Id });
                }

                ViewBag.ClienteID = clientesgestion.Cliente.Id;
                return RedirectToAction("Create");
            }
            catch
            {

                return RedirectToAction("Create");
            }

        }

        public FileResult PDFDisplay(int id)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            return File(gestion.PdfGestion, "application/pdf");
        }

        public ActionResult Edit(int id = 0)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            if (gestion == null)
            {
                return HttpNotFound();
            }
            ViewBag.Estados = _EstadoService.GetAll();
            return View(gestion);
        }

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

                _ClientesGestionService.Update(clientesgestion);

                return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.Cliente.Id });
            }

            ViewBag.Estados = _EstadoService.GetAll();

            return View(clientesgestion);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClientesGestionService.Delete(gestion);
            return RedirectToAction("Index", routeValues: new { ClienteID = gestion.Cliente.Id });
        }

        #endregion

    }
}