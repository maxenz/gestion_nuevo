using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;

namespace Gestion.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class ClientesUsuariosController : Controller
    {
        private GestionDb db = new GestionDb();

        //
        // GET: /ClientesUsuarios/

        public ActionResult Index(int ClienteID, String searchName = null, int page = 1)
        {

            var qUsuarios = from u in db.ClientesUsuarios where u.ClienteID == ClienteID select u;

            if (qUsuarios == null)
            {
                return HttpNotFound();
            }

            qUsuarios = qUsuarios.OrderBy(p => p.Usuario.UserName);

            ViewBag.Cliente_ID = ClienteID;

            return PartialView("_ClientesUsuarios", qUsuarios.ToList());

        }

        //
        // GET: /ClientesUsuarios/Details/5

        public ActionResult Details(int id = 0)
        {
            ClientesUsuario clientesusuario = db.ClientesUsuarios.Find(id);
            if (clientesusuario == null)
            {
                return HttpNotFound();
            }
            return View(clientesusuario);
        }

        //
        // GET: /ClientesUsuarios/Create

        public ActionResult Create()
        {
            setGeneralData();
            return View();
        }

        private void setGeneralData()
        {
            ViewBag.Usuarios = db.UserProfiles.ToList();
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial");
            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName");
        }

        private void setGeneralDataSelected(ClientesUsuario clientesusuario)
        {
            ViewBag.Usuarios = db.UserProfiles.ToList();
            ViewBag.ClienteID = new SelectList(db.Clientes, "ID", "RazonSocial", clientesusuario.ClienteID);
            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName", clientesusuario.UsuarioID);
        }

        //
        // POST: /ClientesUsuarios/Create

        [HttpPost]
        public ActionResult Create(ClientesUsuario clientesusuario)
        {
            if (ModelState.IsValid)
            {
                db.ClientesUsuarios.Add(clientesusuario);
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientesusuario.ClienteID });
            }

            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        //
        // GET: /ClientesUsuarios/Edit/5

        public ActionResult Edit(int id = 0)
        {
            ClientesUsuario clientesusuario = db.ClientesUsuarios.Find(id);
            if (clientesusuario == null)
            {
                return HttpNotFound();
            }
            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        //
        // POST: /ClientesUsuarios/Edit/5

        [HttpPost]
        public ActionResult Edit(ClientesUsuario clientesusuario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(clientesusuario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", "Clientes", new { id = clientesusuario.ClienteID });
            }
            setGeneralDataSelected(clientesusuario);
            return View(clientesusuario);
        }

        //
        // GET: /ClientesUsuarios/Delete/5

        public ActionResult Delete(int id = 0)
        {
            ClientesUsuario clientesusuario = db.ClientesUsuarios.Find(id);
            if (clientesusuario == null)
            {
                return HttpNotFound();
            }
            return View(clientesusuario);
        }

        //
        // POST: /ClientesUsuarios/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ClientesUsuario clientesusuario = db.ClientesUsuarios.Find(id);
            var cliente_id = clientesusuario.ClienteID;
            db.ClientesUsuarios.Remove(clientesusuario);
            db.SaveChanges();
            return RedirectToAction("Index", routeValues: new { ClienteID = cliente_id });
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}