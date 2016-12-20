using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Gestion.Models
{
    [Table("LogsRegistrosSistema")]
    public class LogRegistroSistema
    {
        public LogRegistroSistema(string desc, int userID, string observ = null)
        {
            this.UserProfileID = userID;
            this.DescripcionAccion = desc;
            this.ObservacionesAccion = observ;
            this.Fecha = DateTime.Now;
        }

        public LogRegistroSistema() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required]
        [ForeignKey("UserProfile")]
        public int UserProfileID { get; set; }

        [Required]
        [Display(Name="Descripción del Log")]
        public string DescripcionAccion { get; set; }

        [Display(Name="Observaciones")]
        public string ObservacionesAccion { get; set; }

        [DataType(DataType.Date)]
        public DateTime Fecha { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        
    }
}