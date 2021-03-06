﻿using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Paramedic.Gestion.Service;
using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using Paramedic.Gestion.Web.Converters;
using Paramedic.Gestion.Web.ViewModels;
using System;
using SocialMedia.Services;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.Controllers
{
	[Authorize]
	public class MisTicketsController : Controller
	{
		#region Properties

		ITicketService _TicketService;
		IUserProfileService _UserProfileService;
		IClienteService _ClienteService;
		IClientesUsuarioService _ClientesUsuarioService;
		ITicketsClasificacionService _TicketsClasificacionService;
		ITicketsClasificacionUsuarioService _TicketsClasificacionUsuarioService;

		private int controllersPageSize = 6;

		#endregion

		#region Constructors

		public MisTicketsController(
			ITicketService TicketService,
			IUserProfileService UserProfileService,
			IClientesUsuarioService ClientesUsuarioService,
			IClienteService ClienteService,
			ITicketsClasificacionService TicketsClasificacionService,
			ITicketsClasificacionUsuarioService TicketsClasificacionUsuarioService
			)
		{
			_TicketService = TicketService;
			_UserProfileService = UserProfileService;
			_ClientesUsuarioService = ClientesUsuarioService;
			_ClienteService = ClienteService;
			_TicketsClasificacionService = TicketsClasificacionService;
		}

		#endregion

		#region Public Methods

		public ActionResult Index(string searchName = null, int page = 1, int selTicketsClasificacion = 0, int selCliente = 0, int selUsuario = 0, int selEstado = 0)
		{
			int userId = _UserProfileService.GetCurrentUserId(User.Identity.Name);
			bool isAdmin = User.IsInRole("Administrador");
			IList<TicketViewModel> vmTickets = new List<TicketViewModel>();

			var estado = selEstado != 0 ? (TicketEstadoType) selEstado : 0;

			TicketQueryControllerParametersDTO queryParameters = new TicketQueryControllerParametersDTO(searchName, controllersPageSize, page, userId, isAdmin, selTicketsClasificacion, estado, selUsuario, selCliente);

			IEnumerable<Ticket> tickets = _TicketService.GetTickets(queryParameters);

			setDropdowns();

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

		public ActionResult Create()
		{
			ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName");
			setDropdownClasificaciones();
			return View();
		}

		[HttpPost]
		public ActionResult CreateEditTicketEvento(TicketEvento ticketEvento, HttpPostedFileBase image = null)
		{
			TicketEventoType type = User.IsInRole("Administrador") ? TicketEventoType.Answer : TicketEventoType.Question;
			int userProfileId = _UserProfileService.GetCurrentUserId(User.Identity.Name);

			Ticket ticket = _TicketService.GetById(ticketEvento.TicketId);
			if (ticketEvento.Id == 0)
			{
				ticket = TicketConverter.CreateTicketWithEvent(ticket, ticketEvento.Descripcion, image, type, userProfileId);
			}

			ticket.TicketEstadoType = type == TicketEventoType.Answer ? TicketEstadoType.Answered : TicketEstadoType.NotAnswered;

			_TicketService.Update(ticket);
			MailService.Instance.SendNewTicketEventoMail(ticket.TicketEventos.LastOrDefault());

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
					.FirstOrDefault(x => x.TicketTipoEventoType == TicketEventoType.Question);
				ViewBag.lastRelevantData = lastQuestion != null ? lastQuestion.Descripcion : "";
				ViewBag.lastRelevantLabel = "Última consulta";
				ViewBag.title = string.Format("{0}: {1}", "Respuesta de", User.Identity.Name);

			}
			else
			{
				TicketEvento lastAnswer = ticket
					.TicketEventos
					.OrderByDescending(x => x.UpdatedDate)
					.FirstOrDefault(x => x.TicketTipoEventoType == TicketEventoType.Answer);

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

		[HttpPost]
		public ActionResult Create(Ticket ticket, string descripcion, HttpPostedFileBase image)
		{
			if (ModelState.IsValid)
			{
				ticket.TicketEstadoType = TicketEstadoType.NotAnswered;
				ticket.UserProfileId = _UserProfileService.GetCurrentUserId(User.Identity.Name);
				ticket.Usuario = _UserProfileService.FindBy(x => x.Id == ticket.UserProfileId).FirstOrDefault();
				ticket = TicketConverter.CreateTicketWithEvent(ticket, descripcion, image, TicketEventoType.Question, ticket.UserProfileId);
				_TicketService.Create(ticket);
				ticket.TicketsClasificacion = _TicketsClasificacionService.GetById(ticket.TicketsClasificacionId);
				MailService.Instance.SendNewTicketEventoMail(ticket.TicketEventos.FirstOrDefault());
				return RedirectToAction("Index");
			}

			ViewBag.UsuarioID = new SelectList(_UserProfileService.GetAll(), "Id", "UserName");
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

		public ActionResult CreateAdminTicket()
		{
			setDropdownClientes();
			setDropdownClasificaciones();
			return View("_FormTicketAdmin");
		}

		[HttpPost]
		public ActionResult CreateAdminTicket(TicketAdminViewModel vm, HttpPostedFileBase image = null)
		{
			try
			{
				if (ModelState.IsValid)
				{
					Ticket ticket = vm.ToTicket();
					ticket.TicketEstadoType = TicketEstadoType.NotAnswered;
					ticket.Usuario = _UserProfileService.GetById(ticket.UserProfileId);
					ticket = TicketConverter.CreateTicketWithEvent(ticket, vm.Descripcion, image, TicketEventoType.Question, ticket.UserProfileId);
					_TicketService.Create(ticket);
					ticket.TicketsClasificacion = _TicketsClasificacionService.GetById(ticket.TicketsClasificacionId);
					MailService.Instance.SendNewAdminTicketMail(ticket.TicketEventos.FirstOrDefault());
					return RedirectToAction("Index");
				}
			}
			catch (Exception exception)
			{
				LoggingService.Instance.Write(LoggingTypes.Error, exception.Message);
			}


			setDropdownClientes();
			setDropdownClasificaciones();
			return View("_FormTicketAdmin");
		}

		public JsonResult GetUsersByClientId(string id)
		{

			int clientId = Convert.ToInt32(id);

			IEnumerable<UserProfile> users =
				_ClientesUsuarioService
				.FindBy(x => x.ClienteId == clientId)
				.Select(x => x.Usuario);

			return Json(new SelectList(users, "Id", "UserName"));
		}

		#endregion

		#region Private Methods

		private void setDropdownClientes()
		{
			var clients = _ClienteService.GetAll().Where(x => x.ClientesUsuarios.Count > 0).OrderBy(x => x.RazonSocial);

			ViewBag.Clientes = new SelectList(clients, "Id", "RazonSocial");
		}

		private void setDropdownUsuarios()
		{
			var estados = _UserProfileService.GetAll().Select(x => { x.UserName = x.UserName.ToUpper(); return x; }).OrderBy(x => x.UserName);

			ViewBag.Usuarios = new SelectList(estados, "Id", "UserName");
		}

		private void setDropdownClasificaciones()
		{
			IEnumerable<Model.TicketsClasificacion> lstClasif = _TicketsClasificacionService.GetAll();
			IEnumerable<SelectListItem> items = (from c in lstClasif
												 select new SelectListItem
												 {
													 Selected = false,
													 Text = c.Descripcion.ToUpper(),
													 Value = c.Id.ToString()
												 }).OrderBy(x => x.Text);

			ViewBag.Clasificaciones = new SelectList(items, "Value", "Text");
		}

		private void setDropdownEstados()
		{
			var estados = (from TicketEstadoType c in Enum.GetValues(typeof(TicketEstadoType))
						   select new
						   {
							   Selected = false,
							   Text = c.GetAttribute<DisplayAttribute>().Name.ToUpper(),
							   Value = (int) c
						   }).OrderBy(x => x.Text);

			ViewBag.Estados = new SelectList(estados, "Value", "Text");
		}

		private void setDropdowns()
		{
			setDropdownClasificaciones();
			setDropdownClientes();
			setDropdownEstados();
			setDropdownUsuarios();
		}

		

		#endregion

	}
}