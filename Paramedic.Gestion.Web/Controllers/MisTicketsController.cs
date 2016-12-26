using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gestion.Models;
using Gestion.ViewModels;
using PagedList;
using System.Net.Mail;
using System.Text;
using System.Web.Configuration;
using Paramedic.Gestion.Service;

namespace Gestion.Controllers
{
    //[Authorize]
    public class MisTicketsController : Controller
    {
        ITicketService _TicketService;

        public MisTicketsController(ITicketService TicketService)
        {
            _TicketService = TicketService;
        }
        private string emailAdministrator = WebConfigurationManager.AppSettings["administratorEmail"];
        //
        // GET: /MisTickets/

        public ActionResult Index(string searchName = null,string chkFutureFeatures = null, int page = 1)
        {

            int userID = GetCurrentUserID();

            IQueryable<Ticket> allTickets = null;

            if (!isAdministrator())
            {
                allTickets = db.Tickets.Where(p => p.UsuarioID == userID);
            }
            else
            {
                allTickets = db.Tickets;
            }


            var qTickets = new List<TicketsPrincipal>();
            foreach (var ticket in allTickets)
            {
                string estado = "";

                if (ticket.Resuelto == true)
                {
                    estado = "RESUELTO";
                }
                else
                {
                    estado = ticket.TicketEstado.Descripcion;
                }

                Cliente cli = db.ClientesUsuarios
                                .Where(x => x.UsuarioID == ticket.UsuarioID)
                                .Select(x => x.Cliente).FirstOrDefault();
                                
                qTickets.Add(new TicketsPrincipal
                {
                    Id = ticket.ID,
                    FechaHora = ticket.FechaCreacion,
                    Asunto = ticket.Asunto,
                    Estado = estado,
                    Usuario = ticket.Usuario.UserName,
                    Cliente = cli.RazonSocial,
                    ClienteId = cli.ID,
                    FuturaMejora = ticket.FuturaMejora
                });
            }

            if (!String.IsNullOrEmpty(searchName))
            {
                qTickets = qTickets.Where(p => p.Cliente.ToUpper().Contains(searchName.ToUpper())).ToList();
            }

            if (!String.IsNullOrEmpty(chkFutureFeatures))
            {
                qTickets = qTickets.Where(x => x.FuturaMejora).ToList();
            }

            qTickets = qTickets.OrderBy(p => p.Estado).ThenByDescending(p => p.FechaHora).ToList();

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MisTickets", qTickets.ToPagedList(page, 6));
            }

            return View(qTickets.ToPagedList(page, 6));
        }

        //
        // GET: /MisTickets/Create

        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName");
            ViewBag.TicketEstadoID = new SelectList(db.TicketEstados, "ID", "Descripcion");
            return View();
        }

        private TicketEvento doTicketEvento(Ticket ticket, string descripcion, int tipoEvento, HttpPostedFileBase image)
        {
            TicketEvento tkEvento = new TicketEvento();
            tkEvento.Descripcion = descripcion == "" ? ticket.Asunto : descripcion;
            tkEvento.FechaCreacion = DateTime.Now;
            tkEvento.TicketID = ticket.ID;
            tkEvento.UserID = GetCurrentUserID();
            tkEvento.TicketTipoEventoID = tipoEvento;

            setImageTicketEvento(tkEvento, image);

            db.TicketEventos.Add(tkEvento);
            db.SaveChanges();

            return tkEvento;
        }

        private void doEmailAdministrator(TicketEvento tkEvento)
        {
            var subject = "Consulta Shaman Gestión del " + DateTime.Now;
            string body = createBodyForAdministrator(tkEvento);
            sendEmail(emailAdministrator, subject, body);

        }


        private void doEmailCliente(TicketEvento tkEvento)
        {
            var subject = "Respuesta Shaman Gestión del " + DateTime.Now;
            string body = createBodyForCliente(tkEvento);
            sendEmail(tkEvento.Ticket.Usuario.Email, subject, body);

        }

        [HttpPost]
        public ActionResult CreateTicketEvento(int id, string descripcion, int tipoEvento, HttpPostedFileBase image)
        {
            Ticket ticket = db.Tickets.Find(id);

            TicketEvento tkEvento = doTicketEvento(ticket, descripcion, tipoEvento, image);

            if (tipoEvento == 2)
            {
                ticket.TicketEstadoID = 1;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();

                doEmailCliente(tkEvento);
            }
            else
            {
                ticket.TicketEstadoID = 2;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                doEmailAdministrator(tkEvento);

            }

            setEditableTicket(ticket);

            return RedirectToAction("Edit", ticket);

        }

        private string createBodyForCliente(TicketEvento tkEvento)
        {
            string href = "<a href=\"http://www.200.49.156.125:57771/MisTickets/Edit/" + tkEvento.TicketID + "\">Aquí</a>";
            StringBuilder body = new StringBuilder()
                .AppendLine("Asunto: " + tkEvento.Ticket.Asunto + "<br /><br />")
                .AppendLine("Para acceder a la respuesta haga click:  " + href);

            return body.ToString();
        }

        private string createBodyForAdministrator(TicketEvento tkEvento)
        {
            StringBuilder body = new StringBuilder()
                .AppendLine("Usuario: " + tkEvento.Ticket.Usuario.UserName + "<br /><br />")
                .AppendLine("Asunto: " + tkEvento.Ticket.Asunto + "<br /><br />")
                .AppendLine("Descripción: " + tkEvento.Descripcion);

            return body.ToString();
        }

        private void sendEmail(string to, string subject, string body)
        {
            try
            {
                MailMessage mailMsg = new MailMessage();
                mailMsg.To.Add(to);
                MailAddress mailAddress = new MailAddress("sistemas@paramedic.com.ar");
                mailMsg.From = mailAddress;
                mailMsg.Subject = subject;
                mailMsg.Body = body;
                mailMsg.IsBodyHtml = true;

                SmtpClient smtpClient = new SmtpClient("smtp.fibertel.com.ar", 25);
                System.Net.NetworkCredential credentials =
                   new System.Net.NetworkCredential("sistemas.paramedic.com.ar", "pwsi01");
                smtpClient.Credentials = credentials;
                smtpClient.Send(mailMsg);
            }
            catch
            {
                
            }


        }

        [HttpPost]
        public ActionResult EditTicketEvento(int ID, string descripcion, HttpPostedFileBase image)
        {

            TicketEvento tkEvento = db.TicketEventos.Find(ID);
            setImageTicketEvento(tkEvento, image);
            tkEvento.Descripcion = descripcion == "" ? tkEvento.Descripcion : descripcion;
            tkEvento.UserID = GetCurrentUserID();
            tkEvento.FechaCreacion = DateTime.Now;
            db.Entry(tkEvento).State = EntityState.Modified;
            db.SaveChanges();

            Ticket ticket = db.Tickets.Find(tkEvento.TicketID);

            setEditableTicket(ticket);

            return RedirectToAction("Edit", ticket);

        }

        public FileContentResult GetImage(int id)
        {
            TicketEvento tkEvento = db.TicketEventos.FirstOrDefault(p => p.ID == id);
            if (tkEvento != null)
            {
                return File(tkEvento.ImageData, tkEvento.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult Create(Ticket ticket, string Descripcion, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {

                ticket.Resuelto = false;
                ticket.UsuarioID = GetCurrentUserID();
                ticket.TicketEstadoID = 2;
                ticket.FechaCreacion = DateTime.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();

                TicketEvento tkEvento = doTicketEvento(ticket, Descripcion, 1, image);

                doEmailAdministrator(tkEvento);

                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName", ticket.UsuarioID);
            ViewBag.TicketEstadoID = new SelectList(db.TicketEstados, "ID", "Descripcion", ticket.TicketEstadoID);
            return View(ticket);
        }

        private void setImageTicketEvento(TicketEvento tkEvento, HttpPostedFileBase image)
        {
            if (image != null)
            {
                tkEvento.ImageMimeType = image.ContentType;
                tkEvento.ImageData = new byte[image.ContentLength];
                image.InputStream.Read(tkEvento.ImageData, 0, image.ContentLength);
            }
        }

        private int GetCurrentUserID()
        {

            var user = db.UserProfiles.Where(u => u.UserName == User.Identity.Name).FirstOrDefault();
            return user.UserId;

        }

        //
        // GET: /MisTickets/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ticket ticket = db.Tickets.Find(id);
            int userID = GetCurrentUserID();

            if (isAdministrator())
            {
                if (ticket == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                if (ticket == null || ticket.UsuarioID != userID)
                {
                    return HttpNotFound();
                }
            }

            setEditableTicket(ticket);

            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName", ticket.UsuarioID);
            ViewBag.TicketEstadoID = new SelectList(db.TicketEstados, "ID", "Descripcion", ticket.TicketEstadoID);
            return View(ticket);
        }

        private void setEditableTicket(Ticket ticket)
        {
            TicketEvento ultEvento = ticket.TicketEventos.OrderByDescending(p => p.FechaCreacion).FirstOrDefault();
            string lastTipoEvento = db.TicketTipoEventos.Find(ultEvento.TicketTipoEventoID).Descripcion;

            DateTime dtLastEvento = ultEvento.FechaCreacion;
            DateTime dtNow = DateTime.Now;
            TimeSpan span = dtNow - dtLastEvento;
            double totalMinutes = span.TotalMinutes;

            if (isAdministrator())
            {
                if (lastTipoEvento.Equals("Respuesta"))
                {

                    if (totalMinutes <= 10 && ticket.Resuelto == false)
                    {
                        ViewBag.RtaEditable = 1;
                    }

                }
            }
            else
            {
                if (lastTipoEvento.Equals("Consulta"))
                {

                    if (totalMinutes <= 10 && ticket.Resuelto == false)
                    {
                        ViewBag.Editable = 1;
                    }

                }
            }

        }

        [HttpPost]
        public ActionResult CloseTicket(int ID)
        {
            Ticket ticket = db.Tickets.Find(ID);
            ticket.Resuelto = true;
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
            return PartialView("_Cerrado");
        }

        //
        // POST: /MisTickets/Edit/5

        [HttpPost]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(db.UserProfiles, "UserId", "UserName", ticket.UsuarioID);
            ViewBag.TicketEstadoID = new SelectList(db.TicketEstados, "ID", "Descripcion", ticket.TicketEstadoID);
            return View(ticket);
        }

        //
        // GET: /MisTickets/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        //
        // POST: /MisTickets/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetFutureFeature(bool isFutureFeature, int ticketId)
        {
            Ticket ticket = db.Tickets.Find(ticketId);
            ticket.FuturaMejora = isFutureFeature;
            db.Entry(ticket).State = EntityState.Modified;
            db.SaveChanges();
            return new HttpStatusCodeResult(200);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}