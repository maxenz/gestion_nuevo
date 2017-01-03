using Paramedic.Gestion.Model;
using System;
using System.Net;
using System.Xml.Linq;

namespace Paramedic.Gestion.Service
{
    public class GeolocalizationService
    {

        #region Public Methods

        public Geopoint GetLocalization(string address)
        {
            var requestUri = string.Format("http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false", Uri.EscapeDataString(address));

            var request = WebRequest.Create(requestUri);
            var response = request.GetResponse();
            var xdoc = XDocument.Load(response.GetResponseStream());

            var result = xdoc.Element("GeocodeResponse").Element("result");
            var locationElement = result.Element("geometry").Element("location");
            string lat = locationElement.Element("lat").Value.ToString();
            string lng = locationElement.Element("lng").Value.ToString();

            return new Geopoint(lat, lng);
        }

        #endregion

    }
}
