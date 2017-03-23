using Paramedic.Gestion.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class ClientesGestionViewModel
    {
        #region Properties

        public int Id { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        public string Observaciones { get; set; }

        public HttpPostedFileBase PdfUpload { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Fecha { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Fecha Recontacto")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? FechaRecontacto { get; set; }

        #endregion

        #region Constructors

        public ClientesGestionViewModel() { }

        public ClientesGestionViewModel(ClientesGestion clientesGestion)
        {
            this.Id = clientesGestion.Id;
            this.Observaciones = clientesGestion.Observaciones;
            this.FechaRecontacto = clientesGestion.FechaRecontacto;
            this.Fecha = clientesGestion.Fecha;
            this.ClienteId = clientesGestion.ClienteId;
            this.EstadoId = clientesGestion.EstadoId;

        }

        public ClientesGestion ClientesGestionViewModelToClientesGestion()
        {
            ClientesGestion cg = new ClientesGestion();
            cg.Id = this.Id;
            cg.Observaciones = this.Observaciones;
            cg.FechaRecontacto = this.FechaRecontacto;
            cg.Fecha = this.Fecha;
            cg.ClienteId = this.ClienteId;
            cg.EstadoId = this.EstadoId;

            if (this.PdfUpload != null)
            {
                cg.PdfGestion = new byte[this.PdfUpload.ContentLength];
                this.PdfUpload.InputStream.Read(cg.PdfGestion, 0, this.PdfUpload.ContentLength);
            }

            return cg;
        }

        #endregion
    }
}