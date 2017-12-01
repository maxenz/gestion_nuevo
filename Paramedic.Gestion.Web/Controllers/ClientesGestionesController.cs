using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Web.ViewModels;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador, ColaboradorCliente")]
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
        public ActionResult Create(ClientesGestionViewModel vm)
        {
            ViewBag.ClienteId = vm.ClienteId;
            ViewBag.Estados = _EstadoService.GetAll();
            if (ModelState.IsValid)
            {
                try
                {
                    ClientesGestion cg = new ClientesGestion();
                    cg = vm.ClientesGestionViewModelToClientesGestion(cg);
                    _ClientesGestionService.Create(cg);

                    return RedirectToAction("Edit", "Clientes", new { id = cg.ClienteId });

                }
                catch
                {
                    return RedirectToAction("Create", "ClientesGestiones", new { clienteId = vm.ClienteId });
                }
            }

            return RedirectToAction("Create", "ClientesGestiones", new { clienteId = vm.ClienteId });

        }

        public FileResult PDFDisplay(int id)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            return File(gestion.PdfGestion, "application/pdf");
        }

        public ActionResult Edit(int id = 0)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            ClientesGestionViewModel vm = new ClientesGestionViewModel(gestion);
            ViewBag.ClienteId = gestion.ClienteId;
            if (gestion == null)
            {
                return HttpNotFound();
            }

            ViewBag.PdfContent = gestion.PdfGestion;
            ViewBag.Estados = _EstadoService.GetAll();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(ClientesGestionViewModel vm)
        {
            if (ModelState.IsValid)
            {
                ClientesGestion cg = _ClientesGestionService.GetById(vm.Id);
                cg = vm.ClientesGestionViewModelToClientesGestion(cg);
                if (vm.PdfUpload == null)
                {
                    cg.PdfGestion = cg.PdfGestion;
                }

                _ClientesGestionService.Update(cg);

                return RedirectToAction("Edit", "Clientes", new { id = vm.ClienteId });
            }

            ViewBag.Estados = _EstadoService.GetAll();

            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesGestion gestion = _ClientesGestionService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClientesGestionService.Delete(gestion);
            return RedirectToAction("Index", routeValues: new { ClienteID = gestion.ClienteId });
        }

        #endregion

    }
}