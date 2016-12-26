
namespace Paramedic.Gestion.Model
{
    public class TicketQueryControllerParametersDTO : QueryControllerParametersDTO
    {
        #region Properties
        public string FutureFeature { get; set; }

        public int UserId { get; set; }

        public bool IsAdmin { get; set; }

        #endregion

        #region Constructors

        public TicketQueryControllerParametersDTO(string searchDescription, int pageSize, int page, string futureFeature, int userId, bool isAdmin) : base(searchDescription, pageSize, page)
        {
            FutureFeature = futureFeature;
            UserId = userId;
            IsAdmin = isAdmin;
        }

        #endregion

    }
}