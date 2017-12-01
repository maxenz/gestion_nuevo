using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Web.ViewModels;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador, ColaboradorCliente")]
    public class ClientesController : Controller
    {
        #region Properties

        IClienteService _ClienteService;
        ILocalidadService _LocalidadService;
        IMedioDifusionService _MedioDifusionService;
        IRevendedorService _RevendedorService;
        GeolocalizationService _GeolocalizationService;
        private int controllersPageSize = 12;

        #endregion

        #region Constructors

        public ClientesController(IClienteService ClienteService, ILocalidadService LocalidadService, IMedioDifusionService MedioDifusionService, IRevendedorService RevendedorService)
        {
            _ClienteService = ClienteService;
            _LocalidadService = LocalidadService;
            _MedioDifusionService = MedioDifusionService;
            _RevendedorService = RevendedorService;
            _GeolocalizationService = new GeolocalizationService();
        }

        #endregion

        #region Public Methods

        public JsonResult ValidarLocalidad(int id = 0)
        {
            if (id != 0)
            {
                Localidad localidad = _LocalidadService.FindBy(x => x.Id == id).FirstOrDefault();
                return Json( new {
                    provincia = localidad.Provincia.Descripcion,
                    pais = localidad.Provincia.Pais.Descripcion
                }, JsonRequestBehavior.AllowGet);
            }

            return null;
        }

        public ActionResult Index(string searchName = null, int page = 1, int selTipoClientes = 1, int selDatosSegunVista = 1)
        {
            ClientControllerParametersDTO queryParameters = new ClientControllerParametersDTO(searchName, controllersPageSize, page, (ClientType)selTipoClientes, selDatosSegunVista);
            IList<ClienteViewModel> clientesViewModel = new List<ClienteViewModel>();
            IEnumerable<Cliente> clientes = _ClienteService.GetClientsByType(queryParameters);

            foreach (Cliente cliente in clientes)
            {
                clientesViewModel.Add(new ClienteViewModel(cliente));
            }

            int count = _ClienteService.GetCount(_ClienteService.getPredicateByConditions(queryParameters));
            var resultAsPagedList = new StaticPagedList<ClienteViewModel>(clientesViewModel, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                if (selDatosSegunVista.Equals(1))
                {
                    return PartialView("_Clientes", resultAsPagedList);
                }
                else
                {
                    return PartialView("_ClientesGestion", resultAsPagedList);
                }

            }

            return View(resultAsPagedList);

        }

        public ActionResult Create()
        {
            setDropdowns();

            return View();
        }

        [HttpPost]
        public ActionResult Create(ClienteViewModel cliente)
        {
            if (ModelState.IsValid)
            {
                Cliente cli = cliente.ClienteViewModelToCliente(cliente);
                cli = validarGeoreferenciacion(cli);
                _ClienteService.Create(cli);

                return RedirectToAction("Index");
            }

            setDropdowns();
            return View(cliente);
        }

        public ActionResult Edit(int id = 0)
        {
            setDropdowns();
            Cliente cliente = _ClienteService.FindBy(x => x.Id == id).FirstOrDefault();
            ClienteViewModel viewModel = new ClienteViewModel(cliente);
            if (viewModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Contactos = cliente.ClientesContactos;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cli)
        {
            if (ModelState.IsValid)
            {
                cli = validarGeoreferenciacion(cli);
                _ClienteService.Update(cli);
                return RedirectToAction("Index");
            }
            setDropdowns();
            return View(cli);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = _ClienteService.FindBy(x => x.Id == id).FirstOrDefault();
            _ClienteService.Delete(cliente);
            return RedirectToAction("Index");
        }

        #endregion

        #region Private Methods

        private void setDropdowns()
        {
            ViewBag.Localidades = _LocalidadService.GetAll().OrderBy(x => x.Descripcion);
            ViewBag.MediosDifusion = _MedioDifusionService.GetAll().OrderBy(x => x.Descripcion);
            ViewBag.Revendedores = _RevendedorService.GetAll().OrderBy(x => x.Nombre);
        }

        private Cliente validarGeoreferenciacion(Cliente cli)
        {
            cli.Localidad = _LocalidadService.GetById(cli.LocalidadId);
			if (!string.IsNullOrEmpty(cli.Calle) && !string.IsNullOrEmpty(cli.Altura))
			{
				Geopoint point = _GeolocalizationService.GetLocalization(cli.GeoAddress);
				cli.Latitud = point.Latitude;
				cli.Longitud = point.Longitude;
			}           

            return cli;
        }

        #endregion
    }
}