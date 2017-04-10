
namespace Paramedic.Gestion.Model
{
    public class TicketQueryControllerParametersDTO : QueryControllerParametersDTO
    {
        #region Properties
        public string FutureFeature { get; set; }

        public int UserId { get; set; }

        public bool IsAdmin { get; set; }

        public int TicketClasificacionId { get; set; }

        #endregion

        #region Constructors

        public TicketQueryControllerParametersDTO(string searchDescription, int pageSize, int page, int userId, bool isAdmin, int ticketClasificacionId) : base(searchDescription, pageSize, page)
        {
            UserId = userId;
            IsAdmin = isAdmin;
            TicketClasificacionId = ticketClasificacionId;        
        }

        #endregion

    }
}