using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Estados")]
    public class Estado : AuditableEntity<int>
    {
        [Required]
        public int Numero{ get; set; }

        [Required]
        [Display(Name = "Estado")]
        public string Descripcion { get; set; }

    }
}