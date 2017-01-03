namespace Paramedic.Gestion.Model
{
    public class QueryControllerParametersDTO
    {
        #region Properties

        public string SearchDescription { get; set; }
        public int PageSize { get; set; }
        public int Page { get; set; }

        #endregion

        #region Constructors
        public QueryControllerParametersDTO(string searchDescription, int pageSize, int page)
        {
            SearchDescription = searchDescription;
            PageSize = pageSize;
            Page = page;
        }

        #endregion

    }
}