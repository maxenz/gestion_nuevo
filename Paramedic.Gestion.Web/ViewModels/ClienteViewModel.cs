using Paramedic.Gestion.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Paramedic.Gestion.Web.ViewModels
{
    public class ClienteViewModel
    {
        #region Properties

        public int Id { get; set; }

        [Display(Name = "Razón Social")]

        public string RazonSocial { get; set; }

        public string Email { get; set; }

        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [Display(Name = "País")]
        public string Pais { get; set; }

        public string Provincia { get; set; }

        public string Localidad { get; set; }

        [Display(Name = "Gestión")]
        public string Gestion { get; set; }

        [Display(Name = "Fecha Ult. Gestión")]
        public DateTime FecUltGestion { get; set; }

        #endregion

        #region Constructors

        public ClienteViewModel() { }

        public ClienteViewModel(Cliente cliente)
        {
            ClientesContacto contactoPrincipal = cliente.ClientesContactos.Where(x => x.flgPrincipal == 1).FirstOrDefault();

            this.Id = cliente.Id;
            this.RazonSocial = cliente.RazonSocial;

            if (contactoPrincipal != null)
            {
                this.Email = contactoPrincipal.Email;
                this.Telefono = contactoPrincipal.Telefono;
            }

            this.Pais = cliente.Localidad.Provincia.Pais.Descripcion;
            this.Provincia = cliente.Localidad.Provincia.Descripcion;
            this.Localidad = cliente.Localidad.Descripcion;

            Estado estadoUltimaGestion = cliente.ClientesGestiones
                 .OrderByDescending(c => c.Fecha)
                 .Select(c => c.Estado).FirstOrDefault();

            if (estadoUltimaGestion != null)
            {
                this.Gestion = estadoUltimaGestion.Descripcion;
                this.FecUltGestion = estadoUltimaGestion.UpdatedDate;
            }

        }

        #endregion


    }
}