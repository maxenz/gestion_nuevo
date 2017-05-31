using System.Data;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using System.Collections.Generic;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesUsuariosController : Controller
    {
        #region Properties

        IClientesUsuarioService _ClientesUsuarioService;
        IClienteService _ClienteService;
        IUserProfileService _UserProfileService;

        #endregion

        #region Constructors

        public ClientesUsuariosController(IClientesUsuarioService ClientesUsuarioService, IClienteService ClienteService, IUserProfileService UserProfileService)
        {
            _ClientesUsuarioService = ClientesUsuarioService;
            _ClienteService = ClienteService;
            _UserProfileService = UserProfileService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(int ClienteID, string searchName = null, int page = 1)
        {
            IEnumerable<ClientesUsuario> usuarios =
                _ClientesUsuarioService
                .FindBy(x => x.ClienteId == ClienteID)
                .OrderBy(x => x.Usuario.UserName).ToList();

            if (usuarios == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                usuarios = usuarios.Where(x => x.Usuario.UserName.Contains(searchName)).ToList();
            }

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesUsuarios", usuarios);
        }

        public ActionResult Create()
        {
            setGeneralData();
            return View();
        }

        private void setGeneralData()
        {
            ViewBag.ClienteID = new SelectList(_ClienteService.GetAll().OrderBy(x => x.RazonSocial), "Id", "RazonSocial");
            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll().OrderBy(x => x.UserName), "Id", "UserName");
        }

        private void setGeneralDataSelected(ClientesUsuario clientesusuario)
        {
            ViewBag.ClienteID = new SelectList(_ClienteService.GetAll(), "Id", "RazonSocial", clientesusuario.ClienteId);
            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName", clientesusuario.UsuarioId);
        }

        [HttpPost]
        public ActionResult Create(ClientesUsuario clientesusuario)
        {
            if (ModelState.IsValid)
            {
                _ClientesUsuarioService.Create(clientesusuario);                                
                return RedirectToAction("Edit", "Clientes", new { id = clientesusuario.ClienteId });
            }

            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        public ActionResult Edit(int id = 0)
        {
            ClientesUsuario clientesusuario = _ClientesUsuarioService.FindBy(x => x.Id == id).FirstOrDefault();
            
            if (clientesusuario == null)
            {
                return HttpNotFound();
            }
            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        [HttpPost]
        public ActionResult Edit(ClientesUsuario clientesusuario)
        {
            if (ModelState.IsValid)
            {
                _ClientesUsuarioService.Update(clientesusuario);                
                return RedirectToAction("Edit", "Clientes", new { id = clientesusuario.ClienteId });
            }
            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesUsuario clientesusuario = _ClientesUsuarioService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClientesUsuarioService.Delete(clientesusuario);
            return RedirectToAction("Index", routeValues: new { ClienteID = clientesusuario.ClienteId });
        }

        #endregion
    }
}