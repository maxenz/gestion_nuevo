﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesLicencias")]
    public class ClientesLicencia : AuditableEntity<int>
    {
        [ForeignKey("Licencia")]
        [Display(Name = "Licencia")]
        public int LicenciaId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Required]
        [Display(Name = "DataSource")]
        public string CnnDataSource { get; set; }

        [Required]
        [Display(Name = "Catalog")]
        public string CnnCatalog { get; set; }

        [Required]
        [Display(Name = "Usuario")]
        public string CnnUser { get; set; }

        [Required]
        [Display(Name = "Password")]
        public string CnnPassword { get; set; }

        [DataType(DataType.Date)]
        [Column(TypeName = "DateTime2")]
        [Display(Name = "Fecha de Vencimiento")]
        public DateTime FechaDeVencimiento { get; set; }

        [Display(Name = "Servidor")]
        public int? ConexionServidorId  { get; set; }

        public virtual Sitio ConexionServidor { get; set; }

        [Display(Name = "Sitio")]
        public int? SitioId { get; set; }

        public virtual Sitio Sitio { get; set; }

        [Display(Name = "Web Service Cache")]
        public int? WebServiceCacheId { get; set; }

        public virtual Sitio WebServiceCache { get; set; }

        [Display(Name = "Puerto del Sitio")]
        [Range(0, int.MaxValue, ErrorMessage = "Por favor, ingrese un número válido para el puerto.")]
        public int? SitioPuerto { get; set; }

        public string Alias { get; set; }

        [Display(Name = "Password configurable en Android app")]
        public string AndroidPassword { get; set; }

        [Display(Name = "Url configurable en Android app")]
        public int? AndroidUrlId { get; set; }

        public virtual Sitio AndroidUrl { get; set; }

        [Display(Name = "Dirección Ftp para archivos de Android app")]
        public int? FtpAndroidDirId { get; set; }

        public virtual Sitio FtpAndroidDir { get; set; }

        [Display(Name = "Usuario para Ftp de archivos de Android app")]
        public string FtpAndroidUser { get; set; }

        [Display(Name = "Password para Ftp de archivos de Android app")]
        public string FtpAndroidPassword { get; set; }

        [Display(Name = "Subdominio")]
        public int? SitioSubDominioId { get; set; }

		[DataType(DataType.Date)]
		[Column(TypeName = "DateTime2")]
		[Display(Name = "Fecha de vencimiento del soporte")]
		public DateTime FechaVencimientoSoporte { get; set; }

		public virtual Sitio SitioSubDominio { get; set; }

        public virtual Cliente Cliente { get; set; }

        public virtual Licencia Licencia { get; set; }

        public virtual ICollection<ClientesLicenciasProducto> ClientesLicenciasProductos { get; set; }

        public virtual string FullUrl
        {
            get
            {
                if (this.Sitio == null) return string.Empty;
                if (this.SitioPuerto > 0)
                {
                    return string.Format("{0}:{1}", this.Sitio.Url, this.SitioPuerto);
                }
                else
                {
                    return this.Sitio.Url;
                }
            }

        }

        public virtual string ConnectionString
        {
            get
            {
                bool emptyDataSource = string.IsNullOrEmpty(this.CnnDataSource);
                bool emptyCatalog = string.IsNullOrEmpty(this.CnnCatalog);
                bool emptyUser = string.IsNullOrEmpty(this.CnnUser);
                bool emptyPassword = string.IsNullOrEmpty(this.CnnPassword);				
                if (!emptyDataSource && !emptyCatalog && !emptyUser && !emptyPassword && this.ConexionServidor != null)
                {
                    return string.Format("Data Source = {0}; Initial Catalog = {1}; User Id = {2}; Password = {3}",
                        this.ConexionServidor.Url,
                        this.CnnCatalog,
                        this.CnnUser,
                        this.CnnPassword);
                }
                else
                {
                    return null;
                }
            }
        }
    }
}