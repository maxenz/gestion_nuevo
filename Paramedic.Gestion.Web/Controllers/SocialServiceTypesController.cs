using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Paramedic.Gestion.Model;
using PagedList;
using Paramedic.Gestion.Service;
using LinqKit;
using Paramedic.Gestion.Web.ViewModels;

namespace Paramedic.Gestion.Web.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class SocialServiceTypesController : Controller
    {
        #region Properties

        ISocialServiceTypesService _SocialServiceTypesService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public SocialServiceTypesController(ISocialServiceTypesService SocialServiceTypesService)
        {
            _SocialServiceTypesService = SocialServiceTypesService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {

            var predicate = PredicateBuilder.New<SocialServiceType>();
            predicate = !string.IsNullOrEmpty(searchName) ? predicate.And(x => x.Description.Contains(searchName)) : null;

            IEnumerable<SocialServiceType> socialServiceTypes =
                _SocialServiceTypesService.FindByPage(predicate, "Id ASC", controllersPageSize, page);
            int count = _SocialServiceTypesService.FindBy(predicate).Count();
            var resultAsPagedList = new StaticPagedList<SocialServiceType>(socialServiceTypes, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SocialServiceTypes", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        public ActionResult Create()
        {
            ViewBag.IsCreateForm = true;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SocialServiceTypesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _SocialServiceTypesService.Create(vm.ToSocialServiceType());
                return RedirectToAction("Index");
            }

            ViewBag.IsCreateForm = true;
            return View(vm);
        }

        public ActionResult Edit(int id = 0)
        {
            SocialServiceType socialServiceType = _SocialServiceTypesService.FindBy(x => x.Id == id).FirstOrDefault();
            SocialServiceTypesViewModel vm = new SocialServiceTypesViewModel(socialServiceType);
            if (socialServiceType == null)
            {
                return HttpNotFound();
            }
            ViewBag.IsCreateForm = false;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(SocialServiceTypesViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _SocialServiceTypesService.Update(vm.ToSocialServiceType());
                return RedirectToAction("Index");
            }
            ViewBag.IsCreateForm = false;
            return View(vm);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            SocialServiceType socialServiceType = _SocialServiceTypesService.FindBy(x => x.Id == id).FirstOrDefault();
            _SocialServiceTypesService.Delete(socialServiceType);
            return RedirectToAction("Index");
        }

        #endregion
    }
}