using Paramedic.Gestion.Model.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Paramedic.Gestion.Model
{
    [Table("TicketEventos")]
    public class TicketEvento : AuditableEntity<int>
    {
        #region Properties

        [Required(ErrorMessage = @"Debe ingresar la descripcion.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [Display(Name = "Ticket")]
        public int TicketId { get; set; }

        [Display(Name = "Usuario")]
        public int UserProfileId { get; set; }

        public byte[] ImageData { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ImageMimeType { get; set; }

        [Display(Name = "Tipo de evento")]
        public TicketEventoType TicketTipoEventoType { get; set; }

        [ForeignKey("TicketId")]
        public virtual Ticket Ticket { get; set; }

        [ForeignKey("UserProfileId")]
        public virtual UserProfile Usuario { get; set; }

        #endregion

        #region Constructors
        public TicketEvento() { } //EF

        public TicketEvento(string description, int userProfileId, TicketEventoType type)
        {
            if (string.IsNullOrEmpty(description))
            {
                if (this.Ticket != null)
                {
                    Descripcion = this.Ticket.Asunto;
                }
            }
            else
            {
                Descripcion = description;
            }

            UserProfileId = userProfileId;
            TicketTipoEventoType = type;
        }

        #endregion

    }
}
