using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model.Enums
{
    public enum GestionType
    {
        [Display(Name = "Programación")]
        Programming = 1,
        [Display(Name = "Gestión")]
        Management = 2,
        [Display(Name = "Todas")]
        Default = 3,
    }

}
