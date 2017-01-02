using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("LicenciasLogs")]
    public class LicenciasLog : AuditableEntity<int>
    {

        [Required]
        public int LicenciaId { get; set; }

        public int SolicitudId { get; set; }

        [Required]
        public string IP { get; set; }

        public string GenericDescription { get; set; }

        public string Referencias { get; set; }

        [ForeignKey("LicenciaId")]
        public Licencia Licencia { get; set; }

    }
}