using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model.Enums
{
    public enum TicketEventoType
    {
        [Display(Name="Consulta")]
        Question = 1,
        [Display(Name ="Respuesta")]
        Answer = 2
    }
}
