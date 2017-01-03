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
        public int Id { get; set; }

        [Display(Name="Razón Social")]

        public string RazonSocial { get; set; }

        public string Email { get; set; }

        [Display(Name="Teléfono")]
        public string Telefono { get; set; }

        [Display(Name="País")]
        public string Pais { get; set; }

        public string Provincia { get; set; }

        public string Localidad { get; set; }

        [Display(Name = "Gestión")]
        public string Gestion { get; set; }

        [Display(Name = "Fecha Ult. Gestión")]
        public string FecUltGestion { get; set; }
    }
}