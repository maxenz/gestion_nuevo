using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model
{
    [Table("TicketsClasificaciones")]
    public class TicketsClasificacion : AuditableEntity<int>
    {
        #region Properties

        [Display(Name = "Clasificación")]
        public string Descripcion { get; set; }

        public string Color { get; set; }

        public bool AltaPrioridad { get; set; }

        #endregion

        #region Constructors

        public TicketsClasificacion() { } // EF

        public TicketsClasificacion(string descripcion, string color, bool altaPrioridad)
        {
            this.Descripcion = descripcion;
            this.Color = color;
            this.AltaPrioridad = altaPrioridad;
        }

        #endregion
    }
}
