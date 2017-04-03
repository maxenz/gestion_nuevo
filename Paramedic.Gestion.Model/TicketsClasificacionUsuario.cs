using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
    [Table("TicketsClasificacionUsuarios")]
    public class TicketsClasificacionUsuario
    {
        #region Properties

        [ForeignKey("UserProfile")]
        [Display(Name = "Usuario")]
        [Key, Column(Order = 0)]
        public int UserProfileId { get; set; }

        [ForeignKey("TicketsClasificacion")]
        [Display(Name = "Clasificación del ticket")]
        [Key, Column(Order = 1)]
        public int TicketsClasificacionId { get; set; }

        public virtual UserProfile UserProfile { get; set; }

        public virtual TicketsClasificacion TicketsClasificacion { get; set; }

        #endregion

        #region Constructors

        public TicketsClasificacionUsuario() { } // EF

        public TicketsClasificacionUsuario(int userProfileId, int ticketsClasificacionId)
        {
            this.UserProfileId = userProfileId;
            this.TicketsClasificacionId = ticketsClasificacionId;
        }

        #endregion

    }
}
