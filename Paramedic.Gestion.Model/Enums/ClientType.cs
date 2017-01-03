using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model.Enums
{
    public enum ClientType
    {
        [Display(Name = "Cliente normal")]
        Default = 1,
        [Display(Name = "Con licencia adquirida")]
        WithLicense = 2,
        [Display(Name = "En gestión")]
        InNegotiation = 3
    }

}
