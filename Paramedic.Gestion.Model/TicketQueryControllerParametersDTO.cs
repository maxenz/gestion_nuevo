
using Paramedic.Gestion.Model.Enums;

namespace Paramedic.Gestion.Model
{
	public class TicketQueryControllerParametersDTO : QueryControllerParametersDTO
	{
		#region Properties
		public string FutureFeature { get; set; }

		public int UserId { get; set; }

		public bool IsAdmin { get; set; }

		public int TicketClasificacionId { get; set; }

		public TicketEstadoType Estado { get; set; }

		public int SelectedUserId { get; set; }

		public int ClienteId { get; set; }

		#endregion

		#region Constructors

		public TicketQueryControllerParametersDTO(
			string searchDescription,
			int pageSize,
			int page,
			int userId,
			bool isAdmin,
			int ticketClasificacionId,
			TicketEstadoType estado,
			int selectedUserId,
			int clienteId
			) : base(searchDescription, pageSize, page)
		{
			UserId = userId;
			IsAdmin = isAdmin;
			TicketClasificacionId = ticketClasificacionId;
			Estado = estado;
			SelectedUserId = selectedUserId;
			ClienteId = clienteId;
		}

		#endregion

	}
}