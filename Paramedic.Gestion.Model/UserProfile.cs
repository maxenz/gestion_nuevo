using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("UserProfile")]
    public class UserProfile : AuditableEntity<int>
    {
        public int UserId { get; set; }
        [Display(Name = "Usuario")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
    }
}
