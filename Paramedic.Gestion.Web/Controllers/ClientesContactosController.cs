using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesContactosController : Controller
    {
        #region Properties

        IClientesContactoService _ClientesContactoService;
        IClienteService _ClienteService;

        #endregion

        #region Constructors

        public ClientesContactosController(IClientesContactoService ClientesContactoService, IClienteService ClienteService )
        {
            _ClientesContactoService = ClientesContactoService;
            _ClienteService = ClienteService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int clienteId, string searchName = null, int page = 1)
        {
            ICollection<ClientesContacto> contactos =
                _ClientesContactoService
                .FindBy(x => x.ClienteId == clienteId)
                .OrderBy(x => x.Nombre)
                .ToList();

            if (contactos == null)
            {
                return HttpNotFound();
            }

            return PartialView("_ClientesContactos", contactos);
        }

        public ActionResult Create(int clienteId)
        {
            ViewBag.ClienteId = clienteId;
            return View();
        }

        [HttpPost]
        public ActionResult Create(ClientesContacto clientescontacto)
        {
            if ((ModelState.IsValid) & validateContacto(clientescontacto))
            {
                _ClientesContactoService.Create(clientescontacto);

                return RedirectToAction("Edit", "Clientes", new { id = clientescontacto.ClienteId });
            }

            return View(clientescontacto);
        }

        private bool validateContacto(ClientesContacto clientescontacto)
        {

            bool hasMail = clientescontacto.Email != null;
            bool hasOther = clientescontacto.Otros != null;
            bool hasTelephone = clientescontacto.Telefono != null;

            return hasMail || hasOther || hasTelephone;

        }

        public ActionResult Edit(int id = 0)
        {
            ClientesContacto clientescontacto = _ClientesContactoService.FindBy(x => x.Id == id).FirstOrDefault();
            if (clientescontacto == null)
            {
                return HttpNotFound();
            }

            return View(clientescontacto);
        }

        [HttpPost]
        public ActionResult Edit(ClientesContacto clientescontacto)
        {
            if (ModelState.IsValid & validateContacto(clientescontacto))
            {
                _ClientesContactoService.Update(clientescontacto);
                return RedirectToAction("Edit", "Clientes", new { id = clientescontacto.ClienteId});
            }
            return View(clientescontacto);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesContacto clientescontacto = _ClientesContactoService.FindBy(x => x.Id == id).FirstOrDefault();
            Cliente cliente = _ClienteService.FindBy(x => x.Id == clientescontacto.ClienteId).FirstOrDefault();

            if (cliente.ClientesContactos.Count > 1)
            {
                if (clientescontacto.flgPrincipal == 0)
                {
                    _ClientesContactoService.Delete(clientescontacto);
                }
            }

            return RedirectToAction("Index", new { ClienteID = cliente.Id });

        }

        #endregion
    }
}