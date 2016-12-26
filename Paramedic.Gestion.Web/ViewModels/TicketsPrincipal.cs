using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using Paramedic.Gestion.Model;

namespace Gestion.ViewModels
{
    public class TicketsPrincipal
    {
        #region Properties
        public String Asunto { get; set; }
        [Display(Name = "Última actualización")]
        public DateTime FechaHora { get; set; }
        public String Estado { get; set; }
        public int Id { get; set; }
        public String Usuario { get; set; }
        public String Cliente { get; set; }
        public int ClienteId { get; set; }
        public bool FuturaMejora { get; set; }

        #endregion

    }
}