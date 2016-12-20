using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Gestion.ViewModels
{
    public class CuentaCorrienteViewModel
    {

        [Display(Name = "Tipo de Comprobante")]
        public string TipoComprobante { get; set; }

        [Display(Name = "Nro. de Comprobante")]
        public string NroComprobante { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Debe { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Haber { get; set; }

        public string Fecha { get; set; }

        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public double Saldo { get; set; }

    }
}