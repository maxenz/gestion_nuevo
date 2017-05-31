using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using System.Collections.Generic;
using Paramedic.Gestion.Model;
using LinqKit;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class TicketsClasificacionesController : Controller
    {
        #region Properties

        ITicketsClasificacionService _TicketsClasificacionService;
        ITicketsClasificacionUsuarioService _TicketsClasificacionUsuariosService;
        IUserProfileService _UserProfileService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public TicketsClasificacionesController(ITicketsClasificacionService TicketsClasificacionService, IUserProfileService UserProfileService, ITicketsClasificacionUsuarioService TicketsClasificacionUsuarioService)
        {
            _TicketsClasificacionService = TicketsClasificacionService;
            _UserProfileService = UserProfileService;
            _TicketsClasificacionUsuariosService = TicketsClasificacionUsuarioService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            var predicate = PredicateBuilder.New<Model.TicketsClasificacion>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Descripcion.Contains(searchName)) : null;

            IEnumerable<Model.TicketsClasificacion> clasificaciones =
                _TicketsClasificacionService.FindByPage(predicate, "Descripcion ASC", controllersPageSize, page);
            int count = _TicketsClasificacionService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<Model.TicketsClasificacion>(clasificaciones, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Clasificaciones", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            SetDropdownUsers();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Model.TicketsClasificacion clasificacion, int[] usersList)
        {
            if (ModelState.IsValid)
            {
                _TicketsClasificacionService.Create(clasificacion);
                CreateUsers(usersList, clasificacion.Id);
                return RedirectToAction("Index");
            }

            SetDropdownUsers(usersList);
            return View(clasificacion);
        }

        public ActionResult Edit(int id = 0)
        {
			Model.TicketsClasificacion clasificacion = _TicketsClasificacionService.FindBy(x => x.Id == id).FirstOrDefault();

            if (clasificacion == null)
            {
                return HttpNotFound();
            }

            SetDropdownUsers(GetSelectedUsers(clasificacion.Id));
            return View(clasificacion);
        }

        [HttpPost]
        public ActionResult Edit(Model.TicketsClasificacion clasificacion, int[] usersList)
        {
            if (ModelState.IsValid)
            {
                _TicketsClasificacionService.Update(clasificacion);
                DeleteAllUsers(clasificacion.Id);
                CreateUsers(usersList, clasificacion.Id);
                return RedirectToAction("Index");
            }

            SetDropdownUsers(usersList);
            return View(clasificacion);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
			Model.TicketsClasificacion clasificacion = _TicketsClasificacionService.FindBy(x => x.Id == id).FirstOrDefault();
            DeleteAllUsers(id);
            _TicketsClasificacionService.Delete(clasificacion);
            return RedirectToAction("Index");
        }

        #endregion

        #region Private Methods

        private void SetDropdownUsers(int[] selected = null)
        {
            IList<UserProfile> users = _UserProfileService.GetAll().OrderBy(x => x.UserName).ToList();
            ViewBag.Users = new MultiSelectList(users, "Id", "UserName", selected);
        }

        private int[] GetSelectedUsers(int tcId)
        {
            IEnumerable<TicketsClasificacionUsuario> tcu = _TicketsClasificacionUsuariosService
                .FindBy(x => x.TicketsClasificacionId == tcId);

            return tcu.Select(x => x.UserProfileId).ToArray();
        }

        private void DeleteAllUsers(int tcId)
        {
            IEnumerable<TicketsClasificacionUsuario> tcu = _TicketsClasificacionUsuariosService.FindBy(x => x.TicketsClasificacionId == tcId);
            if (tcu != null)
            {
                foreach (var t in tcu.ToList())
                {
                    _TicketsClasificacionUsuariosService.Delete(t);
                }
            }
        }

        private void CreateUsers(int[] usersList, int tcId)
        {
            if (usersList != null)
            {
                foreach (int usrId in usersList)
                {
                    _TicketsClasificacionUsuariosService.Create(new TicketsClasificacionUsuario(usrId, tcId));
                }
            }
        }

        #endregion
    }
}