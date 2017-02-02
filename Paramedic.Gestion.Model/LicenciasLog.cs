using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("LicenciasLogs")]
    public class LicenciasLog : AuditableEntity<int>
    {
        #region Properties

        [Required]
        public int LicenciaId { get; set; }

        [Required]
        public string IP { get; set; }

        [ForeignKey("LicenciaId")]
        public virtual Licencia Licencia { get; set; }
        
        public LicenciasLogType Type { get; set; }

        public string GenericDescription { get; set; }

        public string Referencias { get; set; }

        #endregion

        #region Constructors

        public LicenciasLog() { } // EF

        public LicenciasLog(LicenciasLogType type, string description, string ip, int licenciaId)
        {
            this.Type = type;
            this.GenericDescription = description;
            if (string.IsNullOrEmpty(ip))
            {
                this.IP = "No se pudo obtener la ip";
            } else
            {
                this.IP = ip;
            }
            this.LicenciaId = licenciaId;

        }

        #endregion
    }
}