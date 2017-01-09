using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesContactos")]
    public class ClientesContacto : AuditableEntity<int>
    {
        #region Properties

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public string Otros { get; set; }

        [Required]
        [Display(Name = "Principal")]
        public int flgPrincipal { get; set; }

        [Required]
        [Display(Name = "Institucional")]
        public bool esInstitucional { get; set; }

        [Required]
        public string Nombre { get; set; }

        [Display(Name = "Teléfono")]
        [DataType(DataType.PhoneNumber)]
        public String Telefono { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }

        #endregion

        #region Constructors

        public ClientesContacto() { } // EF

        public ClientesContacto(string Nombre, string Email, string Telefono, int Principal)
        {
            this.Nombre = Nombre;
            this.Email = Email;
            this.Telefono = Telefono;
            this.flgPrincipal = Principal;
        }

        #endregion

    }
}