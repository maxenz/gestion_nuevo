using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("TipoTerminales")]
    public class TipoTerminal : AuditableEntity<int>
    {
        [Required]
        [Display(Name="Terminal")]
        public string Descripcion { get; set; }
    }
}