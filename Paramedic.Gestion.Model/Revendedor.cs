using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("Revendedores")]
    public class Revendedor : AuditableEntity<int>
    {
        [Required]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Comisión")]
        public double Comision { get; set; }

        [Required]
        [Display(Name = "Contrato?")]
        public bool BajoContrato { get; set; }
    }
}