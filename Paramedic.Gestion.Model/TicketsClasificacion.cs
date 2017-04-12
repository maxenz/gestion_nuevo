using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Paramedic.Gestion.Model
{
    [Table("TicketsClasificaciones")]
    public class TicketsClasificacion : AuditableEntity<int>
    {
        #region Properties
        [Required]
        [Display(Name = "Clasificación")]
        public string Descripcion { get; set; }

        public string Color { get; set; }

        public bool AltaPrioridad { get; set; }

        public virtual ICollection<TicketsClasificacionUsuario> TicketsClasificacionesUsuarios { get; set; }

        #endregion

        #region Constructors

        public TicketsClasificacion() {

            this.TicketsClasificacionesUsuarios = new List<TicketsClasificacionUsuario>();

        } 

        public TicketsClasificacion(string descripcion, string color, bool altaPrioridad) : base()
        {
            this.Descripcion = descripcion;
            this.Color = color;
            this.AltaPrioridad = altaPrioridad;
        }

        #endregion
    }
}
