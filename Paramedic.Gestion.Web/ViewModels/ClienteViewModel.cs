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

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        public string Calle { get; set; }

        public string Altura { get; set; }

        public string Piso { get; set; }

        public string Departamento { get; set; }

        public string Domicilio { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public string Pais { get; set; }

        public string Provincia { get; set; }

        public string Localidad { get; set; }

        [Display(Name = "Sitio Web")]
        [DataType(DataType.Url)]
        public string SitioWeb { get; set; }

        public string Referencia { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        public int LocalidadId { get; set; }

        [Display(Name = "Revendedor")]
        public int? RevendedorId { get; set; }

        [Display(Name = "Cuenta Corriente")]
        public int? CuentaCorrienteId { get; set; }

        [Display(Name = "Medio de difusión")]
        public int? MedioDifusionId { get; set; }

        [Display(Name = "Email del contacto principal")]
        public string Email { get; set; }

        [Display(Name = "Teléfono del contacto principal")]
        public string Telefono { get; set; }

        [Display(Name = "Nombre del contacto principal")]
        public string Nombre { get; set; }

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
            this.LocalidadId = cliente.LocalidadId;
            this.SitioWeb = cliente.SitioWeb;
            this.RevendedorId = cliente.RevendedorId;
            this.CuentaCorrienteId = cliente.CuentaCorrienteId;
            this.Calle = cliente.Calle;
            this.Altura = cliente.Altura;
            this.Departamento = cliente.Departamento;
            this.Piso = cliente.Piso;
            this.MedioDifusionId = cliente.MedioDifusionId;

            Estado estadoUltimaGestion = cliente.ClientesGestiones
                 .OrderByDescending(c => c.Fecha)
                 .Select(c => c.Estado).FirstOrDefault();

            if (estadoUltimaGestion != null)
            {
                this.Gestion = estadoUltimaGestion.Descripcion;
                this.FecUltGestion = estadoUltimaGestion.UpdatedDate;
            }

        }

        public Cliente ClienteViewModelToCliente(ClienteViewModel clienteViewModel)
        {
            Cliente cliente = new Cliente();
            cliente.Calle = clienteViewModel.Calle;
            cliente.Altura = clienteViewModel.Altura;
            cliente.CuentaCorrienteId = clienteViewModel.CuentaCorrienteId;
            cliente.Departamento = clienteViewModel.Departamento;
            cliente.LocalidadId = clienteViewModel.LocalidadId;
            cliente.MedioDifusionId = clienteViewModel.MedioDifusionId;
            cliente.Piso = clienteViewModel.Piso;
            cliente.RazonSocial = clienteViewModel.RazonSocial;
            cliente.Referencia = clienteViewModel.Referencia;
            cliente.RevendedorId = clienteViewModel.RevendedorId;
            cliente.SitioWeb = clienteViewModel.SitioWeb;

            ClientesContacto principal = new ClientesContacto(clienteViewModel.Nombre, clienteViewModel.Email, clienteViewModel.Telefono, 1);
            cliente.ClientesContactos.Add(principal);

            return cliente;
        }

        #endregion
    }
}