using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("LogsRegistrosSistema")]
    public class LogRegistroSistema : AuditableEntity<int>
    {
        public LogRegistroSistema(string desc, int userID, string observ = null)
        {
            this.UserProfileId = userID;
            this.DescripcionAccion = desc;
            this.ObservacionesAccion = observ;
            this.Fecha = DateTime.Now;
        }

        public LogRegistroSistema() { } //EF

        [Required]
        [Display(Name ="Usuario")]
        public int UserProfileId { get; set; }

        [Required]
        [Display(Name="Descripción del Log")]
        public string DescripcionAccion { get; set; }

        [Display(Name="Observaciones")]
        public string ObservacionesAccion { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile UserProfile { get; set; }

        
    }
}