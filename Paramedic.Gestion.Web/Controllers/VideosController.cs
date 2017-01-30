using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Web.ViewModels;

namespace Gestion.Controllers
{
    [Authorize]
    public class VideosController : Controller
    {
        #region Properties

        IVideoService _VideoService;
        IClienteService _ClienteService;
        IUserProfileService _UserProfileService;
        IClientesUsuarioService _ClientesUsuarioService;
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public VideosController(IVideoService VideoService, IClienteService ClienteService, IUserProfileService UserProfileService, IClientesUsuarioService ClientesUsuarioService)
        {
            _VideoService = VideoService;
            _ClienteService = ClienteService;
            _UserProfileService = UserProfileService;
            _ClientesUsuarioService = ClientesUsuarioService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, int page = 1)
        {
            int currentUserId = _UserProfileService.GetCurrentUserId(User.Identity.Name);
            ICollection<Video> videos = new List<Video>();

            if (User.IsInRole("Administrador"))
            {
                videos = _VideoService.GetAll().ToList();
            }
            else
            {
                Cliente cliente =
                    _ClientesUsuarioService
                    .FindBy(x => x.Id == currentUserId)
                    .Select(x => x.Cliente).FirstOrDefault();

                foreach (Video video in _VideoService.GetAll())
                {
                    if (video.EsPublico || video.ClientesVideos.Any(x => x.ClienteId == cliente.Id))
                    {
                        videos.Add(video);
                    }
                }
            }

            if (!string.IsNullOrEmpty(searchName))
            {
                videos = videos.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            int count = videos.Count;
            var resultAsPagedList = new StaticPagedList<Video>(videos.AsEnumerable<Video>(), page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Videos", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            ViewBag.Clientes = _ClienteService.GetAll().OrderBy(x => x.RazonSocial);
            return View();
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Create(VideoViewModel vm)
        {
            ViewBag.Clientes = _ClienteService.GetAll().OrderBy(x => x.RazonSocial);

            if (!vm.EsPublico)
            {
                if (vm.ClienteId == 0)
                {
                    return View(vm);
                }
            }

            if (ModelState.IsValid)
            {
                Video video = vm.ConvertVideoViewModelToVideo();
                _VideoService.Create(video);

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id = 0)
        {
            Video video = _VideoService.FindBy(x => x.Id == id).FirstOrDefault();
            VideoViewModel vm = new VideoViewModel(video);

            if (video == null)
            {
                return HttpNotFound();
            }

            ViewBag.Clientes = _ClienteService.GetAll().OrderBy(x => x.RazonSocial);
            return View(vm);
        }

        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Edit(VideoViewModel vm)
        {
            if (ModelState.IsValid)
            {
                Video video = _VideoService.FindBy(x => x.Id == vm.Id).FirstOrDefault();

                if (video.EsPublico)
                {
                    if (!vm.EsPublico)
                    {
                        VideosCliente vc = new VideosCliente();
                        vc.ClienteId = vm.ClienteId;
                        vc.VideoId = video.Id;
                        video.ClientesVideos.Add(vc);
                    }
                }
                else
                {

                    if (vm.EsPublico)
                    {
                        video.ClientesVideos.Clear();
                    }
                    else
                    {
                        if (video.ClientesVideos.FirstOrDefault().ClienteId != vm.ClienteId)
                        {
                            video.ClientesVideos.FirstOrDefault().ClienteId = vm.ClienteId;
                        }
                    }

                }

                video.Descripcion = vm.Descripcion;
                video.Alias = vm.Alias;
                video.EsPublico = vm.EsPublico;

                _VideoService.Update(video);

                return RedirectToAction("Index");
            }

            return View(vm);

        }


        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = _VideoService.FindBy(x => x.Id == id).FirstOrDefault();
            video.ClientesVideos.Clear();
            _VideoService.Delete(video);
            return RedirectToAction("Index");
        }

        #endregion
    }
}