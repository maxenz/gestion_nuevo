﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.Configuration;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Web.Converters;
using Paramedic.Gestion.Web.ViewModels;

namespace Gestion.Controllers
{
    [Authorize]
    public class MisTicketsController : Controller
    {

        #region Properties

        ITicketService _TicketService;
        IUserProfileService _UserProfileService;
        IClientesUsuarioService _ClientesUsuarioService;

        private string emailAdministrator = WebConfigurationManager.AppSettings["administratorEmail"];
        private string roleAdministratorName = WebConfigurationManager.AppSettings["administratorRoleName"];
        private int controllersPageSize = 6;

        #endregion

        #region Constructors

        public MisTicketsController(ITicketService TicketService, IUserProfileService UserProfileService, IClientesUsuarioService ClientesUsuarioService)
        {
            _TicketService = TicketService;
            _UserProfileService = UserProfileService;
            _ClientesUsuarioService = ClientesUsuarioService;
        }

        #endregion

        #region Public Methods

        public ActionResult Index(string searchName = null, string chkFutureFeatures = null, int page = 1)
        {

            int userId = _UserProfileService.GetCurrentUserId(User.Identity.Name);
            bool isAdmin = User.IsInRole("Administrador");
            IList<TicketViewModel> vmTickets = new List<TicketViewModel>();

            TicketQueryControllerParametersDTO queryParameters = new TicketQueryControllerParametersDTO(searchName, controllersPageSize, page, chkFutureFeatures, userId, isAdmin);

            IEnumerable<Ticket> tickets = _TicketService.GetTickets(queryParameters);

            foreach (Ticket ticket in tickets)
            {
                int userProfileId = ticket.UserProfileId;
                ClientesUsuario clientesUsuario = _ClientesUsuarioService
                    .FindBy(x => x.UsuarioId == userProfileId)
                    .FirstOrDefault();
                TicketViewModel vm = new TicketViewModel(ticket);
                if (clientesUsuario != null)
                {
                    vm.Cliente = clientesUsuario.Cliente.RazonSocial;
                    vm.ClienteId = clientesUsuario.Cliente.Id;
                }

                vmTickets.Add(vm);
            }

            int count = _TicketService.FindBy(queryParameters).Count();
            var resultAsPagedList = new StaticPagedList<TicketViewModel>(vmTickets, page, controllersPageSize, count);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_MisTickets", resultAsPagedList);
            }

            return View(resultAsPagedList);
        }

        #endregion

        public ActionResult Create()
        {
            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName");
            return View();
        }

        [HttpPost]
        public ActionResult CreateEditTicketEvento(TicketEvento ticketEvento, HttpPostedFileBase image = null)
        {
            TicketEventoType type = User.IsInRole("Administrador") ? TicketEventoType.Answer : TicketEventoType.Question;
            int userProfileId = _UserProfileService.GetCurrentUserId(User.Identity.Name);

            Ticket ticket = _TicketService.FindBy(x => x.Id == ticketEvento.TicketId).FirstOrDefault();
            if (ticketEvento.Id == 0)
            {
                ticket = TicketConverter.CreateTicketWithEvent(ticket, ticketEvento.Descripcion, image, type, userProfileId);
            }

            ticket.TicketEstadoType = type == TicketEventoType.Answer ? TicketEstadoType.Answered : TicketEstadoType.NotAnswered;

            _TicketService.Update(ticket);

            return RedirectToAction("Edit", ticket);
        }

        [HttpGet]
        public ActionResult TicketEvento(int ticketId, int ticketEventoId = 0)
        {

            Ticket ticket = _TicketService.FindBy(x => x.Id == ticketId).FirstOrDefault();
            ViewBag.TicketAsunto = ticket.Asunto;
            ViewBag.TicketId = ticketId;

            if (User.IsInRole("Administrador"))
            {
                TicketEvento lastQuestion = ticket
                    .TicketEventos
                    .OrderByDescending(x => x.UpdatedDate)
                    .FirstOrDefault(x => x.TicketTipoEventoType == Paramedic.Gestion.Model.Enums.TicketEventoType.Question);
                ViewBag.lastRelevantData = lastQuestion != null ? lastQuestion.Descripcion : "";
                ViewBag.lastRelevantLabel = "Última consulta";
                ViewBag.title = string.Format("{0}: {1}", "Respuesta de", User.Identity.Name);

            }
            else
            {
                TicketEvento lastAnswer = ticket
                    .TicketEventos
                    .OrderByDescending(x => x.UpdatedDate)
                    .FirstOrDefault(x => x.TicketTipoEventoType == Paramedic.Gestion.Model.Enums.TicketEventoType.Answer);

                ViewBag.lastRelevantData = lastAnswer != null ? lastAnswer.Descripcion : "";
                ViewBag.lastRelevantLabel = "Última respuesta";
                ViewBag.title = string.Format("{0}: {1}", "Consulta de", User.Identity.Name);
            }

            if (ticketEventoId != 0)
            {
                TicketEvento ticketEvento = ticket.TicketEventos.FirstOrDefault(x => x.Id == ticketEventoId);
                return View(ticketEvento);
            }
            return View();
        }

        //private void doEmailAdministrator(TicketEvento tkEvento)
        //{
        //    var subject = "Consulta Shaman Gestión del " + DateTime.Now;
        //    string body = createBodyForAdministrator(tkEvento);
        //    sendEmail(emailAdministrator, subject, body);

        //}


        //private void doEmailCliente(TicketEvento tkEvento)
        //{
        //    var subject = "Respuesta Shaman Gestión del " + DateTime.Now;
        //    string body = createBodyForCliente(tkEvento);
        //    sendEmail(tkEvento.Ticket.Usuario.Email, subject, body);

        //}

        //private string createBodyForCliente(TicketEvento tkEvento)
        //{
        //    string href = "<a href=\"http://www.200.49.156.125:57771/MisTickets/Edit/" + tkEvento.TicketID + "\">Aquí</a>";
        //    StringBuilder body = new StringBuilder()
        //        .AppendLine("Asunto: " + tkEvento.Ticket.Asunto + "<br /><br />")
        //        .AppendLine("Para acceder a la respuesta haga click:  " + href);

        //    return body.ToString();
        //}

        //private string createBodyForAdministrator(TicketEvento tkEvento)
        //{
        //    StringBuilder body = new StringBuilder()
        //        .AppendLine("Usuario: " + tkEvento.Ticket.Usuario.UserName + "<br /><br />")
        //        .AppendLine("Asunto: " + tkEvento.Ticket.Asunto + "<br /><br />")
        //        .AppendLine("Descripción: " + tkEvento.Descripcion);

        //    return body.ToString();
        //}

        //private void sendEmail(string to, string subject, string body)
        //{
        //    try
        //    {
        //        MailMessage mailMsg = new MailMessage();
        //        mailMsg.To.Add(to);
        //        MailAddress mailAddress = new MailAddress("sistemas@paramedic.com.ar");
        //        mailMsg.From = mailAddress;
        //        mailMsg.Subject = subject;
        //        mailMsg.Body = body;
        //        mailMsg.IsBodyHtml = true;

        //        SmtpClient smtpClient = new SmtpClient("smtp.fibertel.com.ar", 25);
        //        System.Net.NetworkCredential credentials =
        //           new System.Net.NetworkCredential("sistemas.paramedic.com.ar", "pwsi01");
        //        smtpClient.Credentials = credentials;
        //        smtpClient.Send(mailMsg);
        //    }
        //    catch
        //    {

        //    }


        //}

        [HttpPost]
        public ActionResult Create(Ticket ticket, string descripcion, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {

                ticket.TicketEstadoType = TicketEstadoType.NotAnswered;
                ticket.UserProfileId = _UserProfileService.GetCurrentUserId(User.Identity.Name);
                ticket = TicketConverter.CreateTicketWithEvent(ticket, descripcion, image, TicketEventoType.Question, ticket.UserProfileId);
                _TicketService.Create(ticket);

                //doEmailAdministrator(tkEvento);

                return RedirectToAction("Index");
            }

            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName");
            //ViewBag.TicketEstadoID = new SelectList(db.TicketEstados, "ID", "Descripcion", ticket.TicketEstadoID);
            return View(ticket);
        }

        public ActionResult Edit(int id = 0)
        {
            Ticket ticket = _TicketService.FindBy(x => x.Id == id).FirstOrDefault();
            int userId = _UserProfileService.GetCurrentUserId(User.Identity.Name);

            if (User.IsInRole("Administrador"))
            {
                if (ticket == null)
                {
                    return HttpNotFound();
                }
            }
            else
            {
                if (ticket == null || ticket.UserProfileId != userId)
                {
                    return HttpNotFound();
                }
            }

            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName", ticket.UserProfileId);
            ViewBag.TicketEstadoID = ticket.TicketEstadoType;
            return View(ticket);
        }

        [HttpPost]
        public ActionResult CloseTicket(Ticket ticketForm)
        {
            Ticket ticket = _TicketService.FindBy(x => x.Id == ticketForm.Id).FirstOrDefault();
            ticket.TicketEstadoType = TicketEstadoType.Resolved;
            _TicketService.Update(ticket);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                _TicketService.Update(ticket);
                return RedirectToAction("Index");
            }
            ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName", ticket.UserProfileId);
            ViewBag.TicketEstadoID = ticket.TicketEstadoType;
            return View(ticket);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = _TicketService.FindBy(x => x.Id == id).FirstOrDefault();
            _TicketService.Delete(ticket);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SetFutureFeature(bool isFutureFeature, int ticketId)
        {
            Ticket ticket = _TicketService.FindBy(x => x.Id == ticketId).FirstOrDefault();
            ticket.FuturaMejora = isFutureFeature;
            _TicketService.Update(ticket);
            return new HttpStatusCodeResult(200);
        }

        public FileContentResult GetImage(int id, int ticketId)
        {
            Ticket ticket = _TicketService.FindBy(x => x.Id == ticketId).FirstOrDefault();
            TicketEvento tkEvento = ticket.TicketEventos.FirstOrDefault(x => x.Id == id);
            if (tkEvento != null)
            {
                return File(tkEvento.ImageData, tkEvento.ImageMimeType);
            }
            else
            {
                return null;
            }
        }

    }
}