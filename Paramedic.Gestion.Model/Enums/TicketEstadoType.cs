using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model.Enums
{
    public enum TicketEstadoType
    {
        [Display(Name ="Respondido")]
        Answered = 1,
        [Display(Name ="Sin responder")]
        NotAnswered = 2,
        [Display(Name ="Resuelto")]
        Resolved = 3
    }

}
