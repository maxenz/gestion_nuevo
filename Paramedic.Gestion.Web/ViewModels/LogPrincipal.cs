using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;


namespace Gestion.ViewModels
{
    public class LogPrincipal
    {

        public String Cliente { get; set; }
        public int ClienteID { get; set; }
        public String Serial { get; set; }
        [Display(Name="Fecha y Hora")]
        public DateTime FechaHora { get; set; }
        public String IP { get; set; }
        public String Referencia { get; set; }

    }
}