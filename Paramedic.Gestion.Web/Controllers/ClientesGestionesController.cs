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

            IEnumerable<ClientesGestion> gestiones =
                _ClientesGestionService
                .FindBy(x => x.ClienteId == ClienteID)
                .OrderByDescending(x => x.Fecha)
                .ToList();

            if (gestiones == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                gestiones = gestiones.Where(x => x.FullDescription.Contains(searchName)).ToList();
            }

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesGestiones", gestiones);

        }

        public ActionResult Create(int clienteId)
        {
            ViewBag.ClienteId = clienteId;
            ViewBag.Estados = _EstadoService.GetAll();
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientesGestion clientesgestion, HttpPostedFileBase pdfDoc)
        {
            ViewBag.ClienteID = clientesgestion.ClienteId;
            try
            {
                if (clientesgestion.FechaRecontacto < DateTime.Now)
                {
                    clientesgestion.FechaRecontacto = DateTime.MinValue;
                }

                if ((clientesgestion.Fecha != DateTime.MinValue) && (!string.IsNullOrEmpty(clientesgestion.Observaciones)))
                {
                    if (pdfDoc != null)
                    {
                        clientesgestion.PdfGestion = new byte[pdfDoc.ContentLength];
                        pdfDoc.InputStream.Read(clientesgestion.PdfGestion, 0, pdfDoc.ContentLength);
                    }

                    _ClientesGestionService.Create(clientesgestion);

                    return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.ClienteId });
                }
            }
            catch
            {

                return RedirectToAction("Create");
            }

            return RedirectToAction("Create");

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
                    clientesgestion.FechaRecontacto = DateTime.MinValue;
                }

                if (pdfDoc != null)
                {
                    clientesgestion.PdfGestion = new byte[pdfDoc.ContentLength];
                    pdfDoc.InputStream.Read(clientesgestion.PdfGestion, 0, pdfDoc.ContentLength);
                }

                _ClientesGestionService.Update(clientesgestion);

                return RedirectToAction("Edit", "Clientes", new { id = clientesgestion.ClienteId });
            }

            ViewBag.Estados = _EstadoService.GetAll();

            return View(clientesgestion);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClientesGestionService.Delete(gestion);
            return RedirectToAction("Index", routeValues: new { ClienteID = gestion.ClienteId});
        }

        #endregion

    }
}