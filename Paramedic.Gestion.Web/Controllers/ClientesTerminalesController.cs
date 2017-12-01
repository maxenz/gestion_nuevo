using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador, ColaboradorCliente")]
    public class ClientesTerminalesController : Controller
    {
        #region Properties

        IClientesTerminalService _ClientesTerminalService;
        ITipoTerminalService _TipoTerminalService;
        IClienteService _ClienteService;

        #endregion

        #region Constructors

        public ClientesTerminalesController(IClientesTerminalService ClientesTerminalService, ITipoTerminalService TipoTerminalService, IClienteService ClienteService)
        {
            _ClientesTerminalService = ClientesTerminalService;
            _TipoTerminalService = TipoTerminalService;
            _ClienteService = ClienteService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ClienteID, string searchName = null, int page = 1)
        {
            IEnumerable<ClientesTerminal> terminales =
                _ClientesTerminalService
                .FindBy(x => x.ClienteId == ClienteID)
                .OrderBy(x => x.TipoTerminal.Descripcion).ToList();

            if (terminales == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                terminales = terminales.Where(x => x.TipoTerminal.Descripcion.Contains(searchName)).ToList();
            }

            ViewBag.ClienteId = ClienteID;

            return PartialView("_ClientesTerminales", terminales);
        }

        public ActionResult Create(int clienteId)
        {
            ViewBag.ClienteId = clienteId;
            setGeneralViewData(null);
            return View();
        }
        
        [HttpPost]
        public ActionResult Create(ClientesTerminal clientesterminal)
        {
            if (ModelState.IsValid)
            {
                _ClientesTerminalService.Create(clientesterminal);                                
                return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteId });
            }

            setGeneralViewData(clientesterminal);
            return View(clientesterminal);
        }

        public ActionResult Edit(int id = 0)
        {
            ClientesTerminal clientesterminal = _ClientesTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
            if (clientesterminal == null)
            {
                return HttpNotFound();
            }

            ViewBag.ClienteId = clientesterminal.ClienteId;
            setGeneralViewData(clientesterminal);

            return View(clientesterminal);
        }

        [HttpPost]
        public ActionResult Edit(ClientesTerminal clientesterminal)
        {
            if (ModelState.IsValid)
            {
                _ClientesTerminalService.Update(clientesterminal);
                return RedirectToAction("Edit", "Clientes", new { id = clientesterminal.ClienteId });
            }

            setGeneralViewData(clientesterminal);
            return View(clientesterminal);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesTerminal clientesterminal = _ClientesTerminalService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClientesTerminalService.Delete(clientesterminal);
            return RedirectToAction("Index", routeValues: new { ClienteID = clientesterminal.ClienteId });
        }

        #endregion

        #region Private Methods

        private void setGeneralViewData(ClientesTerminal clientesterminal)
        {
            ViewBag.TipoTerminalId = new SelectList(_TipoTerminalService.GetAll(), "Id", "Descripcion", clientesterminal != null ? clientesterminal.TipoTerminalId : 0);
        }

        #endregion
    }
}