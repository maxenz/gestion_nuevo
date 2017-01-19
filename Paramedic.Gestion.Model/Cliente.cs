using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Clientes")]
    public class Cliente : AuditableEntity<int>
    {
        #region Properties

        [Required]
        [Display(Name = "Razón Social")]
        public string RazonSocial { get; set; }

        public string Calle { get; set; }

        public string Altura { get; set; }

        public string Piso { get; set; }

        public string Departamento { get; set; }

        public string Domicilio { get; set; }

        [Display(Name = "Sitio Web")]
        [DataType(DataType.Url)]
        public string SitioWeb { get; set; }

        public string Referencia { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        [Column(TypeName = "DateTime2")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [Display(Name = "Localidad")]
        public int LocalidadId { get; set; }

        [Display(Name = "Revendedor")]
        public int? RevendedorId { get; set; }

        [Display(Name = "Cuenta Corriente")]
        public int? CuentaCorrienteId { get; set; }

        [Display(Name = "Medio de difusión")]
        public int? MedioDifusionId { get; set; }

        public virtual ICollection<ClientesContacto> ClientesContactos { get; set; }

        public virtual ICollection<ClientesGestion> ClientesGestiones { get; set; }

        public virtual ICollection<ClientesTerminal> ClientesTerminales { get; set; }

        public virtual ICollection<ClientesUsuario> ClientesUsuarios { get; set; }

        public virtual ICollection<ClientesLicencia> ClientesLicencias { get; set; }

        public virtual ICollection<VideosCliente> ClientesVideos { get; set; }

        [ForeignKey("LocalidadId")]
        public virtual Localidad Localidad { get; set; }

        [ForeignKey("MedioDifusionId")]
        public virtual MedioDifusion MedioDifusion { get; set; }

        public virtual string GeoAddress
        {
            get
            {
                return string.Format("{0} {1}, {2}, {3}, {4}", this.Altura, this.Calle, this.Localidad.Descripcion, this.Localidad.Provincia.Descripcion, this.Localidad.Provincia.Pais.Descripcion);
            }
        }

        #endregion

        #region Constructors

        public Cliente()
        {
            this.ClientesContactos = new List<ClientesContacto>();
            this.ClientesGestiones = new List<ClientesGestion>();
            this.ClientesLicencias = new List<ClientesLicencia>();
            this.ClientesTerminales = new List<ClientesTerminal>();
            this.ClientesUsuarios = new List<ClientesUsuario>();
        }

        #endregion
    }
}