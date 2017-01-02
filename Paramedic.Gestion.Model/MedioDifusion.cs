using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("MediosDifusion")]
    public class MedioDifusion : AuditableEntity<int>
    {
        [Required]
        [Display(Name="Visible en Mapa")]
        public bool MapaVisible { get; set; }

        [Required]
        public string Descripcion { get; set; }

    }
}