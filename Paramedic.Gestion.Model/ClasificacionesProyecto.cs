using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Paramedic.Gestion.Model
{
	[Table("ClasificacionesProyectos")]
	public class ClasificacionesProyecto : AuditableEntity<int>
	{
		#region Properties

		[Required]
		public string Descripcion { get; set; }

		#endregion
	}
}
