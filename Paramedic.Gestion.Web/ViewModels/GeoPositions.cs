using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;


namespace Gestion.ViewModels
{
    public class GeoPositions
    {

        public int ID { get; set; } 
        public String Cliente { get; set; }
        public String Latitud { get; set; }
        public String Longitud { get; set; }
        public String Localidad { get; set; }
        public String EmailPrincipal { get; set; }
        public String Telefono { get; set; }
        public String SitioWeb { get; set; }
        public int EstadoCliente { get; set; }
        public int? MedioDifusionID { get; set; }
        public String VersionShaman { get; set; }

    }
}