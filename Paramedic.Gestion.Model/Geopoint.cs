namespace Paramedic.Gestion.Model
{
    public class Geopoint
    {
        #region Properties

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        #endregion

        #region Constructors

        public Geopoint(string latitude, string longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        #endregion

    }
}
