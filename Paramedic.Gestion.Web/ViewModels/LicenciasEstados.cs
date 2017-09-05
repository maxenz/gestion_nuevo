using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class LicenciasEstados
    {
        public int ID { get; set; }
        public string Estado { get; set; }
        public string Productos { get; set; }
        public string Serial { get; set; }
        public string NumeroLlave { get; set; }
    }
}