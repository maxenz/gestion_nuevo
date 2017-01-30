using Paramedic.Gestion.Model;
using Paramedic.Gestion.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class LicenciasLogViewModel
    {
        #region Properties

        public string Cliente { get; set; }

        public int ClienteId { get; set; }

        public string Serial { get; set; }

        [Display(Name = "Fecha y Hora")]
        public DateTime FechaHora { get; set; }

        public string IP { get; set; }

        public string Referencia { get; set; }

        #endregion

        #region Constructors

        public LicenciasLogViewModel(LicenciasLog log)
        {

            this.Cliente = "No hay datos del cliente";
            this.Serial = "No hay datos de la licencia";

            if (log.Licencia != null)
            {
                this.Serial = log.Licencia.Serial;
                if (log.Licencia.ClientesLicencia != null)
                {
                    this.Cliente = log.Licencia.ClientesLicencia.Cliente.RazonSocial;
                    this.ClienteId = log.Licencia.ClientesLicencia.Cliente.Id;
                }
            }

            this.IP = log.IP;
            this.Referencia = log.Referencias;
            this.FechaHora = log.CreatedDate;
            this.Referencia = log.GenericDescription;
            
            if (log.Type == LicenciasLogType.Android)
            {
                this.Referencia = string.Format("Android connection: {0}", log.GenericDescription);
            }

        }

        #endregion
    }
}