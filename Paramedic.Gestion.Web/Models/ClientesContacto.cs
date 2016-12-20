using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Gestion.Models
{
    [Table("ClientesContactos")]
    public class ClientesContacto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [DataType(DataType.EmailAddress)]
        public String Email { get; set; }

        public String Otros { get; set; }

        [Required]
        [Display(Name="Principal")]
        public int flgPrincipal { get; set; }

        [Required]
        [Display(Name = "Institucional")]
        public bool esInstitucional { get; set; }

        [Required]
        public String Nombre { get; set; }

        [Display(Name="Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public String Telefono { get; set; }

        [Required]
        public int ClienteID { get; set; }

        public virtual Cliente Cliente { get; set; }

    }
}