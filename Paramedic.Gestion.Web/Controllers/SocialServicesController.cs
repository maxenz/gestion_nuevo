using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using Paramedic.Gestion.Web.ViewModels;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SocialServicesController : Controller
    {
        #region Properties

        ISocialServicesService _SocialServicesService;
        ISocialServiceTypesService _SocialServiceTypesService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public SocialServicesController(
            ISocialServicesService SocialServicesService,
            ISocialServiceTypesService SocialServiceTypesService
            )
        {
            _SocialServicesService = SocialServicesService;
            _SocialServiceTypesService = SocialServiceTypesService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<SocialService>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Description.Contains(searchName)) : null;

            IEnumerable<SocialService> socialServices =
                _SocialServicesService.FindByPage(predicate, "Id ASC", controllersPageSize, page);
            int count = _SocialServicesService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<SocialService>(socialServices, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SocialServices", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            ViewBag.IsCreateForm = true;
            setDropdowns();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SocialServicesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _SocialServicesService.Create(vm.ToSocialService());
                return RedirectToAction("Index");
            }

            ViewBag.IsCreateForm = true;
            setDropdowns();
            return View(vm);
        }

        public ActionResult Edit(int id = 0)
        {
            SocialService ss = _SocialServicesService.GetById(id);
            if (ss == null)
            {
                return HttpNotFound();
            }

            ViewBag.IsCreateForm = false;
            setDropdowns();

            SocialServicesViewModel vm = null;
            switch (ss.SocialServiceType.SocialMediaType)
            {
                case Paramedic.Gestion.Model.Enums.SocialMediaTypes.Mail:
                    vm = new MailSocialServiceViewModel(ss);
                    return View("MailServiceEdit",vm);
            }

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(SocialServicesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _SocialServicesService.Update(vm.ToSocialService());
                return RedirectToAction("Index");
            }

            ViewBag.IsCreateForm = false;
            setDropdowns();
            return View(vm);
        }

        [HttpPost]
        public ActionResult EditMailSocialService(MailSocialServiceViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _SocialServicesService.Update(vm.ToSocialService());
                return RedirectToAction("Index");
            }

            ViewBag.IsCreateForm = false;
            setDropdowns();
            return View("MailServiceEdit", vm);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SocialService socialServiceType = _SocialServicesService.GetById(id);
            _SocialServicesService.Delete(socialServiceType);
            return RedirectToAction("Index");
        }

        #endregion

        #region Private Methods

        private void setDropdowns()
        {
            IEnumerable<SocialServiceType> sst =
                _SocialServiceTypesService.GetAll().Where(x => x.Enabled).OrderBy(x => x.Description);

            ViewBag.SocialServiceTypes = new SelectList(sst, "Id", "Description");
        }

        #endregion
    }
}