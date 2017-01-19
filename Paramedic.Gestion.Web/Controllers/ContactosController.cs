using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using Paramedic.Gestion.Model;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ContactosController : Controller
    {
        #region Properties

        IClientesContactoService _ClientesContactoService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public ContactosController(IClientesContactoService ClientesContactoService)
        {
            _ClientesContactoService = ClientesContactoService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<ClientesContacto>();
            if (!string.IsNullOrEmpty(searchName))
            {
                predicate = predicate.Or(x => x.Cliente.RazonSocial.ToUpper().Contains(searchName.ToUpper()));
                predicate = predicate.Or(x => x.Nombre.ToUpper().Contains(searchName.ToUpper()));
                predicate = predicate.Or(x => x.Email.ToUpper().Contains(searchName.ToUpper()));
            }
            else
            {
                predicate = null;
            }

            IEnumerable<ClientesContacto> contactos =
                _ClientesContactoService
                .FindByPage(predicate, "Cliente.RazonSocial ASC", controllersPageSize, page);
            int count = _ClientesContactoService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<ClientesContacto>(contactos, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Contactos", resultAsPagedList);
            }

            return View(resultAsPagedList);

        }

        #endregion
    }
}