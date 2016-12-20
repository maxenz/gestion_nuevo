using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;


namespace Gestion.ViewModels
{
    public class ClientesPrincipal
    {
        public int ID { get; set; }
        [Display(Name="Razón Social")]
        public String RazonSocial { get; set; }
        public String Email { get; set; }
        [Display(Name="Teléfono")]
        public String Telefono { get; set; }
        [Display(Name="País")]
        public String Pais { get; set; }
        public String Provincia { get; set; }
        public String Localidad { get; set; }
        [Display(Name = "Gestión")]
        public String Gestion { get; set; }
        [Display(Name = "Fecha Ult. Gestión")]
        public String FecUltGestion { get; set; }
    }
}