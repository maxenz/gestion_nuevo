using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using PagedList;

namespace Gestion.Controllers
{
    [Authorize]
    public class VideosController : Controller
    {
        private GestionDb db = new GestionDb();

        private bool isAdministrator()
        {

            if (User.IsInRole("Administrador"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        private int GetCurrentUserID()
        {

            var user = db.UserProfiles.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return user.UserId;

        }

        public ActionResult Index(string searchName = null, int page = 1)
        {

            IList<Video> allVideos = new List<Video>();

            if (isAdministrator())
            {
                allVideos = db.Videos.ToList();
            }
            else
            {
                var vd = db.Videos;
                foreach (var v in vd)
                {
                    int user_id = GetCurrentUserID();
                    int cli_video_id = db.VideosClientes.Where(g => g.VideoID == v.ID).Select(g => g.ClienteID).FirstOrDefault();
                    int cli_id = db.ClientesUsuarios.Where(c => c.UsuarioID == user_id).Select(c => c.ClienteID).FirstOrDefault();
                    if ((v.esPublico) || (cli_video_id == cli_id))
                    {
                        allVideos.Add(v);
                    }
                }
            }


            if (!String.IsNullOrEmpty(searchName))
            {

                allVideos = allVideos.Where(p => p.Descripcion.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            allVideos = allVideos.OrderBy(p => p.Descripcion).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Videos", allVideos.ToPagedList(page, 6));
            }

            return View(allVideos.ToPagedList(page, 6));
        }

        //
        // GET: /Videos/Details/5

        private void getClientes()
        {

            List<Cliente> clientes = new List<Cliente>();

            clientes = db.Clientes.OrderBy(p => p.RazonSocial).ToList();

            ViewBag.Clientes = clientes;

        }

        public ActionResult Details(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        //
        // GET: /Videos/Create
        [Authorize(Roles="Administrador")]
        public ActionResult Create()
        {
            getClientes();
            return View();
        }

        //
        // POST: /Videos/Create
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Create(Video video, string vidCliente)
        {

            getClientes();

            if (!video.esPublico)
            {
                if (vidCliente.Equals(null))
                {
                    return View(video);
                }
            }

            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                db.SaveChanges();
                if (vidCliente != null)
                {

                    VideosCliente vidCli = new VideosCliente();
                    vidCli.ClienteID = Convert.ToInt32(vidCliente);
                    vidCli.VideoID = video.ID;
                    db.VideosClientes.Add(vidCli);
                    db.SaveChanges();

                }


                return RedirectToAction("Index");
            }

            return View(video);
        }

        //
        // GET: /Videos/Edit/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Edit(int id = 0)
        {
            Video video = db.Videos.Find(id);
            VideosCliente vc = db.VideosClientes.Where(v => v.VideoID == video.ID).FirstOrDefault();
            if ( vc != null)
            {
                ViewBag.vidCliente = vc.ClienteID;
            }
            if (video == null)
            {
                return HttpNotFound();
            }

            getClientes();
            return View(video);
        }

        //
        // POST: /Videos/Edit/5
        [Authorize(Roles = "Administrador")]
        [HttpPost]
        public ActionResult Edit(Video video, string vidCliente)
        {

            VideosCliente vc = db.VideosClientes.Where(v => v.VideoID == video.ID).FirstOrDefault();

            if (vidCliente == null)
            {
                vidCliente = "";
            }

            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                db.SaveChanges();

                if (video.esPublico)
                {
                    if (vc != null)
                    {
                        db.VideosClientes.Remove(vc);
                        db.SaveChanges();
                    }

                    return RedirectToAction("Index");
                }
                else
                {
                    if (vidCliente == "")
                    {
                        return anyError(video, vc);
                    }
                    else
                    {
                        if (vc != null)
                        {
                            vc.ClienteID = Convert.ToInt32(vidCliente);
                            db.Entry(vc).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                        else
                        {
                            VideosCliente nvc = new VideosCliente();
                            nvc.ClienteID = Convert.ToInt32(vidCliente);
                            nvc.VideoID = video.ID;
                            db.VideosClientes.Add(nvc);
                            db.SaveChanges();
                        }

                        return RedirectToAction("Index");
                    }
                }

            }
            else
            {
                return anyError(video, vc);
            }

        }

        private ActionResult anyError(Video video, VideosCliente vc)
        {
            getClientes();
            if (vc != null)
            {
                ViewBag.vidCliente = vc.ClienteID;
            }
            return View(video);
        }

        //
        // GET: /Videos/Delete/5
        [Authorize(Roles = "Administrador")]
        public ActionResult Delete(int id = 0)
        {
            Video video = db.Videos.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        //
        // POST: /Videos/Delete/5
        [Authorize(Roles = "Administrador")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Video video = db.Videos.Find(id);
            db.Videos.Remove(video);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}