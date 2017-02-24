using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Sitios")]
    public class Sitio : AuditableEntity<int>
    {

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Url { get; set; }
    }
}