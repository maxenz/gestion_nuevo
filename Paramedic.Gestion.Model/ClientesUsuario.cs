using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("ClientesUsuarios")]
    public class ClientesUsuario : AuditableEntity<int>
    {

        [Required]
        [Display(Name = "Usuario")]
        public int UsuarioId { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public int ClienteId { get; set; }

        [Display(Name = "Shaman Full ID")]
        public int? ShamanFullId { get; set; }

        [Display(Name = "Shaman Express ID")]
        public int? ShamanExpressId { get; set; }

        [ForeignKey("ClienteId")]
        public virtual Cliente Cliente { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual UserProfile Usuario { get; set; }

    }
}