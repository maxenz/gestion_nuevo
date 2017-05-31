using System.ComponentModel.DataAnnotations;

namespace Paramedic.Gestion.Model.Enums
{
	public enum TrialStateType
	{
		[Display(Name = "Activo")]
		Active = 1,
		[Display(Name = "Vencido")]
		Expired = 2,
	}
}
