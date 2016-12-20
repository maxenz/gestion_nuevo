using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Gestion.Models
{
    [Table("Clientes")]
    public class Cliente
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [Display(Name="Razón Social")]
        public String RazonSocial { get; set; }

        public String Calle { get; set; }

        public String Altura { get; set; }

        public String Piso { get; set; }

        public String Departamento { get; set; }

        public String Domicilio { get; set; }

        [Display(Name="Sitio Web")]
        [DataType(DataType.Url)]
        public String SitioWeb { get; set; }

        public String Referencia { get; set; }

        [Column(TypeName = "DateTime2")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime FechaIngreso { get; set; }

        [Required]
        [ForeignKey("Localidad")]
        public int LocalidadID { get; set; }

        public int? RevendedorID { get; set; }

        public int? CuentaCorrienteID { get; set; }

        [ForeignKey("MedioDifusion")]
        public int? MedioDifusionID { get; set; }

        public string Latitud { get; set; }

        public string Longitud { get; set; }

        public virtual IList<ClientesContacto> ClientesContactos { get; set; }
        public virtual IList<ClientesGestion> ClientesGestiones { get; set; }
        public virtual IList<ClientesTerminal> ClientesTerminales { get; set; }
        public virtual IList<ClientesUsuario> ClientesUsuarios { get; set; }
        public virtual IList<ClientesLicencia> ClientesLicencias { get; set; }
        public virtual Localidad Localidad { get; set; }
        public virtual MedioDifusion MedioDifusion { get; set; }

    }
}