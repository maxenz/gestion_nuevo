using Paramedic.Gestion.Model.Enums;

namespace Paramedic.Gestion.Model
{
    public class ClientControllerParametersDTO : QueryControllerParametersDTO
    {

        #region Properties

        public ClientType SelectedClientType { get; set; }

        public int SelectedViewType { get; set; }

        #endregion

        #region Constructors

        public ClientControllerParametersDTO(string searchDescription, int pageSize, int page, ClientType selectedClientType, int selectedViewType) : base(searchDescription, pageSize, page)
        {
            this.SelectedClientType = selectedClientType;
            this.SelectedViewType = selectedViewType;
        }

        #endregion

    }
}
